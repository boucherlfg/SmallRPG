using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level
{
    public List<Vector2Int> Ground => ground;
    public List<Agent> Agents => agents;
    public List<Room> Rooms => rooms;

    protected List<Agent> agents;
    protected List<Room> rooms;
    protected List<Vector2Int> ground;

    public Level(int size)
    {
        Room MakeNewRoom<T>() where T : Room, new()
        {
            Room room = rooms[Random.Range(0, rooms.Count)];
            var available = room.GetAvailableDirections(rooms);

            while (available.Count <= 0)
            {
                room = rooms[Random.Range(0, rooms.Count)];
                available = room.GetAvailableDirections(rooms);
            }

            var dir = available[Random.Range(0, available.Count)];
            room.doors.Add(dir);
            room = new T { position = room.position + dir };
            room.doors.Add(-dir);


            return room;
        }

        rooms = new List<Room>();
        Room room = new StartingRoom { position = Vector2Int.zero };
        rooms.Add(room);

        while (rooms.Count < size - 1)
        {
            room = MakeNewRoom<Room>();
            rooms.Add(room);
        }
        room = MakeNewRoom<EndRoom>();
        rooms.Add(room);
    }

    public void Generate()
    {
        ground = new List<Vector2Int>();
        agents = new List<Agent>();
        AddWallsAndStartEnd();
        AddCorridors();
        AddFoes();
        AddTraps();
        AddFurniture();
        AddLoot();
        AddResources();
        AddCraftingStations();
    }

    private void AddWallsAndStartEnd()
    {
        //generate room walls and start / end points
        rooms.ForEach(room =>
        {
            agents.AddRange(room.Generate());
            ground.AddRange(room.Ground);
        });
        //walls may have overlapped, so we delete the twins
        agents = agents.Distinct(new AgentComparer()).ToList();

        //we add entrance, plater and exit
        Room room = rooms.Find(x => x is StartingRoom);
        Agent agent = room.CreateAtRandomPosition<Entrance>(Agents);
        agents.Add(agent);
        agent = room.CreateAtRandomPosition<Player>(Agents);
        agents.Add(agent);
        room = rooms.Find(x => x is EndRoom);
        agent = room.CreateAtRandomPosition<Exit>(Agents);
        agents.Add(agent);
        
    }
    private void AddFoes()
    {
        Mob RandomAgent(Room room)
        {
            var types = GameHelper.GetAllSubTypes<Mob>();
            var type = GameHelper.LinearRandom(types);
            return room.CreateAtRandomPosition(type, room.Agents) as Mob;
        }
        float foeBudget = rooms.Count;
        
        while (foeBudget > 0)
        {
            var room = GameHelper.LinearRandom(rooms.FindAll(x => !(x is StartingRoom)));
            var mob = RandomAgent(room);
            agents.Add(mob);
            foeBudget -= mob.Value;
        }
    }
    private void AddFurniture()
    {
        float furnitureBudget = rooms.Count;

        while (furnitureBudget > 0)
        {
            FurnitureAgent furniture = null;
            var room = GameHelper.LinearRandom(rooms.FindAll(x => !(x is StartingRoom)));

            //is there a furniture with nothing next to it?
            var agent = agents.Find(x => x is FurnitureAgent && !agents.Exists(y => Mathf.Abs(1 - Vector2.Distance(x.position, y.position)) < 0.5f));
            if (agent == null)
            {
                furniture = room.CreateAtRandomPosition<FurnitureAgent>(agents);
            }
            else
            {
                var pos = agent.position + GameHelper.RandomCardinal();
                furniture = new FurnitureAgent { position = pos };
            }
            agents.Add(furniture);
            furnitureBudget--;
        }
    }
    private void AddTraps()
    {
        float trapBudget = rooms.Count/2;

        while (trapBudget > 0)
        {
            var room = GameHelper.LinearRandom(rooms.FindAll(x => !(x is StartingRoom)));
            var trap = room.CreateAtRandomPosition<TrapAgent>(agents);
            agents.Add(trap);
            trapBudget--;
        }
    }
    private void AddLoot()
    {
        float lootBudget = rooms.Count;
        while (lootBudget > 0)
        {
            var nextPrice = Random.Range(3, 5);
            var room = rooms[Random.Range(0, rooms.Count)];
            var loot = room.CreateAtRandomPosition<Loot>(room.Agents);
            loot.loot = GameHelper.PopulateLoot(nextPrice);
            agents.Add(loot);
            lootBudget -= nextPrice;
        }
    }
    private void AddResources()
    {
        var types = GameHelper.GetAllSubTypes<Source>();
        float resourceBudget = rooms.Count * 2;
        while (resourceBudget > 0)
        {
            var choice = GameHelper.LinearRandom(types);
            var room = GameHelper.LinearRandom(rooms);
            var agent = room.CreateAtRandomPosition(choice, agents);
            agents.Add(agent);
            resourceBudget--;
        }
    }
    private void AddCraftingStations()
    {
        var types = GameHelper.GetAllSubTypes<CraftingStation>();
        var roomList = new List<Room>();
        foreach (var type in types)
        {
            var room = GameHelper.LinearRandom(rooms);
            while (roomList.Contains(room)) room = GameHelper.LinearRandom(rooms);
            var agent = room.CreateAtRandomPosition(type, agents);
            agents.Add(agent);
        }
        
    }
    private void AddCorridors()
    {
        var doneRooms = new List<Room>();
        rooms.ForEach(room =>
        {
            doneRooms.Add(room);
            room.doors.ForEach(door =>
            {
                if (doneRooms.Exists(x => x.position == room.position + door))
                {
                    return;
                }
                var center = new Vector2Int(
                    Mathf.FloorToInt(Room.Constraint.x * room.position.x + Room.Constraint.x / 2),
                    Mathf.FloorToInt(Room.Constraint.y * room.position.y + Room.Constraint.y / 2));

                var topLeft = new Vector2Int(
                    room.topLeft.x + room.position.x * Room.Constraint.x,
                    room.topLeft.y + room.position.y * Room.Constraint.y);

                var bottomRight = new Vector2Int(
                    room.bottomRight.x + room.position.x * Room.Constraint.x,
                    room.bottomRight.y + room.position.y * Room.Constraint.y);


                var start = Vector2Int.zero;
                if (door == Vector2Int.up)
                {
                    start = new Vector2Int(center.x, topLeft.y);
                }
                else if (door == Vector2Int.down)
                {
                    start = new Vector2Int(center.x, bottomRight.y);
                }
                else if (door == Vector2Int.left)
                {
                    start = new Vector2Int(topLeft.x, center.y);
                }
                else if (door == Vector2Int.right)
                {
                    start = new Vector2Int(bottomRight.x, center.y);
                }
                else throw new System.Exception();

                Vector2Int v = start;
                agents.Remove(agents.Find(r => r.position == v - door));
                agents.Remove(agents.Find(r => r.position == v));

                ground.Add(new Vector2Int(v.x, v.y));
                ground.Add(new Vector2Int(v.x - door.x, v.y - door.y));
                
                agents.Add(new Wall { position = v + new Vector2Int(door.y, -door.x) });
                agents.Add(new Wall { position = v - new Vector2Int(door.y, -door.x) });

                for (; !agents.Exists(r => r.position == v + door); v += door)
                {
                    agents.Remove(agents.Find(r => r.position == v));

                    ground.Add(new Vector2Int(v.x, v.y));

                    agents.Add(new Wall { position = v + new Vector2Int(door.y, -door.x) });
                    agents.Add(new Wall { position = v - new Vector2Int(door.y, -door.x) });
                }
                agents.Remove(agents.Find(r => r.position == v));
                agents.Remove(agents.Find(r => r.position == v + door));

                ground.Add(new Vector2Int(v.x, v.y));
                ground.Add(new Vector2Int(v.x + door.x, v.y + door.y));

                agents.Add(new Wall { position = v + new Vector2Int(door.y, -door.x) });
                agents.Add(new Wall { position = v - new Vector2Int(door.y, -door.x) });
            });
        });

    }
}