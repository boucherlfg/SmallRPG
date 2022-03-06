using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameHelper
{
    public static bool CalculateHit(IStats attack, IStats defense)
    {
        var attackPrecision = attack.Precision;
        var defenseEvasion = defense.Evasion;
        return Random.value < ((float)attackPrecision) / ((float)(1 + defenseEvasion));
    }
    public static int CalculateDamage(IStats attack, IStats defense)
    {
        return attack.Attack - defense.Defense < 0 ? (Random.value < 0.5f ? 0 : 1) : attack.Attack - defense.Defense;
    }

    public static List<string> PopulateLoot(float value)
    {
        var loot = new List<string>();

        float k = value;
        for (int i = 0; i < 10; i++) //infinite loop guard
        {
            int f = DistributedRandom(0, ItemsCodex.Instance.Items.Count);
            Item item = ItemsCodex.Instance.Items.OrderBy(x => x.value).ToArray()[f];
            int max = (int)(k / item.value);
            if (max < 1) continue;

            i = 0;
            max = DistributedRandom(1, max > 5 ? 5 : max);
            for (int j = 0; j < max; j++)
            {
                loot.Add(item.name);
            }
            k -= item.value * max;
        }
        return loot;
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
    public static int DistributedRandom(float min, float max) => (int)Mathf.Clamp((RarityDistribution(UnityEngine.Random.value) * max) + min, min, max);
    public static T DistributedRandom<T>(IEnumerable<T> list) => list.ElementAtOrDefault(DistributedRandom(0, list.Count()));
    public static T LinearRandom<T>(IEnumerable<T> list) => list.ElementAtOrDefault(UnityEngine.Random.Range(0, list.Count()));
}