using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrapingMenuElementScript : MonoBehaviour 
{
    private Item item;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text label;
    public Item Item
    {
        get => item;
        set
        {
            item = value;
            int count = DataModel.Inventory.HowMany(item.name);
            label.text = $"{count}";
            GetComponent<DoWhenOver>().tooltipText = item.visibleName;
            image.sprite = item.sprite;
        }
    }
    public void Scrap()
    {
        DataModel.Inventory.Delete(item.name);
        UIManager.Notifications.CreateNotification("you destroyed the item...");
        var recipes = Codex.Recipes.FindAll(x => x.output.Exists(y => y.name == item.name));
        if (recipes.Count == 0)
        {
            UIManager.Notifications.CreateNotification("and got nothing.");
            UIManager.Scraping.Refresh();
            return;
        }

        var chosenRecipe = GameHelper.LinearRandom(recipes);
        float inputCount = chosenRecipe.input.Count;
        float outputCount = chosenRecipe.output.Count;

        var chosenItem = GameHelper.LinearRandom(chosenRecipe.input);

        if (Random.value > inputCount/outputCount)
        {
            UIManager.Notifications.CreateNotification("and got nothing.");
            UIManager.Scraping.Refresh();
            return;
        }

        UIManager.Notifications.CreateNotification($"and got a {chosenItem.visibleName}.");
        DataModel.Inventory.Add(chosenItem.name);
        UIManager.Scraping.Refresh();
    }
}
