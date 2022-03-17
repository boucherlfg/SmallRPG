using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Loot : Agent, IDrawable, ICollision, IActivatable, IEndable
{
    public List<string> loot;
    const string loot_tag = "loot";
    private bool locked;
    private bool broke;
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
            var tool = DataModel.Equipment.Tool;
            if (tool == null || !(tool is Tool) || !UseType.Loot.HasFlag((tool as Tool).useType))
            {
                UIManager.Notifications.CreateNotification("You would need a key.");
                return;
            }

            var recharge = DataModel.Inventory.HowMany(DataModel.Equipment.Tool.name) > 0 ? DataModel.Equipment.Tool : null;
            DataModel.Equipment.ConsumeTool();
            DataModel.Equipment.Tool = recharge;

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
        AudioManager.PlayAsSound(broke ? "empty" : "loot");
    }
}