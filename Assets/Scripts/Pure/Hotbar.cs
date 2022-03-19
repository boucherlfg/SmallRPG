using System;
using System.Collections.Generic;

public class Hotbar
{
    public event VoidAction Changed;
    private List<(int position, string item)> hotbarData;

    public void Use(int position)
    {
        var name = this[position];
        if (name == null) return;

        var item = Codex.Items[name];
        if (!item) return;

        item.Use();
    }
    public void Reset()
    {

        hotbarData.Clear();
        Changed?.Invoke();
        
    }
    public Hotbar()
    {
        hotbarData = new List<(int, string)>();
    }

    public void Inventory_Changed()
    {
        hotbarData.RemoveAll(x => DataModel.Inventory.HowMany(x.item) <= 0);
    }

    public string this[int position]
    {
        get
        {
            if (position < 1 || position > 5) throw new IndexOutOfRangeException();

            if (!hotbarData.Exists(x => x.position == position)) return null;
            return hotbarData.Find(x => x.position == position).item;
        }
        set
        {
            if (position < 1 || position > 5) throw new IndexOutOfRangeException();

            hotbarData.RemoveAll(x => x.position == position);
            hotbarData.Add((position, value));
            Changed?.Invoke();
        }
    }
}