
[System.Serializable]
public struct StatBlock
{
    public int life;
    public int mana;
    public int attack;
    public int defense;
    public int precision;
    public int evasion;

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
}
public abstract class ItemWithStats : Item
{
    public StatBlock heal;
    public StatBlock buff;
    public StatBlock regen;
}