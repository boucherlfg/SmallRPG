using UnityEngine;

public class Ally : Agent, IMovable, ICollision
{
    public bool Immobilized { get; set; }
    public Vector2Int Orientation { get; set; }
}