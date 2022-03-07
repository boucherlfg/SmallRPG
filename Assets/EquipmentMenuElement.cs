using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentMenuElement : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;
    public string Item
    {
        get => item.name;
        set
        {
            item = ItemsCodex.Instance[value];
            label.text = item.visibleName;
        }
    }
    private Item item;

    void Start()
    {
        label = GetComponent<TMP_Text>();
        StartCoroutine(HookWhenReady());
        IEnumerator HookWhenReady()
        {
            yield return null;
        }
    }
    public void Unequip()
    {
        item.Unequip();
    }
}
