using UnityEngine;

public interface IMovable : IAgentFunction
{
    public bool Immobilized { get; set; }
    public Vector2Int Orientation { get; set; }
}