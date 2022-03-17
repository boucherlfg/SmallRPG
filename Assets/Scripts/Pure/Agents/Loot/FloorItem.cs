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

    public FloorItem(Item reference)
    {
        this.reference = reference;
    }
    private void Pickup()
    {
        AudioManager.PlayAsSound("equip");
        DataModel.Inventory.Add(reference.name);
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