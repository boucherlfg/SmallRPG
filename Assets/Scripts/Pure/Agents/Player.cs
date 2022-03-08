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
    public int Life { get => DataModel.Life; set => DataModel.Life = value; }
    public int Mana { get => DataModel.Mana; set => DataModel.Mana = value; }
    public int Attack { get => DataModel.Attack; set => DataModel.Attack = value; }
    public int Defense { get => DataModel.Defense; set => DataModel.Defense = value; }
    public int Precision { get => DataModel.Precision; set => DataModel.Precision = value; }
    public int Evasion { get => DataModel.Evasion; set => DataModel.Evasion = value; }
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

    public virtual void End()
    {
        if (DataModel.Life <= 0)
        {
            Game.Instance.Create(new Tombstone { position = position });
            UIManager.Notifications.CreateNotification("oh dear! you are dead!");
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
            if (DataModel.Equipment.Tool != null)
            {
                DataModel.Equipment.Tool.Use();
            }
            else
            {
                var target = Game.Instance.Agents.FindLast(agent => agent is IUsableAgent && agent.position == self.position + self.Orientation);
                if (target != null)
                {
                    (target as IUsableAgent).Use(self);
                }
        
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
