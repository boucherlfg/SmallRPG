using UnityEngine.Tilemaps;

public class DoorAgent : Agent, ICollision, IOpaque, IActivatable, IDrawable
{
    public bool locked;

    public Tile CurrentTile => DisplayManager.Instance["door"];

    public void Activate(IMovable source)
    {
        if (!(source is Player)) return;

        if (!locked)
        {
            AudioManager.PlayAsSound("door", volume: 0.25f);
            Game.Instance.Destroy(this);
            return;
        }

        var tool = Codex.Items.Find(x => x is Tool && (x as Tool).useType == UseType.Loot);
        var hasTool = DataModel.Inventory.HowMany(tool.name) > 0;

        if (!hasTool)
        {
            UIManager.Notifications.CreateNotification("the door is locked");
            return;
        }
        AudioManager.PlayAsSound("door", volume : 0.25f);
        Game.Instance.Destroy(this);
    }
}