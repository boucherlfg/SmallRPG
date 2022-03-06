public class ActiveEffect : Agent, IUpdatable, IEndable
{
    private IStats target;
    private Consumable item;
    public int duration;
    public ActiveEffect(Consumable item, IStats target)
    {
        this.target = target;
        this.item = item;
        this.duration = item.duration;

        target.MaxLife += item.buff.life;
        target.MaxMana += item.buff.mana;
        target.MaxAttack += item.buff.attack;
        target.MaxDefense += item.buff.defense;
        target.MaxPrecision += item.buff.precision;
        target.MaxEvasion += item.buff.evasion;

        target.Life += item.heal.life;
        target.Mana += item.heal.mana;
        target.Attack += item.heal.attack;
        target.Defense += item.heal.defense;
        target.Precision += item.heal.precision;
        target.Evasion += item.heal.evasion;

        if (target.Life < 0) Game.Instance.Destroy(target as Agent);
    }
    public void Update()
    {
        if (duration <= 1) Game.Instance.Destroy(this);
        target.Life += item.regen.life;
        target.Mana += item.regen.mana;
        target.Attack += item.regen.attack;
        target.Defense += item.regen.defense;
        target.Precision += item.regen.precision;
        target.Evasion += item.regen.evasion;
        duration--;
        if (target.Life < 0) Game.Instance.Destroy(target as Agent);
    }

    public void End()
    {
        target.MaxLife -= item.buff.life;
        target.MaxMana -= item.buff.mana;
        target.MaxAttack -= item.buff.attack;
        target.MaxDefense -= item.buff.defense;
        target.MaxPrecision -= item.buff.precision;
        target.MaxEvasion -= item.buff.evasion;
    }
}