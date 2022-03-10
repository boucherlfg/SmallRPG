using UnityEngine;

public class Deer : Mob
{
    public override float Value => 3;

    public override AgentData.AgentType AgentType => AgentData.AgentType.Herbivore;

    protected override State InitialState => new RandomWalk(this);
    protected override string Mob_tag => "deer";

    class RandomWalk : State
    {
        public RandomWalk(Mob self) : base(self)
        {
        }
        public override State Update()
        {
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
    class Flee : State
    {
        private Agent target;
        public Flee(Deer self, Agent toFlee) : base(self)
        {
            this.target = toFlee;
        }
        public override State Update()
        {
            if (!IsInRange(target, self))
            {
                return new RandomWalk(self);
            }

            if (!collisionDetection(self.position + (target as IMovable).Orientation))
            {
                self.position += (target as IMovable).Orientation;
                return this;
            }

            if (path.Count == 0)
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

            var next = path[0];
            if (collisionDetection(next))
            {
                path.Clear();
                return this;
            }
            path.RemoveAt(0);
            self.Orientation = next - self.position;
            self.position = next;
            return this;
        }
    }
}