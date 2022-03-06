using UnityEngine.Tilemaps;

public class Entrance : Agent, IDrawable, IActivatable
{
    const string entrance_tag = "entrance";
    public Tile CurrentTile => DisplayManager.Instance[entrance_tag];

    public void Activate(IMovable source)
    {
        if (source is Player) NotifManager.CreateNotification("you cant go back...");
    }
}