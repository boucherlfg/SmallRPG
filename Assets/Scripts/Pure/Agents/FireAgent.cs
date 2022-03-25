using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Tilemaps;

public class FireAgent : Agent, IUpdatable, IActivatable, IDrawable, ILightSource
{
    private List<Vector2Int> burntTiles;
    public Tile CurrentTile => DisplayManager.Instance["fire"];

    private FireAgent parent;
    FireAgent Root
    {
        get
        {
            var current = this;
            while (current.parent != null)
            {
                current = parent;
            }
            return current;
        }
    }
    public FireAgent() 
    {
        burntTiles = new List<Vector2Int>();
    }
    public FireAgent(FireAgent parent) : this()
    {
        this.parent = parent;
    }
    public float Range => 2;
    private bool reproduce = true;
    public void Activate(IMovable source)
    {
        if (source is IStats)
        {
            var buff = new Buff(() => source as Agent, new StatBlock() { life = -1 });
            DataModel.ActiveBuffs.Add(buff);
            if (source is Mob)
            {
                UIManager.Notifications.CreateNotification($"fire hit a {(source as Mob).data.visibleName}!");
            }
            else if (source is Player)
            {
                UIManager.Notifications.CreateNotification($"you were hit by fire!");
            }
            return;
        }

    }

    public void Update()
    {
        if (reproduce)
        {
            CreateOtherFire(Vector2Int.up);
            CreateOtherFire(Vector2Int.down);
            CreateOtherFire(Vector2Int.left);
            CreateOtherFire(Vector2Int.right);
        }
        Game.Instance.Destroy(this);
    }
    private void CreateOtherFire(Vector2Int direction)
    {
        var pos = position + direction;
        var all = Game.Instance.Agents.FindAll(x => x.position == pos);

        //if this is out of the game limit, pass
        if (all.Count == 0 && !Game.Instance.Level.Ground.Contains(pos)) return;

        if (Root.burntTiles.Contains(pos)) return;
        Root.burntTiles.Add(pos);
        var fire = new FireAgent(Root)
        {
            position = pos,
        };

        
        if (all.Exists(x => x is ICollision))
        {
            fire.reproduce = false;
        }
        
        Game.Instance.Create(fire);
    }
}