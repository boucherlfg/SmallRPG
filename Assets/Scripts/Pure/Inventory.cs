using System.Collections.Generic;
using System.Linq;

public struct ItemState
{
    public string name;
    public int durability;
    public int maxDurability;
    public ItemState(string name, int durability, int maxDurability)
    {
        this.name = name;
        this.durability = durability;
        this.maxDurability = durability;
    }

    public static implicit operator ItemState(Item i)
    {
        return new ItemState(i.name, i.durability, i.durability);
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
        StartingGear.Inventory.ForEach(item => items.Add(item));
    }
    public Inventory()
    {
        items = new List<ItemState>();
    }

    public void Add(string name, int durability)
    {
        ItemState toAdd = Codex.Items[name];
        toAdd.durability = durability;
        items.Add(toAdd);
        Changed?.Invoke();
    }
    public void Add(string name)
    {
        var obj = Codex.Items[name];
        items.Add(obj);
    }
    public void Delete(string item)
    {
        if (DataModel.Equipment.HasItem(item, out EquipType type))
        {
            DataModel.Equipment.Unequip(type);
        }
        items.Remove(items.FindAll(x => x.name == item).Minimum(x => x.durability));
        Changed?.Invoke();
    }
    public void Damage(string name)
    {
        try
        {
            var item = items.FindAll(x => x.name == name).Minimum(x => x.durability);
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