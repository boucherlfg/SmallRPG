using UnityEngine;
using static Player;

public enum UseType
{
    Attack = 1,
    Mining = 2,
    Woodcutting = 4,
    Harvesting = 8,
    Loot = 16,
}
[CreateAssetMenu(menuName = "RPGItems/Tool")]
public class Tool : Item
{
    public UseType useType;
    public StatBlock stats;
    public override void Use()
    {
        var player = Game.Instance.Player;
        var target = Game.Instance.Agents.Find(agent => agent is IUsableAgent && agent.position == player.position + player.Orientation);
        if (target == null)
        {
            return;
        }
        (target as IUsableAgent).Use(player);
    }
}