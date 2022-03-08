using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Potion")]
class Potion : Consumable
{
    public override void Consume()
    {
        Game.Instance.Create(new ActiveEffect(this, Game.Instance.Player));
        UIManager.Notifications.CreateNotification($"you just drank {visibleName}.");
    }
}
