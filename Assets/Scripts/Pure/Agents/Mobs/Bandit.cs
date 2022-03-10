using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bandit : Mob
{
    private Vector2Int savedPosition;
    public override float Value => 3;

    private string mob_tag = "bandit";
    protected override string Mob_tag => mob_tag;
    protected override State InitialState => new RandomWalk(this);
    public override void Use(Player user)
    {
        base.Use(user);
        state = new AttackState(this, user);
    }

    public override AgentData.AgentType AgentType => AgentData.AgentType.Bandit;

    class ReturnToPosition : State
    {
        public ReturnToPosition(Bandit self) : base(self)
        {
            path = new List<Vector2Int>();
        }

        public override State Update()
        {
            if (IsInRange(Game.Instance.Player, self))
            {
                return new GotoState(self as Bandit, Game.Instance.Player);
            }
            if (AnyInRange<Mob>())
            {
                var target = Game.Instance.Agents.Minimum(x => Vector2Int.Distance(x.position, self.position));
                return new GotoState(self as Bandit, target);
            }
            if (path.Count <= 0)
            {
                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos);
                path = Astar.GetPath(self.position, (self as Bandit).savedPosition);
                Astar.CollisionDetection = astarColDetection;
                if (path.Count <= 0) return new RandomWalk(self as Bandit);
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
        public GotoState(Bandit self, Agent target) : base(self)
        {
            this.target = target;
            self.savedPosition = self.position;
            path = new List<Vector2Int>();
            lastSeenAt = target.position;
            if (target is Player && Random.value < 0.3)
            {
                AudioManager.PlayAsSound(self.Mob_tag);
            }
        }
        public override State Update()
        {
            
            if (target == self) return new RandomWalk(self as Bandit);

            if (!(target is Player))
            {
                //------------------------------------------- trying to find player
                var hit = GameHelper.Raycast(self.position, Game.Instance.Player.position);
                if (hit == Game.Instance.Player)
                {
                    return new GotoState(self as Bandit, hit);
                }
            }
            else if (IsInRange(target))
            {
                lastSeenAt = target.position;
            }
            if (path.Count == 0)
            {
                if (IsInRange(target, self)) //path is done and target is still in range
                {
                    lastSeenAt = target.position;

                    var astarColDetection = Astar.CollisionDetection;
                    Astar.CollisionDetection = pos => collisionDetection(pos, target);
                    path = Astar.GetPath(self.position, lastSeenAt);
                    Astar.CollisionDetection = astarColDetection;
                    return this;
                }
                else //path is done but target is not in range anymor
                {
                    return new ReturnToPosition(self as Bandit);
                }
            }
            else //path is not done
            {
                var step = path[0];
                if (collisionDetection(step))
                {
                    path.Clear();
                    return this;
                }
                self.Orientation = step - self.position;
                self.position = path[0];
                path.RemoveAt(0);

                if (IsNextToMe(target))
                {
                    return new AttackState(self as Bandit, target);
                }
                else
                {
                    return this;
                }
            }
        }
    }
    class RandomWalk : State
    {       
        public RandomWalk(Bandit self) : base(self) 
        {
            path = new List<Vector2Int>();
        }
        public override State Update()
        {
            //--------------------------- update state

            //------------------------------------------- trying to find player
            var hit = GameHelper.Raycast(self.position, Game.Instance.Player.position);
            if (hit == Game.Instance.Player)
            {
                return new GotoState(self as Bandit, hit);
            }


            // -------------------------------------------- trying to molest a deer
            hit = Game.Instance.Agents.FindAll(x =>
            {
                return x is Deer;
            }).Minimum(x =>
            {
                return Vector2.Distance(x.position, self.position);
            });
            if (hit != null)
            {
                hit = GameHelper.Raycast(self.position, hit.position);
                if (hit is Deer)
                {
                    return new GotoState(self as Bandit, hit);
                }
            }

            // -------------------------------------------- trying to molest a wolf
            hit = Game.Instance.Agents.FindAll(x => 
            {
                return x is Wolf;
            }).Minimum(x => 
            { 
                return Vector2.Distance(x.position, self.position); 
            });
            if (hit != null)
            {
                hit = GameHelper.Raycast(self.position, hit.position);
                if (hit is Wolf)
                {
                    return new GotoState(self as Bandit, hit);
                }
            }


            // ---------------------------------------- go to random position            
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
    class AttackState : State
    {
        private Agent target;
        public AttackState(Bandit self, Agent target) : base(self)
        {
            this.target = target;
        }
        public override State Update()
        {
            bool isPlayer = target is Player;

            if (isPlayer) UIManager.Notifications.CreateNotification($"{self.data.visibleName} tries to hit you");
            var hit = GameHelper.CalculateHit(self, target as IStats);
            if (hit)
            {
                var dmg = GameHelper.CalculateDamage(self, target as IStats);
                
                var s = (target as IStats).Stats;
                s.life -= dmg;
                (target as IStats).Stats = s;
                if(isPlayer) UIManager.Notifications.CreateNotification($"and deals {dmg} damage to you.");
                if ((target as IStats).Stats.life <= 0)
                {
                    return new RandomWalk(self as Bandit);
                }
            }
            else
            {
                if (isPlayer) UIManager.Notifications.CreateNotification($"and misses.");
            }
            if (IsNextToMe(target)) return this;
            else return new GotoState(self as Bandit, target);
        }
    }
}