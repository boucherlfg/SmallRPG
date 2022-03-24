﻿using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Player : Agent, IStats, IMovable, IDrawable, IUpdatable, ICollision, IEndable, ILightSource
{
    const string player_south = nameof(player_south);
    const string player_north = nameof(player_north);
    const string player_east = nameof(player_east);
    const string player_west = nameof(player_west);
    public bool Immobilized { get; set; }
    public Vector2Int Orientation { get; set; }

    public float Range => 20;
    public virtual Tile CurrentTile
    {
        get
        {
            if (Orientation == Vector2Int.up) return DisplayManager.Instance[player_north];
            else if (Orientation == Vector2Int.left) return DisplayManager.Instance[player_west];
            else if (Orientation == Vector2Int.right) return DisplayManager.Instance[player_east];
            else return DisplayManager.Instance[player_south];
        }
    }

    #region stats
    public StatBlock Stats 
    { 
        get => DataModel.StatBlock;
        set
        {
            DataModel.StatBlock = value;
            if (DataModel.StatBlock.life <= 0) Game.Instance.Destroy(this);
        }
    }

    #endregion

    public State state;
    public Player()
    {
        Orientation = Vector2Int.down;
        state = new WaitState(this);
    }


    public virtual void Update()
    {
        Immobilized = false;

        if (state != null)
        {
            state = state.Update();
        }
    }

    public virtual void End()
    {
        if (DataModel.StatBlock.life <= 0)
        {
            Game.Instance.Create(new Tombstone { position = position });
            UIManager.Notifications.CreateNotification("oh dear! you are dead!");
            UIManager.GameOver.Active = true;
            AudioManager.PlayAsMusic("end_song");
        }
    }

    public abstract class State
    {
        protected Player self;
        public State(Player self)
        {
            this.self = self;
        }
        public abstract State Update();
    }
    public class ExitState : State
    {
        public ExitState(Player self) : base(self) { }

        public override State Update()
        {
            return null;
        }
    }
    public class MoveState : State
    {
        private Vector2Int orientation;
        public MoveState(Player self, Vector2Int orientation) : base(self) 
        {
            this.orientation = orientation;
        }

        public override State Update()
        {
            self.Orientation = orientation;
            self.position += self.Orientation;
            var room = Game.Instance.Level.Rooms.Find(x => x.Ground.Contains(self.position));
            if (room != null)
            {
                room.discovered = true;
                MinimapScript.Instance.UpdateMinimap();
            }
            return null;
        }
    }
    public class AttackState : State
    {
        public AttackState(Player self) : base(self) { }
        public override State Update()
        {
            var target = Game.Instance.Agents.FindLast(agent => agent is IUsableAgent && agent.position == self.position + self.Orientation);
            if (target != null)
            {
                (target as IUsableAgent).Use(self);
            }
            return null;
        }
    }
    public class UseState : State
    {
        private int position;
        public UseState(Player self, int position) : base(self)
        {
            this.position = position;
        }

        public override State Update()
{
            UIManager.Hotbar.hotbarMenuElements[position - 1].Use();
            return null;
        }
    }
    public class WaitState : State
    {
        public WaitState(Player self) : base(self) { }

        public override State Update()
        {
            return null;
        }
    }
}
