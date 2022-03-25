using UnityEngine.Tilemaps;

public class IceAgent : Agent, IUpdatable, ICollision, IDrawable, IActivatable
{
    private int counter = 10;

    public Tile CurrentTile => DisplayManager.Instance["ice"];

    public void Activate(IMovable source)
    {
        Game.Instance.Destroy(this);
        AudioManager.PlayAsSound("empty");
    }

    public void Update()
    {
        if (counter < 0)
        {
            Game.Instance.Destroy(this);
            AudioManager.PlayAsSound("empty", volume: 0.25f);
        }
        counter--;
    }
}