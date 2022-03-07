using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemMenuElement : MonoBehaviour
{
    public string Item
    {
        get => item.name;
        set
        {
            item = ItemsCodex.Instance[value];
            label.text = $"{PlayerData.Inventory.HowMany(value)} x {item.visibleName}";
        }
    }
    private Item item;
    private InventoryPanel inventoryPanel;
    [SerializeField]
    private TMP_Text label;
    void Start()
    {
        inventoryPanel = transform.root.GetComponent<InventoryPanel>();
    }
    public void Drop()
    {
        PlayerData.Inventory.Delete(item.name);
    }
    public void Use()
    {
        item.Equip();
    }
}
