using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HotbarMenuElementScript : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static RectTransform selected;
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
        get => item == null ? null : item.name;
        set
        {
            if (value != null)
            {
                item = Codex.Items[value];
                image.sprite = item.sprite;
                image.color = new Color(1, 1, 1, 1);
                label.text = DataModel.Inventory.HowMany(item.name) + "";
                GetComponent<DoWhenOver>().tooltipText = item.visibleName;

            }
            else
            {
                item = null;
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0);
                label.text = "";
                GetComponent<DoWhenOver>().tooltipText = null;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop");
        var item = selected.GetComponent<StringValue>().value;
        DataModel.Hotbar[Position] = item;
    }
    public void Use()
    {
        DataModel.Hotbar.Use(Position);
        UIManager.Hotbar.Refresh();
    }

    public void OnDrag(PointerEventData eventData)
    {
        selected.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(selected.gameObject);
        selected = null;
        Debug.Log("end drag");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (DataModel.Hotbar[Position] == null) return;
        var obj = new GameObject();

        var v = obj.AddComponent<StringValue>();
        v.value = item.name;

        obj.AddComponent<CanvasRenderer>();

        var img = obj.AddComponent<Image>();
        img.sprite = item.sprite;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);

        var cvs = obj.AddComponent<CanvasGroup>();
        cvs.interactable = false;
        cvs.blocksRaycasts = false;

        obj.transform.SetParent(transform.root);
        obj.transform.position = transform.position;

        selected = obj.GetComponent<RectTransform>();
        selected.sizeDelta = Vector2.one * 50;
        DataModel.Hotbar[Position] = null;
        Debug.Log("begin drag");
    }
}