using UnityEngine;
using UnityEngine.Tilemaps;

public class Tombstone : Player
{
    const string tombstone_tag = "tombstone";
    public override Tile CurrentTile => DisplayManager.Instance[tombstone_tag];
}