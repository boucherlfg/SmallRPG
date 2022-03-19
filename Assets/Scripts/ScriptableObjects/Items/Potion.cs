using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Potion")]
[InventoryCategory]
class Potion : Consumable
{
    public override void Consume()
    {
        var buff = new Buff(() => Game.Instance.Player, heal, regen, resolve, duration);
        DataModel.ActiveBuffs.Add(buff);
        UIManager.Notifications.CreateNotification($"you just drank {visibleName}.");
        AudioManager.PlayAsSound("potion");
    }
}
