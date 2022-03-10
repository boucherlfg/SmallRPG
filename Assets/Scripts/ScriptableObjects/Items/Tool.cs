using UnityEngine;
using static Player;

public enum UseType
{
    Mining = 1,
    Woodcutting = 2,
    Harvesting = 4,
    Loot = 8,
}
[CreateAssetMenu(menuName = "Felix/Items/Tool")]
public class Tool : Item
{
    public UseType useType;
    public StatBlock stats;
    public override void Use()
    {
        var player = Game.Instance.Player;
        var target = Game.Instance.Agents.FindLast(agent => agent is IUsableAgent && agent.position == player.position + player.Orientation);
        if (target == null)
        {
            return;
        }
        (target as IUsableAgent).Use(player);
    }
}