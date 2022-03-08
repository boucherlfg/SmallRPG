using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public abstract class Mob : Agent, IStats, IMovable, IDrawable, IUpdatable, ICollision, IUsableAgent, IEndable
{
    
    protected List<string> loot;
    public AgentData data;
    protected State state;
    public int detectionTreshold = 15;

    public Mob()
    {
        Orientation = Vector2Int.down;

        data = Codex.Agents[Mob_tag];
        loot = GameHelper.PopulateLoot(data);

        Life = (int)data.statblock.life;
        Mana = (int)data.statblock.mana;
        Attack = (int)data.statblock.attack;
        Defense = (int)data.statblock.defense;
        Precision = (int)data.statblock.precision;
        Evasion = (int)data.statblock.evasion;

        state = InitialState;
    }

    #region [stats]
    public int Life { get; set; }
    public int Mana { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Precision { get; set; }
    public int Evasion { get; set; }
    #endregion

    public abstract float Value { get; }
    protected abstract State InitialState { get; }
    protected abstract string Mob_tag { get; }
    public virtual Tile CurrentTile => DisplayManager.Instance[Mob_tag];
    public Vector2Int Orientation { get; set; }
    public bool Immobilized { get; set; }

    public virtual void End()
    {
        loot.ForEach(item =>
        {
            Game.Instance.Create(new FloorItem(Codex.Items[item]) { position = position });
        });
        UIManager.Notifications.CreateNotification($"The {data.visibleName} crumbles in agony.");
    }
    public virtual void Update()
    {
        Immobilized = false;
        state = state.Update();
    }
    public virtual void Use(Player user)
    {
        var tool = DataModel.Equipment.Tool;
        if (tool == null || !(tool is Tool) || (tool as Tool).useType != UseType.Attack)
        {
            UIManager.Notifications.CreateNotification("Get a weapon before you attack!");
            return;
        }

        UIManager.Notifications.CreateNotification("you attempt to attack an enemy...");
        if (!GameHelper.CalculateHit(user, this))
        {
            UIManager.Notifications.CreateNotification("but your attack misses.");
            return;
        }
        var damage = GameHelper.CalculateDamage(user, this);
        UIManager.Notifications.CreateNotification($"and you manage to deal {damage} damage.");
        Life -= damage;
        if (Life <= 0)
        {
            Game.Instance.Destroy(this);
        }
    }
    protected abstract class State
    {
        protected List<Vector2Int> path;
        protected Mob self;
        public State(Mob self) 
        {
            this.self = self;
            path = new List<Vector2Int>();
        }
        public abstract State Update();

        protected bool PlayerIsDead => Game.Instance.Player is Tombstone;

        protected bool collisionDetection(Vector2Int pos, params Agent[] exclude) => Game.Instance.Agents.Exists(agent => agent.position == pos && agent is ICollision && !exclude.ToList().Contains(agent));
        protected bool AnyInRange<U>() where U : Agent => Game.Instance.Agents.Exists(x => x is U && IsInRange(x));
        protected bool IsInRange<U>(U obj) where U : Agent
        {
            var path = Astar.GetPath(obj.position, self.position);
            return path.Count > 0 && path.Count < self.detectionTreshold;
        }
        protected bool IsNextToMe<U>(U obj) where U : Agent => Vector2Int.Distance(self.position, obj.position) < 1.1;
    }
}