using UnityEngine.Tilemaps;

public class LureAgent : Agent, IDrawable, IActivatable, IEndable
{
    public Lure lure;
    public LureAgent(Lure lure)
    {
        this.lure = lure;
    }

    public void Activate(IMovable source)
    {
        Game.Instance.Destroy(this);
    }

    public void End()
    {
        AudioManager.PlayAsSound("empty");
    }

    private Tile tile;
    public Tile CurrentTile
    {
        get
        {
            if (!tile)
            {
                tile = GameHelper.CreateTile(lure.sprite);
                var color = tile.color;
                color *= 0.8f;
                color.a = 1;
                tile.color = tile.color;
            }
            return tile;
        }
    }
}