using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CraftingType
{
    Smelting = 0,
    Bench = 1,
    Anvil = 2,
    Hand = 3,
}
[CreateAssetMenu(menuName = "Felix/Recipe")]
public class Recipe : ScriptableObject
{
    public CraftingType craftingType;
    public List<Item> input;
    public List<Item> output;
    public float successRate = 1;

    public bool CanCraft
    {
        get
        {
            return input.TrueForAll(ingredient =>
            {
                return DataModel.Inventory.HowMany(ingredient.name) >= input.Count(item => item.name == ingredient.name);
            });
        }
    }
    public void Craft()
    {
        if (!CanCraft)
        {
            UIManager.Notifications.CreateNotification("you cant craft that");
            return;
        }
        var items = new List<Item>(input);
        while(items.Count > 0)
        {
            var sub = items.FindAll(x => x.name == items[0].name);
            UIManager.Notifications.CreateNotification($"you used {sub.Count} {items[0].visibleName}.");
            sub.ForEach(item =>
            {
                items.Remove(item);
                DataModel.Inventory.Delete(item.name);
            });
        }

        if (Random.value > successRate)
        {
            UIManager.Notifications.CreateNotification("you botched your job. What a shame..");
        }
        items = new List<Item>(output);
        while (items.Count > 0)
        {
            var sub = items.FindAll(x => x.name == items[0].name);
            UIManager.Notifications.CreateNotification($"you crafted {sub.Count} {items[0].visibleName}.");
            sub.ForEach(item =>
            {
                items.Remove(item);
                DataModel.Inventory.Add(item.name);
            });
        }
    }
}
