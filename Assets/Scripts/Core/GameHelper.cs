using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GameHelper
{
    public static float Round(this float f, int place)
    {
        f *= Mathf.Pow(10, place);
        f = (int)f;
        f /= Mathf.Pow(10, place);
        return f;
    }
    public static Vector2Int Rotate90Right(this Vector2Int vect) => new Vector2Int(vect.y, -vect.x);
    
    public static Tile CreateTile(Sprite sprite)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        var color = tile.color;
        color.a = 1;
        tile.color = color;
        return tile;
    }
    public static bool CalculateHit(IStats attack, IStats defense)
    {
        var attackPrecision = attack.Stats.precision < 1 ? 1 : attack.Stats.precision;
        var defenseEvasion = defense.Stats.evasion < 1 ? 1 : attack.Stats.evasion;

        var result = Random.value < ((float)attackPrecision) / ((float)(1 + defenseEvasion));
        return result;
    }
    public static int CalculateDamage(IStats attack, IStats defense)
    {
        var dmg = (int)(attack.Stats.attack - defense.Stats.defense);
        return dmg <= 0 ? (Random.value < 0.5f ? 0 : 1) : dmg;
    }


    public static List<string> PopulateLoot(float value)
    {
        var loot = new List<string>();
        var choices = Codex.Items;
        float k = value;

        for (int i = 0; k > 0 && i < 10 && loot.Count < 3; i++) //continue populating until we got 3 objects, or value is depleted
        {
            Item item = DistributedRandom(choices.OrderBy(x => x.value));
            choices.Remove(item);
            if (value < item.value) continue;
            int max = (int)(k / item.value);
            if (max < 1) max = 1;

            i = 0;
            max = DistributedRandom(1, max > 3 ? 3 : max);
            for (int j = 0; j < max; j++)
            {
                loot.Add(item.name);
            }
            k -= item.value * max;
        }

        return loot;
    }
    public static List<string> PopulateLoot(AgentData agentData)
    {
        var loot = new List<string>();
        agentData.certainLoot.ForEach(item => loot.Add(item.name));
        var choices = new List<Item>(agentData.possibleLoot);
        if (agentData.useCodexForPossibleLoot) choices = Codex.Items;
        float k = agentData.value;


        for (int i = 0; k > 0 && i < 10 && loot.Count < 3; i++) //continue populating until we got 3 objects, or value is depleted
        {
            Item item = DistributedRandom(choices.OrderBy(x => x.value));
            choices.Remove(item);
            if (agentData.value < item.value) continue;
            int max = (int)(k / item.value);
            if (max < 1) max = 1;

            i = 0;
            max = DistributedRandom(1, max > 3 ? 3 : max);
            for (int j = 0; j < max; j++)
            {
                loot.Add(item.name);
            }
            k -= item.value * max;
        }

        return loot;
    }
    public static T Minimum<T>(this IEnumerable<T> list, System.Func<T, float> func)
    {
        if (list.Count() < 1) return default;
        T ret = list.First();
        foreach (T elem in list)
        {
            if (func(ret) > func(elem)) ret = elem;
        }
        return ret;
    }
    public static T Maximum<T>(this IEnumerable<T> list, System.Func<T, float> func)
    {
        if (list.Count() < 1) return default;
        T ret = list.First();
        foreach (T elem in list)
        {
            if (func(ret) < func(elem)) ret = elem;
        }
        return ret;
    }

    public static Rect GetBoundingBox(this Agent agent)
    {
        return new Rect(agent.position, Vector2.one);
    }
    public static Agent Raycast(Vector2 startingPoint, Vector2 endingPoint, float distance, params Agent[] exclude)
    {
        //techniquement O(n²) mais exclude sera toujours très petit
        var list = Game.Instance.Agents.FindAll(agent => agent is ICollision && !exclude.Contains(agent));
        list.Sort((x, y) => System.Math.Sign(Vector2.Distance(x.position, startingPoint) - Vector2.Distance(y.position, startingPoint)));

        float len = 0;
        while (len < distance || list.Count <= 0)
        {
            //avancer d'un closest à l'autre en raycastant sur la bounding box. devrait donner une recherche en O(n) -> O(n²) si on considère le while
            var closest = list.Minimum(x => Vector2.Distance(x.position, startingPoint));
            list.Remove(closest);

            len = Vector2.Distance(startingPoint, closest.position);
            if (len <= 0) continue;

            var hit = new Geometry.Ray(startingPoint, (endingPoint - startingPoint).normalized).Cast(closest.GetBoundingBox());
            if (hit != null)
            {
                return closest;
            }
        }
        return null;
    }

    class Geometry 
    {
        public struct Line
        {
            public Vector2 a;
            public Vector2 b;
            public Line(float ax, float ay, float bx, float by)
            {
                a = new Vector2(ax, ay);
                b = new Vector2(bx, by);
            }
        }
        public struct Ray
        {
            public Vector2 pos;
            public Vector2 dir;
            public Ray(Vector2 pos, Vector2 dir)
            {
                this.pos = pos;
                this.dir = dir;
            }
            private Vector2? Cast(Line line)
            {
                var denom = (line.a.x - line.b.x) * dir.y - (line.a.y - line.b.y) * dir.x;
                if (denom == 0) return null;

                var t = ((line.a.x - pos.x) * dir.y - (line.a.y - pos.y) * dir.x) / denom;
                var u = ((line.a.x - line.b.x) * (line.a.y - pos.y) - (line.a.y - line.b.y) * (line.a.x - pos.x)) / denom;

                if (t > 0 && t < 1 && u > 0)
                {
                    var ret = new Vector2();
                    ret.x = line.a.x + t * (line.b.x - line.a.x);
                    ret.y = line.a.y + t * (line.b.y - line.a.y);
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            public Vector2? Cast(Rect boundingBox)
            {
                return Cast(new Line[] 
                {
                    new Line(boundingBox.xMin, boundingBox.yMin, boundingBox.xMin, boundingBox.yMax), //left   0001
                    new Line(boundingBox.xMin, boundingBox.yMin, boundingBox.xMax, boundingBox.yMin), //bottom 0010
                    new Line(boundingBox.xMax, boundingBox.yMin, boundingBox.xMax, boundingBox.yMax), //right  1011
                    new Line(boundingBox.xMin, boundingBox.yMax, boundingBox.xMax, boundingBox.yMax), //top    0111
                });
            }
            public Vector2? Cast(Line[] lines)
            {
                var record = float.MaxValue;
                Vector2? closest = null;
                foreach (Line line in lines)
                {
                    var pt = Cast(line);
                    if (pt != null)
                    {
                        var d = Vector2.Distance(pos, pt.Value);
                        if(d < record) 
                        {
                            record = d;
                            closest = pt;
                        }
                    }
                }
                return closest;
            }
        }
    }

    public static System.Type[] GetAllSubTypes<T>() 
    {
        var ret = new List<System.Type>();
        foreach (var type in typeof(T).Assembly.GetTypes())
        {
            if (type.IsSubclassOf(typeof(T))) ret.Add(type);
        }
        return ret.ToArray();
    }
    public static float RarityDistribution(float x) => 0.017122f - ((Mathf.Log(-(x - 1.05263f))) / (Mathf.Log(20)));
    public static int DistributedRandom(float min, float max) => (int)Mathf.Clamp((RarityDistribution(Random.value) * max) + min, min, max);
    public static T DistributedRandom<T>(IEnumerable<T> list) => list.ElementAtOrDefault(DistributedRandom(0, list.Count()));
    public static T LinearRandom<T>(IEnumerable<T> list) => list.ElementAtOrDefault(Random.Range(0, list.Count()));
    public static Vector2Int RandomCardinal() => LinearRandom(new Vector2Int[] { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right });
}