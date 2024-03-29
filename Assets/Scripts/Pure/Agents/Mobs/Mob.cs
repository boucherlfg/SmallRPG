﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public abstract class Mob : Agent, IStats, IMovable, IDrawable, IUpdatable, ICollision, IActivatable, IEndable
{
    
    protected List<string> loot;
    public AgentData data;
    public State state;
    public int detectionTreshold = 8;
    public abstract AgentData.AgentType AgentType { get; }
    public Mob()
    {
        Orientation = Vector2Int.down;

        var datas = Codex.Agents.FindAll(x => x.agentType == AgentType);
        data = GameHelper.DistributedRandom(datas.OrderBy(x => x.value));
        loot = GameHelper.PopulateLoot(data);

        Stats = data.statblock;
        state = InitialState;
    }

    #region [stats]
    private StatBlock stats;
    public StatBlock Stats
    {
        get => stats;
        set
        {
            stats = value;
            if (stats.life <= 0) Game.Instance.Destroy(this);
        }
    }
    #endregion

    public abstract float Value { get; }
    protected abstract State InitialState { get; }
    public virtual Tile CurrentTile => data.tile;
    public Vector2Int Orientation { get; set; }
    public bool Immobilized { get; set; }

    public virtual void End()
    {
        loot.ForEach(item =>
        {
            Game.Instance.Create(new FloorItem(Codex.Items[item]) { position = position });
        });
    }
    public virtual void Update()
    {
        Immobilized = false;
        var newState = state.Update();
        state = newState;
    }
    public virtual void Activate(IMovable user)
    {
        AudioManager.PlayAsSound("use");
        UIManager.Notifications.CreateNotification($"you attempt to attack a {data.visibleName}...");

        DataModel.Equipment.Damage(EquipType.Weapon);

        if (!GameHelper.CalculateHit(user as IStats, this))
        {
            UIManager.Notifications.CreateNotification("but your attack misses.");
            return;
        }
        var damage = GameHelper.CalculateDamage(user as IStats, this);
        UIManager.Notifications.CreateNotification($"and you manage to deal {damage} damage.");
        DisplayManager.Instance.CreateDamageText(damage, position);
        var s = Stats;
        s.life -= damage;
        Stats = s;
    }
    public abstract class State
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

        protected bool collisionDetection(Vector2Int pos, params Agent[] exclude)
        {
            return Game.Instance.Agents.Exists(agent => agent.position == pos && agent is ICollision && !exclude.ToList().Contains(agent));
        }
        protected bool AnyInRange<U>() where U : Agent => Game.Instance.Agents.Exists(x => x is U && IsInRange(x) && self != x);
        protected bool IsInRange<U>(U obj, params Agent[] exclude) where U : Agent
        {
            var hit = GameHelper.Raycast(self.position, obj.position, self.detectionTreshold, exclude);
            return hit == obj;
        }
        protected bool IsNextToMe<U>(U obj) where U : Agent => Vector2Int.Distance(self.position, obj.position) < 1.1;
    }
    public class IdleState : State
    {
        private State previous;
        public IdleState(Mob self, State previous) : base(self)
        {
            this.previous = previous;
        }

        public override State Update()
        {
            path.Clear();
            return self.InitialState;
        }
    }
}