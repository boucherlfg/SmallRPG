using UnityEngine.Tilemaps;

public class Floor : Agent, IDrawable
{
    const string floor_tag = "floor";
    public Tile CurrentTile => DisplayManager.Instance[floor_tag];
}