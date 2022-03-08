using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CraftingPanel : PanelWrapper
{
    [SerializeField]
    private Transform container;
    [SerializeField]
    public GameObject craftingMenuElementPrefab;
    [SerializeField]
    private TMP_Text title;
    private CraftingType craftingType;

    void Start()
    {
        StartCoroutine(HookAfterNull());
        IEnumerator HookAfterNull()
        {
            yield return null;
            DataModel.Inventory.Changed += Refresh;
        }
    }
    public CraftingType CraftingType
    {
        get => craftingType;
        set 
        {
            craftingType = value;
            title.text = craftingType.ToString();
            Refresh();
        }
    }

    private void Refresh()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        var list = Codex.Recipes.FindAll(x => x.CanCraft && x.craftingType == craftingType);
        list.ForEach(recipe =>
        {
            var obj = Instantiate(craftingMenuElementPrefab, container);
            var comp = obj.GetComponent<CraftingMenuElementScript>();
            comp.Recipe = recipe;
        });
    }
}
