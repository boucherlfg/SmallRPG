using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ItemsCodex : MonoSingleton<ItemsCodex>
{
    [SerializeField]
    private List<Item> items;
    public static ItemsCodex Instance => _instance;
    public List<Item> Items => _instance.items;
    public Item this[string name]
    {
        get => items.FirstOrDefault(x => x.name == name);
    }
}