using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenuElementScript : MonoBehaviour
{
    private Dictionary<CraftingType, string> audios => new Dictionary<CraftingType, string>()
    {
        [CraftingType.Alchemy] = "potion",
        [CraftingType.Anvil] = "rock",
        [CraftingType.Hand] = "equip",
        [CraftingType.Bench] = "wood",
        [CraftingType.Smelting] = "longMetal"
    };
    [SerializeField]
    private Transform inputContainer;
    [SerializeField]
    private Transform outputContainer;
    [SerializeField]
    private TMP_Text number;

    private Recipe recipe;
    public Recipe Recipe
    {
        get => recipe;
        set
        {
            recipe = value;
            Refresh();
        }
    }
    void Refresh()
    {
        foreach (Transform t in inputContainer) Destroy(t.gameObject);
        foreach (Transform t in outputContainer) Destroy(t.gameObject);

        recipe.input.ForEach(x => CreateIcon(x, inputContainer));
        recipe.output.ForEach(x => CreateIcon(x, outputContainer));
        number.text = "" + GetHowMany();

    }
    int GetHowMany()
    {
        var dict = new Dictionary<string, int>();
        recipe.input.ForEach(x => dict[x.name] = DataModel.Inventory.HowMany(x.name)/recipe.input.Count(y => y.name == x.name));
        return dict.Minimum(x => x.Value).Value;
    }
    void CreateIcon(Item item, Transform container)
    {
        GameObject go = new GameObject();
        go.transform.SetParent(container, false);
        
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(40, 40);
        
        go.AddComponent<CanvasRenderer>();

        var tooltip = go.AddComponent<TooltipWhenOver>();
        tooltip.tooltipText = item.visibleName;
        
        Image img = go.AddComponent<Image>();
        img.sprite = item.sprite;

    }
    public void Craft()
    {
        recipe.Craft();
        var sound = audios[recipe.craftingType];
        AudioManager.PlayAsSound(sound);
        Refresh();
    }
}