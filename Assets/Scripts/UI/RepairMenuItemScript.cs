using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairMenuItemScript : MonoBehaviour
{
    private ItemState itemState;
    private Item item;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text label;

    public ItemState Item
    {
        get => itemState;
        set
        {
            itemState = value;
            item = Codex.Items[value.name];
            image.sprite = item.sprite;
            label.text = $"{itemState.durability}/{itemState.maxDurability}";
            GetComponent<DoWhenOver>().tooltipText = item.visibleName;
        }
    }
    public void Repair()
    {
        DataModel.Inventory.Items.Remove(itemState);
        itemState.durability = itemState.maxDurability;
        DataModel.Inventory.Items.Add(itemState);
        AudioManager.PlayAsSound("longMetal");
        UIManager.Repair.Refresh();
    }
}