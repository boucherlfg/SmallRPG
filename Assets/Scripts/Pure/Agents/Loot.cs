using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

public class Loot : Agent, IDrawable, ICollision, IUsableAgent
{
    public List<string> loot;
    const string loot_tag = "loot";

    public Loot()
    {
        loot = new List<string>();
    }

    public Tile CurrentTile => DisplayManager.Instance[loot_tag];

    public void Use(Player user)
    {
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