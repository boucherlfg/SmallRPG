using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : PanelWrapper
{
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
            PlayerData.Inventory.onChanged += Refresh;
            InputManager.Inventory += ToggleInventory;
        }
    }

    public void ToggleInventory()
    {
        Active = !Active;
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
}
