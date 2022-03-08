using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bandit : Mob
{
    private Vector2Int savedPosition;
    public override float Value => 3;
    protected override string Mob_tag => "bandit";
    protected override State InitialState => new RandomWalk(this);

    public UseType UseType => UseType.Attack;
    
    class ReturnToPosition : State
    {
        public ReturnToPosition(Bandit self) : base(self)
        {
            path = new List<Vector2Int>();
        }

        public override State Update()
        {
            if (IsInRange(Game.Instance.Player))
            {
                UIManager.Notifications.CreateNotification($"a {self.data.visibleName} has found you!");
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
            lastSeenAt = Game.Instance.Player.position;
        }
        public override State Update()
        {
            if (path.Count == 0)
            {
                if (IsInRange(target)) //path is done and target is still in range
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
                    if (target is Player)
                    {
                        UIManager.Notifications.CreateNotification($"a {self.data.visibleName} has lost you");
                    }
                    return new ReturnToPosition(self as Bandit);
                }
            }
            else //path is not done
            {
                var step = path[0];

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
            if (IsInRange(Game.Instance.Player)) 
            {
                UIManager.Notifications.CreateNotification($"a {self.data.visibleName} has found you!");
                return new GotoState(self as Bandit, Game.Instance.Player);
            }
            if (AnyInRange<Mob>())
            {
                var target = Game.Instance.Agents.Minimum(x => Vector2Int.Distance(x.position, self.position));
                return new GotoState(self as Bandit, target);
            }

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
                (target as IStats).Life -= dmg;
                if(isPlayer) UIManager.Notifications.CreateNotification($"and deals {dmg} damage to you.");
                if ((target as IStats).Life <= 0)
                {
                    Game.Instance.Destroy(target);
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