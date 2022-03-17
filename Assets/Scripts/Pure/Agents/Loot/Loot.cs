using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Loot : Agent, IDrawable, ICollision, IActivatable, IEndable
{
    public List<string> loot;
    const string loot_tag = "loot";
    private bool locked;
    public Loot()
    {
        loot = new List<string>();
        locked = Random.value < 0.3f;
    }
    public Loot(bool locked) : base()
    {
        this.locked = locked;
    }

    public Tile CurrentTile => DisplayManager.Instance[loot_tag];

    public UseType UseType => UseType.Loot;
    public void Activate(IMovable user)
    {
        if (locked)
        {
            UIManager.Notifications.CreateNotification("this loot is locked");
            var tool = Codex.Items.Find(x => x is Tool && (x as Tool).useType == UseType);
            var hasTool = DataModel.Inventory.Items.Exists(x => x == tool.name);

            if (!hasTool)
            {
                UIManager.Notifications.CreateNotification("You would need a key.");
                return;
            }

            DataModel.Inventory.Delete(tool.name);

            UIManager.Notifications.CreateNotification("and you manage to open it.");
        }


        if (loot.Count <= 0)
        {
            UIManager.Notifications.CreateNotification("that loot was empty...");
        }
        while (loot.Count > 0)
        {
            int count = loot.Count(x => x == loot[0]);
            Item item = Codex.Items[loot[0]];
            UIManager.Notifications.CreateNotification($"you found {count} {item.visibleName}!");
            for (int i = 0; i < count; i++)
            {
                DataModel.Inventory.Add(loot[0]);
            }
            loot.RemoveAll(x => x == loot[0]);
        }

        Game.Instance.Destroy(this);
    }

    public void End()
    {
        AudioManager.PlayAsSound("loot");
    }
}