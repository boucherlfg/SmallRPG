using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentMenuElement : MonoBehaviour
{
    [SerializeField]
    private Image image;
    public event VoidAction isEnter;
    public event VoidAction isExit;

    public string Item
    {
        get => item.name;
        set
        {
            item = Codex.Items[value];
            image.sprite = item.sprite;
            GetComponent<DoWhenOver>().tooltipText = item.visibleName;
        }
    }
    private Item item;

    void Start()
    {
        
    }
    public void Unequip()
    {
        (item as Equipable).Unequip();
        UIManager.Equipment.Refresh();
    }
}
