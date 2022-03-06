using UnityEngine.Tilemaps;

public class Exit : Agent, IActivatable, IDrawable
{
    const string exit_tag = "exit";
    public Tile CurrentTile => DisplayManager.Instance[exit_tag];

    public void Activate(IMovable source)
    {
        if (!(source is Player)) return;
        (source as Player).state = new Player.ExitState(source as Player);
    }
}