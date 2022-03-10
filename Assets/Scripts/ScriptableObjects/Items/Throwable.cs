using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Throwable")]
public class Throwable : Consumable
{
    public override void Consume()
    {
        UIManager.Notifications.CreateNotification("you threw " + visibleName);
        var player = Game.Instance.Player;
        var startPosition = player.position + player.Orientation;
        for (int i = 0; i < 10; i++)
        {
            var agent = Game.Instance.Agents.Find(x => x.position == startPosition);
            if (agent != null && agent is ICollision)
            {
                if (agent is IStats)
                {
                    var buff = new Buff(() => agent, heal, regen, resolve, duration);
                    DataModel.ActiveBuffs.Add(buff);
                    if (agent is Mob)
                    {
                        UIManager.Notifications.CreateNotification($"and it landed on a {(agent as Mob).data.visibleName}!");
                        return;
                    }
                    else
                    {
                        UIManager.Notifications.CreateNotification("and it landed on a friend!");
                        return;
                    }
                }
                else
                {
                    UIManager.Notifications.CreateNotification("and it landed on a wall");
                    return;
                }
            }
            startPosition += player.Orientation;
        }
        
    }
}