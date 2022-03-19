using UnityEngine;


[CreateAssetMenu(menuName = "Felix/Items/Resource")]
[InventoryCategory]
public class Resource : Item
{
    public Source.SourceType type;
    public override void Use()
    {
        UIManager.Notifications.CreateNotification("you can't use that right now.");
    }
}