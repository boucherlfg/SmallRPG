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

    private int currentFilter = 0;
    void Start()
    {
        StartCoroutine(HookWhenReady());
        IEnumerator HookWhenReady()
        {
            ActiveStateChanged += Refresh;
            yield return null;
            DataModel.Inventory.Changed += Refresh;
        }
    }

    public void Refresh(int value)
    {
        currentFilter = value;
        string type = dropdown.options[value].text;
        var types = GameHelper.GetAllSubTypes<Item>();
        foreach (Type t in types)
        {
            if (t.Name == type)
            {
                Refresh(type);
                return;
            }
        }
        Refresh();
    }
    public void Refresh()
    {
        if (currentFilter > 0)
        {
            Refresh(currentFilter);
            return;
        }

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        DataModel.Inventory.ForEachDistinct(item =>
        {
            var obj = Instantiate(itemMenuElementPrefab, container);
            var comp = obj.GetComponent<ItemMenuElement>();
            comp.Item = item;
        });
    }
    public void Refresh(string filter)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        DataModel.Inventory.ForEachDistinct(item =>
        {
            if (Codex.Items[item].GetType().Name == filter)
            {
                var obj = Instantiate(itemMenuElementPrefab, container);
                var comp = obj.GetComponent<ItemMenuElement>();
                comp.Item = item;
            }
        });
    }
}
