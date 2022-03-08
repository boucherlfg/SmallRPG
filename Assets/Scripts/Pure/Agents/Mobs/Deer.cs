using UnityEngine;

public class Deer : Mob 
{
    public override float Value => 3;
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
    class Flee : State
    {
        private Agent target;
        public Flee(Deer self, Agent toFlee) : base(self)
        {
            this.target = toFlee;
        }
        public override State Update()
        {
            if (!IsInRange(target))
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
                var newPosition = Vector2Int.CeilToInt(Random.insideUnitCircle * 15);
                while (collisionDetection(newPosition)) newPosition = Vector2Int.CeilToInt(Random.insideUnitCircle * 15);

                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos, target);
                path = Astar.GetPath(self.position, newPosition);
                Astar.CollisionDetection = astarColDetection;
            }

            var next = path[0];
            path.RemoveAt(0);
            self.Orientation = next - self.position;
            self.position = next;
            return this;
        }
    }
}