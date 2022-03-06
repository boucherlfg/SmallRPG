using UnityEngine;

[CreateAssetMenu(menuName = "RPGItems/Throwable")]
public class Throwable : Consumable
{
    public override void Consume()
    {
        NotifManager.CreateNotification("you threw " + visibleName);
        var player = Game.Instance.Player;
        var startPosition = player.position + player.Orientation;
        for (int i = 0; i < 10; i++)
        {
            var agent = Game.Instance.Agents.Find(x => x.position == startPosition);
            if (agent != null && agent is ICollision)
            {
                if (agent is IStats)
                {
                    var effect = new ActiveEffect(this, agent as IStats);
                    Game.Instance.Agents.Add(effect);
                    if (agent is Foe)
                    {
                        NotifManager.CreateNotification("and it landed on an enemy");
                    }
                    else
                    {
                        NotifManager.CreateNotification("and it landed on a friend!");
                    }
                }
                else
                {
                    NotifManager.CreateNotification("and it landed on a wall");
                }
            }
            startPosition += player.Orientation;
        }
        
    }
}