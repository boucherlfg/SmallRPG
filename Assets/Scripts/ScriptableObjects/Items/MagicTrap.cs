using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Magic trap")]
public class MagicTrap : Trap
{
    [SpellsBehaviours.SpellBehaviourEnum]
    public string spell;
    public override Agent AgentFactory => new MagicTrapAgent(this);
}