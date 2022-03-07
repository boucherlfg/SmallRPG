using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Loot : Agent, IDrawable, ICollision, IUsableAgent
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
    public void Use(Player user)
    {
        if (locked)
        {
            NotifManager.CreateNotification("this loot is locked");
            var tool = PlayerData.Equipment.Tool;
            if (tool == null || !(tool is Tool) || !(UseType.Attack | UseType.Loot).HasFlag((tool as Tool).useType))
            {
                NotifManager.CreateNotification("You would need a key.");
                return;
            }

            //at this point, tool is not null, and it is a tool. either an attack or loot type.

            if((tool as Tool).useType == UseType.Attack) 
            {
                NotifManager.CreateNotification("You try to gently force it...");
                float precision = PlayerData.Precision + PlayerData.Equipment.TotalBonus.precision;
                var success = precision != 0 && Random.value < (precision - 1) / precision;
                if (!success)
                {
                    NotifManager.CreateNotification("but it breaks, and all loot with it...");
                    Game.Instance.Destroy(this);
                    return;
                }
            }
            var recharge = PlayerData.Inventory.HowMany(PlayerData.Equipment.Tool.name) > 0 ? PlayerData.Equipment.Tool : null;
            PlayerData.Equipment.ConsumeTool();
            PlayerData.Equipment.Tool = recharge;
            
            NotifManager.CreateNotification("and you manage to open it.");
        }


        if (loot.Count <= 0)
        {
            NotifManager.CreateNotification("that loot was empty...");
        }
        while (loot.Count > 0)
        {
            int count = loot.Count(x => x == loot[0]);
            Item item = ItemsCodex.Instance[loot[0]];
            NotifManager.CreateNotification($"you found {count} {item.visibleName}!");
            for (int i = 0; i < count; i++)
            {
                PlayerData.Inventory.Add(loot[0]);
            }
            loot.RemoveAll(x => x == loot[0]);
        }

        Game.Instance.Destroy(this);
    }
}