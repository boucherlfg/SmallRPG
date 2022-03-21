using System.Collections.Generic;
using System.Linq;
using UnityEngine;






public class Room
{
    public Room precedent;
    public List<Room> Path
    {
        get
        {
            List<Room> ret = new List<Room>();
            Room temp = this;
            while (temp != null)
            {
                ret.Add(temp);
                temp = temp.precedent;
            }
            return ret;
        }
    }

    public static (int x, int y) Constraint => (15, 15);
    public Vector2Int position;
    public List<Vector2Int> doors;

    public Vector2Int topLeft;
    public Vector2Int bottomRight;
    public Vector2Int Size => bottomRight - topLeft;

    private List<Agent> agents;
    public List<Agent> Agents => agents;
    public List<Vector2Int> Ground => ground;
    private List<Vector2Int> ground;
    public bool discovered;

    public Room()
    {
        doors = new List<Vector2Int>();

        int x = Random.Range(0, Constraint.x / 2 - 3);
        int y = Random.Range(Constraint.y / 2 + 3, Constraint.y);
        topLeft = new Vector2Int(x, y);

        x = Random.Range(Constraint.x / 2 + 3, Constraint.x);
        y = Random.Range(0, Constraint.y / 2 - 3);
        bottomRight = new Vector2Int(x, y);

    }
    public override string ToString()
    {
        return $"({position.x}, {position.y})";
    }
    public virtual List<Agent> Generate()
    {
        ground = new List<Vector2Int>();
        agents = new List<Agent>();

        for (int x = topLeft.x; x <= bottomRight.x; x++)
        {
            agents.Add(new Wall { position = new Vector2Int(x + position.x * Constraint.x, topLeft.y + position.y * Constraint.y) });
            agents.Add(new Wall { position = new Vector2Int(x + position.x * Constraint.x, bottomRight.y + position.y * Constraint.y) });
        }

        for (int y = bottomRight.y; y <= topLeft.y; y++)
        {
            agents.Add(new Wall { position = new Vector2Int(topLeft.x + position.x * Constraint.x, y + position.y * Constraint.y) });
            agents.Add(new Wall { position = new Vector2Int(bottomRight.x + position.x * Constraint.x, y + position.y * Constraint.y) });
        }

        for (int x = topLeft.x + 1; x <= bottomRight.x - 1; x++)
        {
            for (int y = topLeft.y - 1; y >= bottomRight.y + 1; y--)
            {
                ground.Add(new Vector2Int(x + position.x * Constraint.x, y + position.y * Constraint.y));
            }
        }

        return agents.Distinct().ToList();
    }
    public List<Vector2Int> GetAvailableDirections(List<Room> existingRooms)
    {
        bool isAvailable(Vector2Int v) => !doors.Contains(v) && !existingRooms.Exists(r => r.position == position + v);

        var ret = new List<Vector2Int>();
        if (isAvailable(Vector2Int.up)) ret.Add(Vector2Int.up);
        if (isAvailable(Vector2Int.down)) ret.Add(Vector2Int.down);
        if (isAvailable(Vector2Int.left)) ret.Add(Vector2Int.left);
        if (isAvailable(Vector2Int.right)) ret.Add(Vector2Int.right);
        return ret;
    }

    public Agent CreateAtRandomPosition(System.Type type, List<Agent> referenceList)
    {
        var ctr = type.GetConstructor(new System.Type[] { });

        int x = Random.Range(topLeft.x + 2, bottomRight.x - 2);
        var y = Random.Range(bottomRight.y + 2, topLeft.y - 2);
        var position = new Vector2Int(Constraint.x * this.position.x + x, Constraint.y * this.position.y + y);

        while (referenceList.Exists(agent => agent.position == position))
        {
            x = Random.Range(topLeft.x + 2, bottomRight.x - 2);
            y = Random.Range(bottomRight.y + 2, topLeft.y - 2);
            position = new Vector2Int(Constraint.x * this.position.x + x, Constraint.y * this.position.y + y);
        }
        Agent agent = ctr.Invoke(new object[] { }) as Agent;
        agent.position = position;

        return agent;
    }
    public T CreateAtRandomPosition<T>(List<Agent> referenceList) where T : Agent, new()
    {
        int x = Random.Range(topLeft.x + 2, bottomRight.x - 2);
        var y = Random.Range(bottomRight.y + 2, topLeft.y - 2);
        var position = new Vector2Int(Constraint.x * this.position.x + x, Constraint.y * this.position.y + y);

        while (referenceList.Exists(agent => agent.position == position))
        {
            x = Random.Range(topLeft.x + 2, bottomRight.x - 2);
            y = Random.Range(bottomRight.y + 2, topLeft.y - 2);
            position = new Vector2Int(Constraint.x * this.position.x + x, Constraint.y * this.position.y + y);
        }
        
        return new T { position = position };
    }
}