using System.Collections.Generic;
using System.Linq;

public class Inventory
{
    public List<string> Items => items;
    private readonly List<string> items;
    public delegate void OnChange();
    public event OnChange Changed;

    public void Init()
    {
        items.Clear();
        StartingGear.Inventory.ForEach(item => items.Add(item.name));
    }
    public Inventory()
    {
        items = new List<string>();
    }

    public void Add(string item)
    {
        items.Add(item);
        Changed?.Invoke();
    }
    public void Delete(string item)
    {
        items.Remove(item);
        Changed?.Invoke();
    }
    public int HowMany(string item)
    {
        return items.Count(x => x == item);
    }
    public void ForEachDistinct(TypedAction<string> action)
    {
        foreach (var item in items.Distinct().OrderBy(x => x))
        {
            action(item);
        }
    }

}