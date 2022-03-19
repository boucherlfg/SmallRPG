using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HotbarMenuElementScript : MonoBehaviour, IDropHandler
{
    public int Position
    {
        get
        {
            int i = 0;
            for (i = 0; i < 5; i++) if (transform.parent.GetChild(i) == transform) break;
            return i >= 5 ? -1 : i + 1;
        }
    }
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text label;

    private Item item;

    public string ItemName
    {
        get => item.name;
        set
        {
            if (value != null)
            {
                item = Codex.Items[value];
                image.sprite = item.sprite;
                image.color = new Color(1, 1, 1, 1);
                label.text = DataModel.Inventory.HowMany(item.name) + "";
            }
            else
            {
                item = null;
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0);
                label.text = "";
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop");
        var item = ItemMenuElement.selected.GetComponent<StringValue>().value;
        DataModel.Hotbar[Position] = item;
    }
    public void Use()
    {
        DataModel.Hotbar.Use(Position);
        UIManager.Hotbar.Refresh();
    }
}