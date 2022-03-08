using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GameHelper
{
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
        var total = DataModel.Equipment.TotalBonus;
        float precisionBonus = attack is Player ? total.precision : 0;
        float evasionBonus = defense is Player ? total.evasion : 0;

        var attackPrecision = attack.Precision;
        var defenseEvasion = defense.Evasion;
        var result = Random.value < ((float)attackPrecision + precisionBonus) / ((float)(1 + defenseEvasion + evasionBonus));
        return result;
    }
    public static int CalculateDamage(IStats attack, IStats defense)
    {
        int bonus = 0;
        if (attack is Player)
        {
            bonus += (int)DataModel.Equipment.TotalBonus.attack;
        }
        else if (defense is Player)
        {
            bonus -= (int)DataModel.Equipment.TotalBonus.defense;
        }
        var dmg = bonus + attack.Attack - defense.Defense;
        return dmg <= 0 ? (Random.value < 0.5f ? 0 : 1) : dmg;
    }


    public static List<string> PopulateLoot(float value)
    {
        var loot = new List<string>();
        var choices = Codex.Items;
        float k = value;

        for (int i = 0; k > 0 && i < 10 && loot.Count < 3; i++) //continue populating until we got 3 objects, or value is depleted
        {
            Item item = DistributedRandom(choices);
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

    public static List<string> PopulateLoot(AgentData agentData)
    {
        var loot = new List<string>();
        agentData.certainLoot.ForEach(item => loot.Add(item.name));
        var choices = agentData.possibleLoot;
        if (agentData.useCodexForPossibleLoot) choices = Codex.Items;
        float k = agentData.value;


        for (int i = 0; k > 0 && i < 10 && loot.Count < 3; i++) //continue populating until we got 3 objects, or value is depleted
        {
            Item item = DistributedRandom(choices);
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
}