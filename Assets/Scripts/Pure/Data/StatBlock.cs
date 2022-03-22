
using UnityEngine;

[System.Serializable]
public struct StatBlock
{
    public float life;
    public float mana;
    public float attack;
    public float defense;
    public float precision;
    public float evasion;

    public static StatBlock operator +(StatBlock a, StatBlock b)
    {
        return new StatBlock()
        {
            life = (a.life + b.life).Round(2),
            mana = (a.mana + b.mana).Round(2),
            attack = (a.attack + b.attack).Round(2),
            defense = (a.defense + b.defense).Round(2),
            precision = (a.precision + b.precision).Round(2),
            evasion = (a.evasion + b.evasion).Round(2)
        };
    }
    public static StatBlock operator -(StatBlock a, StatBlock b) => a + -b;
    public static StatBlock operator -(StatBlock a) => -1 * a;
    public static StatBlock operator *(float k, StatBlock b)
    {
        return new StatBlock()
        {
            life = (b.life * k).Round(2),
            mana = (b.mana * k).Round(2),
            attack = (b.attack * k).Round(2),
            defense = (b.defense * k),
            precision = (b.precision * k).Round(2),
            evasion = (b.evasion * k).Round(2),
        };
    }
    public override string ToString()
    {
        return $"({life}, {mana}, {attack}, {defense}, {precision}, {evasion})";
    }
}