using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeMenuItemScript : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text count;

    public System.Action tradeAction;
    private Item item;
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

    private float countFloat;
    public float Count
    {
        get => countFloat;
        set
        {
            countFloat = value;
            count.text = value + "";
        }
    }
    public Color Color
    {
        get => image.color;
        set => image.color = value;
    }
    public void TradeAction()
    {
        tradeAction?.Invoke();
        AudioManager.PlayAsSound("equip");
    }
}
