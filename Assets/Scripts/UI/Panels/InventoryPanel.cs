using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryPanel : PanelWrapper
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private Transform container;
    [SerializeField]
    private GameObject itemMenuElementPrefab;
    public override bool ExitableByEscape => true;

    void Start()
    {
        InitDropdown();
        StartCoroutine(HookWhenReady());
        IEnumerator HookWhenReady()
        {
            ActiveStateChanged += RefreshIfActive;
            yield return null;
            DataModel.Inventory.Changed += RefreshIfActive;
        }
    }
    public void RefreshIfActive()
    {
        if (Active)
        {
            Refresh(dropdown.value);
        }
    }
    public void InitDropdown()
    {
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.options.Clear();
        var types = typeof(Item).Assembly.GetTypes();
        types = types.OrderBy(x => types.Count(y => y.IsAssignableFrom(x))).ToArray();

        foreach (var type in types)
        {
            if (Attribute.IsDefined(type, typeof(InventoryCategoryAttribute)))
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(type.Name));
            }
        }
        dropdown.onValueChanged.AddListener(Refresh);
    }
    public void Refresh()
    {
        Refresh(dropdown.value);
    }
    public void Refresh(int value)
    {
        foreach (Transform t in container) Destroy(t.gameObject);

        var categories = GetInventoryCategories();
        var type = categories.FirstOrDefault(x => x.Name == dropdown.options[value].text);

        var items = Codex.Items.FindAll(i => DataModel.Inventory.Items.Exists(u => u.name == i.name)).OrderBy(x => x.visibleName).ToList();
        while(items.Count() > 0)
        {
            var item = items.ElementAt(0);
            items = items.FindAll(x => x.name != item.name);
            if (item.GetType().IsSubclassOf(type) || item.GetType().Equals(type))
            {
                var go = Instantiate(itemMenuElementPrefab, container);
                var comp = go.GetComponent<ItemMenuElement>();
                comp.Item = item;
            }
        }
    }

    Type[] GetInventoryCategories()
    {
        return typeof(Item).Assembly.GetTypes().Where(x => Attribute.IsDefined(x, typeof(InventoryCategoryAttribute))).ToArray();
    }
}
