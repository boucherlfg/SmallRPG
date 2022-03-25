using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Magic trap")]
public class MagicTrap : Trap
{
    [ScrollsBehaviours.ScrollBehaviourEnum]
    public string spell;
    public override Agent AgentFactory => new MagicTrapAgent(this);
}