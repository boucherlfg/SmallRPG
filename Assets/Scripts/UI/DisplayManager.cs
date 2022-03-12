using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class DisplayManager : MonoSingleton<DisplayManager>
{
    public Tile this[string tag] => tilePerType.Find(x => x.name == tag);
    [SerializeField]
    private List<Tile> tilePerType;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Tilemap background;
    [SerializeField]
    private Tile floorTile;
    private void Clear()
    {
        tilemap.ClearAllTiles();
    }
    public void Draw()
    {
        Clear();
        Game.Instance.Agents.FindAll(agent => agent is IDrawable).ForEach(agent =>
        {
            var pos = new Vector3Int(Mathf.RoundToInt(agent.position.x), Mathf.RoundToInt(agent.position.y), 0);
            var tile = (agent as IDrawable).CurrentTile;
            tilemap.SetTile(pos, tile);
        });
    }

    public void ResetBackground() => background.ClearAllTiles();
    public void Background(int x, int y)
    {
        background.SetTile(new Vector3Int(x, y, 0), floorTile);
    }
    public static DisplayManager Instance => _instance;
}