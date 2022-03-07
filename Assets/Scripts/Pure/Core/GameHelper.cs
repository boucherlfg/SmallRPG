using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameHelper
{
    public static bool CalculateHit(IStats attack, IStats defense)
    {
        var total = PlayerData.Equipment.TotalBonus;
        float precisionBonus = attack is Player ? total.precision : 0;
        float evasionBonus = defense is Player ? total.evasion : 0;

        var attackPrecision = attack.Precision;
        var defenseEvasion = defense.Evasion;
        return Random.value < ((float)attackPrecision + precisionBonus) / ((float)(1 + defenseEvasion + evasionBonus));
    }
    public static int CalculateDamage(IStats attack, IStats defense)
    {
        int bonus = 0;
        if (attack is Player)
        {
            bonus += (int)PlayerData.Equipment.TotalBonus.attack;
        }
        else if (defense is Player)
        {
            bonus -= (int)PlayerData.Equipment.TotalBonus.defense;
        }
        var dmg = bonus + attack.Attack - defense.Defense;
        return dmg <= 0 ? (Random.value < 0.5f ? 0 : 1) : dmg;
    }

    public static List<string> PopulateLoot(float value)
    {
        var loot = new List<string>();
        var choices = new List<Item>(ItemsCodex.Instance.Items.OrderBy(x => x.value));
        float k = value;
        for (int i = 0; i < 10 && loot.Count < 3; i++) //infinite loop guard
        {
            Item item = DistributedRandom(choices);
            choices.Remove(item);
            int max = (int)(k / item.value);
            if (max < 1) continue;

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
    public static int DistributedRandom(float min, float max) => (int)Mathf.Clamp((RarityDistribution(UnityEngine.Random.value) * max) + min, min, max);
    public static T DistributedRandom<T>(IEnumerable<T> list) => list.ElementAtOrDefault(DistributedRandom(0, list.Count()));
    public static T LinearRandom<T>(IEnumerable<T> list) => list.ElementAtOrDefault(UnityEngine.Random.Range(0, list.Count()));
}