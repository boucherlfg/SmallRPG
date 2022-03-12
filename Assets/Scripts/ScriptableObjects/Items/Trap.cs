
using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Trap")]
public class Trap : Placeable
{
    public override Agent AgentFactory => new TrapAgent(this);
    public StatBlock heal;
    public StatBlock regen;
    public StatBlock resolve;
    public int duration;
}