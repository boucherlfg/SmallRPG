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
[InventoryCategory]
public class Tool : Item
{
    public UseType useType;
    public StatBlock stats;

    public override void Equip()
    {
        UIManager.Notifications.CreateNotification("you don't need to equip that tool");
    }
    public override void Unequip()
    {
        base.Unequip();
    }
    public override void Use()
    {
        UIManager.Notifications.CreateNotification("you don't need to activate that tool");
    }
}