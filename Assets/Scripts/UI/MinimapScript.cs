using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoSingleton<MinimapScript>
{
    const int width = 100;
    const int height = 100;
    const int res = 20;

    public Image image;
    public static MinimapScript Instance => _instance;
    public void UpdateMinimap()
    {
        Texture2D texture = new Texture2D(width, height);
        var rooms = Game.Instance.Rooms;

        var center = rooms.Find(x => x.Ground.Contains(Game.Instance.Player.position)).position;
        
        foreach (Room r in rooms)
        {
            if (!r.discovered) continue;
            //draw corridors
            foreach (Vector2Int door in r.doors)
            {
                var r2 = rooms.Find(x => x.position == r.position + door);
                if (!r2.discovered) continue;

                var ax = width / 2 + (-center.x + r.position.x) * res;
                var ay = height / 2 + (-center.y + r.position.y) * res;
                var bx = width / 2 + (-center.x + r.position.x + door.x) * res;
                var by = height / 2 + (-center.y + r.position.y + door.y) * res;

                DrawLine(texture, ax + door.y, ay - door.x, bx + door.y, by - door.x, Color.gray);
                DrawLine(texture, ax - door.y, ay + door.x, bx - door.y, by + door.x, Color.gray);
                DrawLine(texture, ax, ay, bx, by, Color.gray);
            }
        }

        foreach (Room r in rooms)
        {
            if (!r.discovered) continue;

            var x = width / 2 + (-center.x + r.position.x) * res - res / 2;
            var y = height / 2 + (-center.y + r.position.y) * res - res / 2;
            
            var color = r is StartingRoom ? Color.red : r is EndRoom ? Color.green : Color.gray;
            if (r.position == center) color = Color.yellow;

            DrawRect(texture, x, y, res - 3, res - 3, color);
        }

        texture.Apply();
        image.sprite = Sprite.Create(texture, new Rect(0, 0, 100, 100), Vector2.one/2, 100);
    }

    void DrawLine(Texture2D tex, int ax, int ay, int bx, int by, Color color)
    {
        Vector2 start = new Vector2(ax, ay);
        Vector2 end = new Vector2(bx, by);
        Vector2 v = end - start;
        Vector2Int u = Vector2Int.RoundToInt(v.normalized);

        Vector2Int current = Vector2Int.RoundToInt(start);
        for (int i = 0; i < v.magnitude; i++)
        {
            if (current.x < 0 || current.x > MinimapScript.width) continue;
            if (current.y < 0 || current.y > MinimapScript.height) continue;
            tex.SetPixel((int)current.x, (int)current.y, color);
            current += u;
            current = Vector2Int.RoundToInt(current);
        }
    }
    
    void DrawRect(Texture2D tex, int x, int y, int width, int height, Color color)
    {
        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                if (i < 0 || i > MinimapScript.width) continue;
                if (j < 0 || j > MinimapScript.height) continue;
                tex.SetPixel(i, j, color);
            }
        }
    }
}
