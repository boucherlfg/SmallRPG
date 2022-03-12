using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuElement : MonoBehaviour
{
    public Item Item
    {
        get => item;
        set
        {
            item = value;
            image.sprite = item.sprite;
            label.text = $"{DataModel.Inventory.HowMany(value.name)} x {value.visibleName}";
        }
    }
    private Item item;
    private InventoryPanel inventoryPanel;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text label;
    void Start()
    {
        inventoryPanel = transform.root.GetComponent<InventoryPanel>();
    }
    public void Drop()
    {
        DataModel.Inventory.Delete(item.name);
        var floorItem = new FloorItem(item) { position = Game.Instance.Player.position };
        Game.Instance.Create(floorItem);
    }
    public void Use()
    {
        item.Equip();
    }
}
