using UnityEngine.Tilemaps;

public class Wall : Agent, IDrawable, ICollision, IOpaque
{
    const string wall_tag = "wall";
    public Tile CurrentTile => DisplayManager.Instance[wall_tag];

}