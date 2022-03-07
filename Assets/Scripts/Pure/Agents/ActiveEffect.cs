using UnityEngine;

public class ActiveEffect : Agent, IUpdatable
{
    private IStats target;
    private Consumable item;
    public int duration;
    StatBlock accumulator;
    public ActiveEffect(Consumable item, IStats target)
    {
        this.target = target;
        this.item = item;
        this.duration = item.duration;

        target.Life += (int)item.heal.life;
        target.Mana += (int)item.heal.mana;
        target.Attack += (int)item.heal.attack;
        target.Defense += (int)item.heal.defense;
        target.Precision += (int)item.heal.precision;
        target.Evasion += (int)item.heal.evasion;

        if (target.Life < 0) Game.Instance.Destroy(target as Agent);
    }
    public void Update()
    {
        if (duration <= 1) Game.Instance.Destroy(this);


        accumulator += item.regen;

        if (Mathf.Abs(accumulator.life)  >= 1)
        {
            target.Life += (int)accumulator.life;
            accumulator.life -= (int)accumulator.life;
        }
        if (Mathf.Abs(accumulator.mana) >= 1)
        {
            target.Mana += (int)accumulator.mana;
            accumulator.mana -= (int)accumulator.mana;
        }
        if (Mathf.Abs(accumulator.attack) >= 1)
        {
            target.Attack += (int)accumulator.attack;
            accumulator.attack -= (int)accumulator.attack;
        }
        if (Mathf.Abs(accumulator.defense) >= 1)
        {
            target.Defense += (int)accumulator.defense;
            accumulator.defense -= (int)accumulator.defense;
        }
        if (Mathf.Abs(accumulator.precision) >= 1)
        {
            target.Precision += (int)accumulator.precision;
            accumulator.precision -= (int)accumulator.precision;
        }
        if (Mathf.Abs(accumulator.evasion) >= 1)
        {
            target.Evasion += (int)accumulator.evasion;
            accumulator.evasion -= (int)accumulator.evasion;
        }
        duration--;
        if (target.Life < 0) Game.Instance.Destroy(target as Agent);
    }
}