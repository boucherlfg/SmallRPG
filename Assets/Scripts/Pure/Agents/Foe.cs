using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Foe : Agent, IStats, IMovable, IDrawable, IUpdatable, ICollision, IUsableAgent, IEndable
{
    private static bool collisionDetection(Vector2Int pos, params Agent[] exclude) => Game.Instance.Agents.Exists(agent => agent.position == pos && agent is ICollision && !exclude.ToList().Contains(agent));
    const string foe_tag = "foe";
    public const int detectionTreshold = 10;
    private State state;
    private Vector2Int savedPosition;
    private List<string> loot;
    protected float value = 3;
    public float Value => value;

    public Foe()
    {
        state = new RandomWalk(this);
        Orientation = Vector2Int.down;

        life = 3;
        attack = 1;
        precision = 1;
        evasion = 1;

        loot = GameHelper.PopulateLoot(value);
    }

    

    public bool Immobilized { get; set; }
    public Vector2Int Orientation { get; set; }
    public Tile CurrentTile => DisplayManager.Instance[foe_tag];

    #region stats
    private int life;
    private int mana;
    private int attack;
    private int defense;
    private int precision;
    private int evasion;

    public int Life { get => life; set => life = value; }
    public int Mana { get => mana; set => mana = value; }
    public int Attack { get => attack; set => attack = value; }
    public int Defense { get => defense; set => defense = value; }
    public int Precision { get => precision; set => precision = value; }
    public int Evasion { get => evasion; set => evasion = value; }
    #endregion

    public void Update()
    {
        Immobilized = false;
        state = state.Update();
    }

    public void End()
    {
        Game.Instance.Create(new Loot { position = position, loot = loot });
        NotifManager.CreateNotification("your foe crumbles in agony, leaving sweet loot behind.");
    }

    public UseType UseType => UseType.Attack;
    public void Use(Player user)
    {
        var tool = PlayerData.Equipment.Tool;
        if (tool == null || !(tool is Tool) || (tool as Tool).useType != UseType)
        {
            NotifManager.CreateNotification("Get a weapon before you attack!");
            return;
        }

        NotifManager.CreateNotification("you attempt to attack an enemy...");
        if (!GameHelper.CalculateHit(user, this))
        {
            NotifManager.CreateNotification("but your attack misses.");
            return;
        }
        var damage = GameHelper.CalculateDamage(user, this);
        NotifManager.CreateNotification($"and you manage to deal {damage} damage.");
        Life -= damage;
        if (Life <= 0)
        {
            Game.Instance.Destroy(this);
        }
    }

    abstract class State
    {
        public Foe self;
        public State(Foe self) => this.self = self;
        public abstract State Update();

        protected bool PlayerIsDead => Game.Instance.Player is Tombstone;
        protected bool PlayerIsInRange => Game.Instance.Player != null && !PlayerIsDead && Vector2Int.Distance(Game.Instance.Player.position, self.position) < detectionTreshold;
        protected bool PlayerIsOnNextTile => Game.Instance.Player != null && !PlayerIsDead && Vector2Int.Distance(Game.Instance.Player.position, self.position) <= 1.1;
    }
    class ReturnToPosition : State
    {
        private List<Vector2Int> path;
        public ReturnToPosition(Foe self) : base(self)
        {
            path = new List<Vector2Int>();
        }

        public override State Update()
        {
            if (PlayerIsInRange)
            {
                NotifManager.CreateNotification("an enemy has seen you!");
                return new FollowPlayer(self);
}

            if (path.Count <= 0)
            {
                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos);
                path = Astar.GetPath(self.position, self.savedPosition);
                Astar.CollisionDetection = astarColDetection;
                if (path.Count <= 0) return new RandomWalk(self);
            }

            var step = path[0];
            self.Orientation = step - self.position;
            self.position = path[0];
            path.RemoveAt(0);
            return this;
        }
    }
    class FollowPlayer : State
    {
        Vector2Int lastSeenAt;
        List<Vector2Int> path;

        public FollowPlayer(Foe self) : base(self)
        {
            self.savedPosition = self.position;
            path = new List<Vector2Int>();
            lastSeenAt = Game.Instance.Player.position;
        }
        public override State Update()
        {
            if (PlayerIsInRange && path.Count == 0) //path is done and player is not in range anymore
            {
                lastSeenAt = Game.Instance.Player.position;

                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos, Game.Instance.Player);
                path = Astar.GetPath(self.position, lastSeenAt);
                Astar.CollisionDetection = astarColDetection;
                return this;
            }
            else if (path.Count == 0) //path is done but player is still in range
            {
                NotifManager.CreateNotification("an enemy has lost track of you!");
                return new ReturnToPosition(self);
            }
            else //path is not done
            {
                var step = path[0];
                
                self.Orientation = step - self.position;
                self.position = path[0];
                path.RemoveAt(0);

                if (PlayerIsOnNextTile)
                {
                    return new AttackPlayer(self);
                }
                else
                {
                    return this;
                }
            }
        }
    }
    class RandomWalk : State
    {
        Vector2Int u;
        List<Vector2Int> path;
        
        public RandomWalk(Foe self) : base(self) 
        {
            path = new List<Vector2Int>();
        }
        public override State Update()
        {
            if (PlayerIsInRange)
            {
                NotifManager.CreateNotification("an enemy has seen you!");
                return new FollowPlayer(self);
            }

            if (path.Count <= 0)
            {
                u = Vector2Int.RoundToInt(10 * Random.insideUnitCircle);
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
    class AttackPlayer : State
    {
        public AttackPlayer(Foe self) : base(self)
        {
        }
        public override State Update()
        {
            if (PlayerIsOnNextTile)
            {
                NotifManager.CreateNotification("enemy tries to attack you... ");
                var player = Game.Instance.Player;

                if (!GameHelper.CalculateHit(self, player))
                {
                    NotifManager.CreateNotification("but misses.");
                    return this;
                }

                var damage = GameHelper.CalculateDamage(self, player);

                player.Life -= damage;
                if (player.Life <= 0)
                {
                    Game.Instance.Destroy(player);
                    return this;
                }
                NotifManager.CreateNotification($"and deals {damage} damage to you.");
                return this;
            }
            else
            {
                return new FollowPlayer(self);
            }
        }
    }
}