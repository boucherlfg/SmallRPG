using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Wolf : Mob
{
    private Vector2Int savedPosition;
    private bool friendly;

    public override float Value => 3;
    protected override string Mob_tag => "wolf";
    protected override State InitialState => new RandomWalk(this);


    public override void Use(Player user)
    {
        base.Use(user);
        friendly = false;
        state = new GotoState(this, user);
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
            if (AnyInRange<Deer>())
            {
                var deer = Game.Instance.Agents.Find(x => IsInRange(x));
                (self as Wolf).savedPosition = self.position;
                return new GotoState(self as Wolf, deer);
            }
            else if (AnyInRange<Lure>())
            {
                var lure = Game.Instance.Agents.Find(x => IsInRange(x));
                return new GotoState(self as Wolf, lure);
            }
            else
            {
                if (path.Count <= 0)
                {
                    var u = Vector2Int.RoundToInt(10 * Random.insideUnitCircle);
                    u += self.position;

                    var astarColDetection = Astar.CollisionDetection;
                    Astar.CollisionDetection = pos => collisionDetection(pos);
                    path = Astar.GetPath(self.position, u);
                    Astar.CollisionDetection = astarColDetection;
                    if (path.Count <= 0) return this;
                }

                var step = path[0];
                self.Orientation = step - self.position;
                self.position = path[0];
                path.RemoveAt(0);

                return this;
            }
        }
    }
    class GotoState : State
    {
        private Agent target;
        public GotoState(Wolf self, Agent target) : base(self) 
        {
            this.target = target;
        }
        public override State Update()
        {
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
            else if (!IsInRange(target))
            {
                //follow player back after loosing track of foe
                if ((self as Wolf).friendly && !(target is Player) && IsInRange(Game.Instance.Player))
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
                    var lastSeenAt = target.position;

                    var astarColDetection = Astar.CollisionDetection;
                    Astar.CollisionDetection = pos => collisionDetection(pos, target);
                    path = Astar.GetPath(self.position, lastSeenAt);
                    Astar.CollisionDetection = astarColDetection;
                    if (path.Count <= 0) return this;
                }

                var step = path[0];
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
            if (IsInRange(Game.Instance.Player))
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
            var hit = GameHelper.CalculateHit(self, target as IStats);
            if (hit)
            {
                var dmg = GameHelper.CalculateDamage(self, target as IStats);
                (target as IStats).Life -= dmg;
                if ((target as IStats).Life <= 0)
                {
                    Game.Instance.Destroy(target);
                }
            }
            if (IsNextToMe(target)) return this;
            else return new GotoState(self as Wolf, target);
        }
    }
}