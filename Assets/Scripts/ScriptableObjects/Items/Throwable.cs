using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Throwable")]
[InventoryCategory]
public class Throwable : Consumable
{
    public override void Consume()
    {
        UIManager.Notifications.CreateNotification("you threw " + visibleName);
        var player = Game.Instance.Player;
        var currentPosition = player.position + player.Orientation;
        for (int i = 0; i < 10; i++)
        {
            var agent = Game.Instance.Agents.Find(x => x.position == currentPosition);
            if (agent != null && agent is ICollision)
            {
                if (agent is IStats)
                {
                    var hit = GameHelper.CalculateHit(player, agent as IStats);
                    if (hit)
                    {
                        if (agent is Mob)
                        {
                            var buff = new Buff(() => agent, heal, regen, resolve, duration);
                            DataModel.ActiveBuffs.Add(buff);
                            UIManager.Notifications.CreateNotification($"and it landed on a {(agent as Mob).data.visibleName}!");
                            return;
                        }
                    }
                }
                UIManager.Notifications.CreateNotification($"and landed on {(Random.value < 0.5 ? "a wall" : "the ceiling")}.");
                return;

            }
            currentPosition += player.Orientation;
        }
        
    }
}