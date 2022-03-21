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
    public override bool ExitableByEscape => true;
    public override bool ExcludeFromPause => false;
    void Start()
    {
        StartCoroutine(HookAfterNull());
        IEnumerator HookAfterNull()
        {
            yield return null;
            DataModel.Inventory.Changed += Refresh;
        }
        ActiveStateChanged += CraftingPanel_ActiveStateChanged;
    }

    private void CraftingPanel_ActiveStateChanged()
    {
        CraftingType = CraftingType.Hand;
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
