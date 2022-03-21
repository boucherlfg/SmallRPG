using UnityEngine.Tilemaps;

public class FloorItem : Agent, IActivatable, IDrawable
{
    private Item reference;

    private Tile tile;
    public Tile CurrentTile
    {
        get
        {
            if (!tile)
            {
                tile = GameHelper.CreateTile(reference.sprite);
            }
            return tile;
        }
    }

    public ItemState droppedItemState;
    public FloorItem(Item reference)
    {
        this.reference = reference;
        droppedItemState = reference;
    }
    private void Pickup()
    {
        AudioManager.PlayAsSound("equip");
        if (droppedItemState.name != null)
        {
            DataModel.Inventory.Add(reference.name, droppedItemState.durability);
        }
        else
        {
            DataModel.Inventory.Add(reference.name);
        }
        UIManager.Notifications.CreateNotification("you just found " + reference.visibleName);
        Game.Instance.Destroy(this);
    }
    public void Activate(IMovable user)
    {
        if (user is Player)
        {
            Pickup();
        }
    }
}