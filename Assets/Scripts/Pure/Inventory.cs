using System.Collections.Generic;
using System.Linq;

public struct ItemState
{
    public string name;
    public int durability;
    public ItemState(string name, int durability = 0)
    {
        this.name = name;
        this.durability = durability;
    }

    public static implicit operator ItemState(Item i)
    {
        return new ItemState(i.name, i.durability);
    }
}
public class Inventory
{
    public List<ItemState> Items => items;
    private readonly List<ItemState> items;
    public delegate void OnChange();
    public event OnChange Changed;

    public void Init()
    {
        items.Clear();
        StartingGear.Inventory.ForEach(item => items.Add(new ItemState(item.name, item.durability)));
    }
    public Inventory()
    {
        items = new List<ItemState>();
    }

    public void Add(string name, int durability)
    {
        items.Add(new ItemState(name, durability));
        Changed?.Invoke();
    }
    public void Add(string name)
    {
        var obj = Codex.Items[name];
        Add(name, obj.durability);
    }
    public void Delete(string item)
    {
        items.Remove(items.Find(x => x.name == item));
        Changed?.Invoke();
    }
    public void Damage(string name)
    {
        try
        {
            var item = items.Find(x => x.name == name);
            var info = Codex.Items[item.name];
            items.Remove(item);
            item.durability--;
            if (item.durability <= 0)
            {
                UIManager.Notifications.CreateNotification($"your {info.visibleName} just broke");
                return;
            }
            items.Add(item);
            Changed?.Invoke();
        }
        catch
        {
        }
        
    }
    public int HowMany(string item)
    {
        return items.Count(x => x.name == item);
    }
    public void ForEachDistinct(TypedAction<ItemState> action)
    {
        foreach (var item in items.Distinct().OrderBy(x => x.name))
        {
            action(item);
        }
    }
}