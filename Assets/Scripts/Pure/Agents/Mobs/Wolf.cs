using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Tilemaps;

public class Wolf : Mob
{
    private Vector2Int savedPosition;
    private bool friendly;

    public override float Value => 3;

    public override AgentData.AgentType AgentType => AgentData.AgentType.Carnivore;

    protected override string Mob_tag => "wolf";
    protected override State InitialState => new RandomWalk(this);


    public override void Use(Player user)
    {
        base.Use(user);
        friendly = false;
        state = new AttackState(this, user);
    }
    class ReturnToPosition : State
    {
        public ReturnToPosition(Wolf self) : base(self)
        {
            path = new List<Vector2Int>();
        }

        public override State Update()
        {
            if (AnyInRange<Mob>())
            {
                var target = Game.Instance.Agents.Minimum(x => Vector2Int.Distance(x.position, self.position));
                return new GotoState(self as Wolf, target);
            }

            if (path.Count <= 0)
            {
                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos);
                path = Astar.GetPath(self.position, (self as Wolf).savedPosition);
                Astar.CollisionDetection = astarColDetection;
                if (path.Count <= 0) return new RandomWalk(self as Wolf);
            }

            var step = path[0];
            if (collisionDetection(step))
            {
                path.Clear();
                return this;
            }
            self.Orientation = step - self.position;
            self.position = path[0];
            path.RemoveAt(0);
            return this;
        }
    }

    class RandomWalk : State
    {
        Vector2Int u;
        public RandomWalk(Wolf self) : base(self) { }
        public override State Update()
        {
            // -------------------------------------------- trying to find a deer
            var hit = Game.Instance.Agents.FindAll(x => x is Deer).Minimum(x => Vector2.Distance(x.position, self.position));
            if (hit != null)
            {
                hit = GameHelper.Raycast(self.position, hit.position);

                if (hit is Deer)
                {
                    return new GotoState(self as Wolf, hit);
                }
            }
            // -------------------------------------------- trying to find a lure
            hit = Game.Instance.Agents.FindAll(x => x is Lure).Minimum(x => Vector2.Distance(x.position, self.position));
            if (hit != null)
            {
                hit = GameHelper.Raycast(self.position, hit.position);
                if (hit is Deer)
                {
                    return new GotoState(self as Wolf, hit);
                }
            }

            // ---------------------------------------- random walk
            if (path.Count <= 0)
            {
                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos, self); //override collision detection method

                while (path.Count <= 0)
                {
                    var u = GameHelper.LinearRandom(Game.Instance.Level.Ground);
                    path = Astar.GetPath(self.position, u);
                }

                Astar.CollisionDetection = astarColDetection;
            }

            var step = path[0];
            if (collisionDetection(step))
            {
                path.Clear();
                return this;
            }
            self.Orientation = step - self.position;
            self.position = path[0];
            path.RemoveAt(0);

            return this;
        }
    }
    class GotoState : State
    {
        Vector2Int lastSeenAt;
        private Agent target;
        public GotoState(Wolf self, Agent target) : base(self) 
        {
            this.target = target;
        }
        public override State Update()
        {
            if (IsInRange(target))
            {
                lastSeenAt = target.position;
            }
            if (IsNextToMe(target))
            {
                if (target is Lure)
                {
                    return new EatState(self as Wolf, target);
                }
                else if (!(self as Wolf).friendly || !(target is Player))
                {
                    return new AttackState(self as Wolf, target);
                }
                else
                {
                    return this;
                }
            }
            else if (!IsInRange(target, self))
            {
                //follow player back after loosing track of foe
                if ((self as Wolf).friendly && !(target is Player) && IsInRange(Game.Instance.Player, self))
                {
                    return new GotoState(self as Wolf, Game.Instance.Player);
                }
                else
                {
                    (self as Wolf).friendly = false;
                    return new ReturnToPosition(self as Wolf);
                }
            }
            else
            {
                if (path.Count <= 0)
                {
                    var astarColDetection = Astar.CollisionDetection;
                    Astar.CollisionDetection = pos => collisionDetection(pos, target);
                    path = Astar.GetPath(self.position, lastSeenAt);
                    Astar.CollisionDetection = astarColDetection;
                    if (path.Count <= 0) return this;
                }

                var step = path[0];
                if (collisionDetection(step))
                {
                    path.Clear();
                    return this;
                }
                self.Orientation = step - self.position;
                self.position = path[0];
                path.RemoveAt(0);
                return this;
            }
        }
    }
    class EatState : State
    {
        private Agent target;
        public EatState(Wolf self, Agent target) : base(self)
        {
            this.target = target;
        }

        public override State Update()
        {
            Game.Instance.Destroy(target);
            if (IsInRange(Game.Instance.Player, self))
            {
                (self as Wolf).friendly = true;
                return new GotoState(self as Wolf , Game.Instance.Player);
            }
            else
            {
                return new GotoState(self as Wolf, target);
            }
        }
    }
    class AttackState : State
    {
        private Agent target;
        public AttackState(Wolf wolf, Agent target) : base(wolf)
        {
            this.target = target;
        }
        public override State Update()
        {
            bool isPlayer = target is Player;

            if (isPlayer) UIManager.Notifications.CreateNotification($"{self.data.visibleName} tries to hit you");
            if (target == self) return new RandomWalk(self as Wolf);
            var hit = GameHelper.CalculateHit(self, target as IStats);
            if (hit)
            {
                var dmg = GameHelper.CalculateDamage(self, target as IStats);
                var lifeBefore = (target as IStats).Stats.life;

                var s = (target as IStats).Stats;
                s.life -= dmg;
                (target as IStats).Stats = s;

                if (isPlayer) UIManager.Notifications.CreateNotification($"and deals {dmg} damage to you.");
                if ((target as IStats).Stats.life <= 0)
                {
                    return new RandomWalk(self as Wolf);
                }
            }
            if (IsNextToMe(target)) return this;
            else return new GotoState(self as Wolf, target);
        }
    }
}