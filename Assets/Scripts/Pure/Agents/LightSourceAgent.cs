using UnityEngine.Tilemaps;

public class LightSourceAgent : Agent, ILightSource, IDrawable, IActivatable, IEndable, ICollision
{
    public Tile CurrentTile => GameHelper.CreateTile(lightSource.sprite);

    public LightSource lightSource;

    public float Range => lightSource.range;

    public void Activate(IMovable source)
    {

        Game.Instance.Destroy(this);
    }

    public void End()
    {
        AudioManager.PlayAsSound("empty");
    }
}