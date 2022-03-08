﻿using System.Collections.Generic;
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
            UIManager.Notifications.CreateNotification("this loot is locked");
            var tool = DataModel.Equipment.Tool;
            if (tool == null || !(tool is Tool) || !(UseType.Attack | UseType.Loot).HasFlag((tool as Tool).useType))
            {
                UIManager.Notifications.CreateNotification("You would need a key.");
                return;
            }

            //at this point, tool is not null, and it is a tool. either an attack or loot type.

            if((tool as Tool).useType == UseType.Attack) 
            {
                UIManager.Notifications.CreateNotification("You try to gently force it...");
                float precision = DataModel.Precision + DataModel.Equipment.TotalBonus.precision;
                var success = precision != 0 && Random.value < (precision - 1) / precision;
                if (!success)
                {
                    UIManager.Notifications.CreateNotification("but it breaks, and all loot with it...");
                    Game.Instance.Destroy(this);
                    return;
                }
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
}