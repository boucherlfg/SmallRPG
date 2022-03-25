using System;
using UnityEngine;

public class ExcludeAttribute : Attribute { }

[Exclude]
public class Ally : Mob
{
    public int duration = 100;
    public override AgentData.AgentType AgentType => AgentData.AgentType.Ally;

    public override float Value => 0;

    protected override State InitialState => new FollowPlayerState(this);

    public override void Update()
    {
        base.Update();
        if (duration < 0) Game.Instance.Destroy(this);
        duration--;
    }

    public class FollowPlayerState : State 
    {
        public FollowPlayerState(Ally self) : base(self)
        {
        }
        public override State Update()
        {
            var all = GameHelper.FOV(self.position, 5);

            var enemy = all.Find(x => x is Mob && x != self);
            if (enemy != null)
            {
                return new GoToState(self, enemy);
            }

            //go to start position
            if (path.Count <= 0)
            {
                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos, Game.Instance.Player);
                path = Astar.GetPath(self.position, Game.Instance.Player.position);
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
    public class GoToState : State
    {
        private Agent target;

        public GoToState(Mob self, Agent target) : base(self)
        {
            this.target = target;
        }

        public override State Update()
        {
            if (!Game.Instance.Agents.Contains(target)) return new FollowPlayerState(self as Ally);

            //go to start position
            if (path.Count <= 0)
            {
                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos, target);
                path = Astar.GetPath(self.position, target.position);
                Astar.CollisionDetection = astarColDetection;
                if (path.Count <= 0) return this;
            }

            if (IsNextToMe(target))
            {
                var hit = GameHelper.CalculateHit(self, target as IStats);
                if (hit)
                {
                    var dmg = GameHelper.CalculateDamage(self, target as IStats);

                    var s = (target as IStats).Stats;
                    s.life -= dmg;
                    DisplayManager.Instance.CreateDamageText(dmg, target.position);
                    (target as IStats).Stats = s;

                    if ((target as IStats).Stats.life <= 0)
                    {
                        return new FollowPlayerState(self as Ally);
                    }
                }
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