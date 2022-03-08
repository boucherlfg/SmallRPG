using UnityEngine.Tilemaps;

public class FloorItem : Agent, IUsableAgent, IDrawable
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
        DataModel.Inventory.Add(reference.name);
        UIManager.Notifications.CreateNotification("you just found " + reference.visibleName);
        Game.Instance.Destroy(this);
    }
    public void Use(Player user)
    {
        Pickup();
    }
}