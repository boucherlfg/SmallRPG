using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Scroll")]
public class Scroll : Item
{
    public override void Use()
    {
        UIManager.Notifications.CreateNotification("unimplemented yet *wink wink*");
    }
}