using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMenuElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Item Item
    {
        get => item;
        set
        {
            item = value;
            image.sprite = item.sprite;
            label.text = $"{DataModel.Inventory.HowMany(value.name)}";
            GetComponent<DoWhenOver>().tooltipText = item.visibleName;
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
        inventoryPanel.Refresh();
    }

    int position = 0;
    public void Use()
    {
        item.Prepare();
        AudioManager.PlayAsSound("equip");
        inventoryPanel.Refresh();
    }


    #region interface implementation

    public void OnDrag(PointerEventData eventData)
    {
        HotbarMenuElementScript.selected.position = eventData.position;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(HotbarMenuElementScript.selected.gameObject);
        HotbarMenuElementScript.selected = null;
        Debug.Log("end drag");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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

        HotbarMenuElementScript.selected = obj.GetComponent<RectTransform>();
        HotbarMenuElementScript.selected.sizeDelta = Vector2.one * 50;
        Debug.Log("begin drag");
    }
    #endregion
}
