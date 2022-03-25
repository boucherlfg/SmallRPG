using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bandit : Mob
{
    private Vector2Int savedPosition;
    public override float Value => 3;
    protected override State InitialState => new RandomWalk(this);
    public override void Activate(IMovable user)
    {
        base.Activate(user);
        state = new AttackState(this, user as Agent);
    }

    public override AgentData.AgentType AgentType => AgentData.AgentType.Bandit;

    public abstract class BanditState : State
    {
        public BanditState(Bandit self) : base(self) { }

        public Agent FindATarget()
        {
            var all = GameHelper.FOV(self.position, self.detectionTreshold);

            // -------------------------------------------- trying to find a lure of type bandit
            var hit = all.Find(x => x is LureAgent && (x as LureAgent).lure.type == Lure.LureType.Bandit);
            if (hit != null)
            {
                return hit;
            }
            //------------------------------------------- trying to find player
            hit = all.Find(x => x is Player);
            if (hit != null)
            {
                return hit;
            }

            // -------------------------------------------- trying to find a deer
            hit = all.Find(x => x is Deer);
            if (hit != null)
            {
                return hit;
            }

            // -------------------------------------------- trying to find a wolf
            hit = all.Find(x => x is Wolf);
            if (hit != null)
            {
                return hit;
            }

            return null;
        }
    }
    public class ReturnToPosition : BanditState
    {
        public ReturnToPosition(Bandit self) : base(self)
        {
            path = new List<Vector2Int>();
        }

        public override State Update()
        {
            var target = FindATarget();
            if (target != null)
            {
                return new GotoState(self as Bandit, target);
            }

            //go to start position
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
    public class GotoState : BanditState
    {
        Vector2Int lastSeenAt;
        private Agent target;
        public GotoState(Bandit self, Agent target) : base(self)
        {
            this.target = target;
            self.savedPosition = self.position;
            path = new List<Vector2Int>();
            lastSeenAt = target.position;
        }


        public override State Update()
        {
            //if we're following ourself, we skur ASAP
            if (target == self) return new RandomWalk(self as Bandit);

            var all = GameHelper.FOV(self.position, self.detectionTreshold);

            bool isInRange = all.Exists(x => x == target);
            var player = Game.Instance.Player;

            //if we were following a lure, and we get next to it, then we eat it. 
            if (target is LureAgent && IsNextToMe(target))
            {
                return new EatState(self as Bandit, target);
            }

            //if we're not following the player, we try to find the player (bc we're obsessed with him)
            if (!(target is Player))
            {
                var hit = all.Find(x => x is Player);
                if (hit != null)
                {
                    return new GotoState(self as Bandit, hit);
                }
            }

            if (all.Exists(x => x == target))
            {
                lastSeenAt = target.position;
            }

            if (path.Count <= 0)
            {
                if (isInRange) //path is done and target is still in range
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
    public class RandomWalk : BanditState
    {       
        public RandomWalk(Bandit self) : base(self) 
        {
            path = new List<Vector2Int>();
        }


        public override State Update()
        {
            var target = FindATarget();

            if (target != null)
            {
                return new GotoState(self as Bandit, target);
            }

            // ---------------------------------------- go to random position            
            if (path.Count <= 0)
            {
                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos, self); //override collision detection method
                var u = GameHelper.LinearRandom(Game.Instance.Level.Ground);
                path = Astar.GetPath(self.position, u);
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
    public class AttackState : BanditState
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
                if (isPlayer)
                {
                    UIManager.Notifications.CreateNotification($"and deals {dmg} damage to you.");
                    DisplayManager.Instance.CreateDamageText(dmg, target.position);
                    DataModel.Equipment.Damage(EquipType.Body, EquipType.Leg, EquipType.Head);
                    
                }
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
    public class EatState : BanditState
    {
        private Agent target;
        public EatState(Bandit self, Agent target) : base(self)
        {
            this.target = target;
        }

        public override State Update()
        {
            Game.Instance.Destroy(target);
            return new RandomWalk(self as Bandit);
        }
    }

}