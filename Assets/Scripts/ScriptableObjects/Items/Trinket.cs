using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Junk")]
[InventoryCategory]
public class Trinket : Item
{
    public override void Equip()
    {
        UIManager.Notifications.CreateNotification("you can't equip that");
    }
    public override void Use()
    {
        UIManager.Notifications.CreateNotification("You can't use that right now.");
    }
}