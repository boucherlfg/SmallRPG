using UnityEngine;

public class Deer : Mob
{
    public override float Value => 3;

    public override AgentData.AgentType AgentType => AgentData.AgentType.Herbivore;

    protected override State InitialState => new RandomWalk(this);
    protected override string Mob_tag => "deer";

    class RandomWalk : State
    {
        public RandomWalk(Deer self) : base(self)
        {
        }

        public override string Message => "random walk";

        public override State Update()
        {
            // -------------------------------------------- trying to find a lure of type deer
            var hit = Game.Instance.Agents.FindAll(x => x is LureAgent && (x as LureAgent).lure.type == Lure.LureType.Carnivore).Minimum(x => Vector2.Distance(x.position, self.position));
            if (hit != null)
            {
                hit = GameHelper.Raycast(self.position, hit.position, self.detectionTreshold);
                if (hit is LureAgent)
                {
                    return new GoToState(self as Deer, hit);
                }
            }

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
    class Flee : State
    {
        private Agent target;
        public Flee(Deer self, Agent toFlee) : base(self)
        {
            this.target = toFlee;

            Debug.Log(self.ID + " flees " + toFlee.ID);
        }

        public override string Message => "flees " + target.ID;

        public override State Update()
        {
            if (target == self) return new RandomWalk(self as Deer);
            if (!IsInRange(target, self))
            {
                return new RandomWalk(self as Deer);
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
                var u = GameHelper.LinearRandom(Game.Instance.Level.Ground);
                path = Astar.GetPath(self.position, u);
                Astar.CollisionDetection = astarColDetection;

                if (path.Count <= 0) return this;
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
    class GoToState : State
    {
        private Agent target;
        public GoToState(Deer self, Agent target) : base(self) 
        {
            this.target = target;
        }

        public override string Message => "go to " + target.ID;

        public override State Update()
        {
            if (target == self) return new RandomWalk(self as Deer);
            var isInRange = IsInRange(target, self);
            if (!isInRange)
            {
                return new RandomWalk(self as Deer);
            }
            else
            {
                //if target is next to us, we attack/eat it
                if (IsNextToMe(target))
                {
                    Game.Instance.Destroy(target);
                    return new RandomWalk(self as Deer);
                }
                else
                {
                    if (path.Count <= 0)
                    {
                        var astarColDetection = Astar.CollisionDetection;
                        Astar.CollisionDetection = pos => collisionDetection(pos, target);
                        path = Astar.GetPath(self.position, target.position);
                        Astar.CollisionDetection = astarColDetection;
                        return this;
                    }
                    else
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
                        return this;
                    }
                }
            }
        }
    }
}