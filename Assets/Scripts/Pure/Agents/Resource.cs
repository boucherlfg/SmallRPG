using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Resource : Agent, IUsableAgent, IDrawable, ICollision
{
    public int life;
    public ResourceType type;
    public List<Item> possibleItems;
    public bool harvested;

    public Resource(ResourceType type)
    {
        this.type = type;
        var items = ItemsCodex.Instance.Items.FindAll(item => item is ResourceItem);
        items = items.FindAll(item => type.HasFlag((item as ResourceItem).type));
        possibleItems = items.OrderBy(x => x.value).ToList();
        life = GameHelper.DistributedRandom(3, 10);
    }

    public abstract Tile CurrentTile { get; }
    public abstract string Name { get; }
    public void Use(Player player)
    {
        NotifManager.CreateNotification($"you start harvesting the {Name}");
        life--;
        if (possibleItems.Count > 0 && Random.value < 0.5)
        {
            harvested = true;
            var item = GameHelper.DistributedRandom(possibleItems);
            PlayerData.Inventory.Add(item.name);
            NotifManager.CreateNotification("and you find " + item.visibleName);
        }
        if (life > 0) return;

        NotifManager.CreateNotification($"the {Name} is now depleted.");

        if (!harvested)
        {
            NotifManager.CreateNotification("you turned out empty handed.");
        }
        Game.Instance.Destroy(this);
    }
}