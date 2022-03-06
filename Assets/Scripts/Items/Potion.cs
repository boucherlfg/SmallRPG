using UnityEngine;

[CreateAssetMenu(menuName = "RPGItems/Potion")]
class Potion : Consumable
{
    public override void Consume()
    {
        Game.Instance.Create(new ActiveEffect(this, Game.Instance.Player));
        NotifManager.CreateNotification($"you just drank {visibleName}.");
    }
}
