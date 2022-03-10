using System.Collections.Generic;

public class AgentComparer : EqualityComparer<Agent>
{
    public override bool Equals(Agent x, Agent y)
    {
        return x.GetType() == y.GetType() && x.position == y.position;
    }

    public override int GetHashCode(Agent obj)
    {
        return obj.GetHashCode();
    }
}