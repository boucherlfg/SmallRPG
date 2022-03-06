using UnityEngine.Tilemaps;

public interface IDrawable : IAgentFunction
{
    public Tile CurrentTile { get; }
}