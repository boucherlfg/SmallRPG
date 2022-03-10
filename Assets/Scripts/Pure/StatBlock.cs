
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
            life = a.life + b.life,
            mana = a.mana + b.mana,
            attack = a.attack + b.attack,
            defense = a.defense + b.defense,
            precision = a.precision + b.precision,
            evasion = a.evasion + b.evasion
        };
    }
    public static StatBlock operator -(StatBlock a, StatBlock b) => a + -b;
    public static StatBlock operator -(StatBlock a) => -1 * a;
    public static StatBlock operator *(float k, StatBlock b)
    {
        return new StatBlock()
        {
            life = b.life * k,
            mana = b.mana * k,
            attack = b.attack * k,
            defense = b.defense * k,
            precision = b.precision * k,
            evasion = b.evasion * k,
        };
    }
}