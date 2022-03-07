using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Source : Agent, IUsableAgent, IDrawable, ICollision
{
    public int life;
    public ResourceType type;
    public List<Item> possibleItems;
    public bool harvested;

    public Source(ResourceType type)
    {
        this.type = type;
        var items = ItemsCodex.Instance.Items.FindAll(item => item is Resource);
        items = items.FindAll(item => type.HasFlag((item as Resource).type));
        possibleItems = items.OrderBy(x => x.value).ToList();
        life = GameHelper.DistributedRandom(3, 10);
    }

    public abstract UseType UseType { get; }
    public abstract Tile CurrentTile { get; }
    public abstract string Name { get; }
    public virtual void Use(Player player)
    {
        var tool = PlayerData.Equipment.Tool;
        if (tool == null || !(tool is Tool) || (tool as Tool).useType != UseType)
        {
            NotifManager.CreateNotification("you need the right tools to harvest this.");
            return;
        }
        
        NotifManager.CreateNotification($"you start harvesting {Name}");
        life--;
        if (possibleItems.Count > 0 && Random.value < 0.5)
        {
            harvested = true;
            var item = GameHelper.DistributedRandom(possibleItems);
            PlayerData.Inventory.Add(item.name);
            NotifManager.CreateNotification("and you find " + item.visibleName);
        }
        if (life > 0) return;

        NotifManager.CreateNotification($"You have completely harvested {Name}");

        if (!harvested)
        {
            NotifManager.CreateNotification("you turned out empty handed.");
        }
        Game.Instance.Destroy(this);
    }
}