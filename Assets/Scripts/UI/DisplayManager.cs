using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

class DisplayManager : MonoSingleton<DisplayManager>
{
    public void CreateDamageText(int damage, Vector2Int position)
    {
        GameObject go = Instantiate(damageTextPrefab);
        go.transform.position = (Vector2)position;
        go.GetComponent<DamageDisplayScript>().Damage = damage;
    }
    public GameObject damageTextPrefab;
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
        ResetBackground();

        var lights = Game.Instance.Agents.FindAll(x => x is ILightSource);
        foreach (var light in lights)
        {
            var nb = GameHelper.MinimumNumberOfCircleRadii((light as ILightSource).Range);
            for (float a = 0; a < Mathf.PI * 2; a += Mathf.PI * 2 / nb)
            {
                for (int len = 0; len < (light as ILightSource).Range; len++)
                {
                    var dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                    var pos = light.position + Vector2Int.RoundToInt(len * dir);

                    if (Game.Instance.Level.Ground.Exists(p => p == pos))
                    {
                        Background(pos.x, pos.y);
                    }

                    var agent = Game.Instance.Agents.Find(a => a.position == pos);

                    if (agent == null) continue;
                    var tile = (agent as IDrawable).CurrentTile;
                    tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), tile);

                    if (agent is Wall && !(agent is Player)) break;
                }
            }
        }
    }
    public void Draw2()
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