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

    void Start()
    {
        StartCoroutine(HookWhenReady());
        IEnumerator HookWhenReady()
        {
            ActiveStateChanged += Refresh;
            yield return null;
            PlayerData.Inventory.Changed += Refresh;
        }
    }

    public void Refresh(int value)
    {
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
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        PlayerData.Inventory.ForEachDistinct(item =>
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

        PlayerData.Inventory.ForEachDistinct(item =>
        {
            if (ItemsCodex.Instance[item].GetType().Name == filter)
            {
                var obj = Instantiate(itemMenuElementPrefab, container);
                var comp = obj.GetComponent<ItemMenuElement>();
                comp.Item = item;
            }
        });
    }
}
