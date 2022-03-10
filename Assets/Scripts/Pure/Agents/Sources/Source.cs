using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Source : Agent, IUsableAgent, IDrawable, ICollision
{
    public enum SourceType
    {
        Crafting = -1,
        Mining = 0,
        Woodcutting = 1,
        Foraging = 2,
        Hunting = 3,
    }
    public int life;
    public SourceType sourceType;
    public List<Item> possibleItems;
    public bool harvested;

    public Source(SourceType type)
    {
        this.sourceType = type;
        var items = Codex.Items.FindAll(item => item is Resource);
        items = items.FindAll(item => type == (item as Resource).type);
        possibleItems = items.OrderBy(x => x.value).ToList();
        life = GameHelper.DistributedRandom(3, 10);
    }

    public abstract UseType UseType { get; }
    public abstract Tile CurrentTile { get; }
    public abstract string Name { get; }
    public virtual void Use(Player player)
    {
        var tool = DataModel.Equipment.Tool;
        if (tool == null || !(tool is Tool) || (tool as Tool).useType != UseType)
        {
            UIManager.Notifications.CreateNotification("you need the right tools to harvest this.");
            return;
        }

        UIManager.Notifications.CreateNotification($"you start harvesting {Name}");
        life--;
        if (possibleItems.Count > 0 && Random.value < 0.5)
        {
            harvested = true;
            var item = GameHelper.DistributedRandom(possibleItems.OrderBy(x => x.value));
            DataModel.Inventory.Add(item.name);
            UIManager.Notifications.CreateNotification("and you find " + item.visibleName);
        }

        if (life > 0)
        {
            AudioManager.PlayAsSound(Name, 0.2f);
            return;
        }

        UIManager.Notifications.CreateNotification($"You have completely harvested {Name}");
        AudioManager.PlayAsSound("empty");
        if (!harvested)
        {
            UIManager.Notifications.CreateNotification("you turned out empty handed.");
        }
        Game.Instance.Destroy(this);
    }
}