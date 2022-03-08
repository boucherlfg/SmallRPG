using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentMenuElement : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;
    [SerializeField]
    private Image image;
    public string Item
    {
        get => item.name;
        set
        {
            item = Codex.Items[value];
            label.text = item.visibleName;
            image.sprite = item.sprite;
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
