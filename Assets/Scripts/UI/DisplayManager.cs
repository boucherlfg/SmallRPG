using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

    [Serializable]
    public struct TileWithTag
    {
        public string tag;
        public Tile tile;
    }

class DisplayManager : MonoSingleton<DisplayManager>
{
    public Tile this[string tag] => tilePerType.Exists(x => x.tag == tag) ? tilePerType.Find(x => x.tag == tag).tile : null;
    [SerializeField]
    private List<TileWithTag> tilePerType;
    [SerializeField]
    private Tilemap tilemap;
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

    public static DisplayManager Instance => _instance;
}