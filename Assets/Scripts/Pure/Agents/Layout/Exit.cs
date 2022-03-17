using UnityEngine.Tilemaps;

public class Exit : Agent, IActivatable, ICollision, IDrawable
{
    const string exit_tag = "exit";
    public Tile CurrentTile => DisplayManager.Instance[exit_tag];

    public void Activate(IMovable source)
    {
        AudioManager.PlayAsSound(exit_tag);
        UIManager.ExitPrompt.Toggle();
    }
}