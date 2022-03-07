using System;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Player : Agent, IStats, IMovable, IDrawable, IUpdatable, ICollision, IEndable
{
    const string player_south = nameof(player_south);
    const string player_north = nameof(player_north);
    const string player_east = nameof(player_east);
    const string player_west = nameof(player_west);
    public bool Immobilized { get; set; }
    public Vector2Int Orientation { get; set; }

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
    public int Life { get => PlayerData.Life; set => PlayerData.Life = value; }
    public int Mana { get => PlayerData.Mana; set => PlayerData.Mana = value; }
    public int Attack { get => PlayerData.Attack; set => PlayerData.Attack = value; }
    public int Defense { get => PlayerData.Defense; set => PlayerData.Defense = value; }
    public int Precision { get => PlayerData.Precision; set => PlayerData.Precision = value; }
    public int Evasion { get => PlayerData.Evasion; set => PlayerData.Evasion = value; }
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
        state = state.Update();
    }

    public void End()
    {
        if (PlayerData.Life <= 0)
        {
            Game.Instance.Create(new Tombstone { position = position });
            NotifManager.CreateNotification("oh dear! you are dead!");
            UIManager.GameOver.Active = true;
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
            return this;
        }
    }
    public class MoveState : State
    {
        public MoveState(Player self) : base(self) { }

        public override State Update()
        {
            self.position += self.Orientation;
            return new WaitState(self);
        }
    }
    public class UseState : State 
    {
        public UseState(Player self) : base(self) { }
        public override State Update()
        {
            if (PlayerData.Equipment.Tool != null)
            {
                PlayerData.Equipment.Tool.Use();
            }
            return new WaitState(self);
        }
    }
    public class WaitState : State
    {
        public WaitState(Player self) : base(self) { }

        public override State Update()
        {
            return this;
        }
    }
}
