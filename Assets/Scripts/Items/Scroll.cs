using UnityEngine;

[CreateAssetMenu(menuName = "RPGItems/Scroll")]
public class Scroll : Item
{
    public override void Use()
    {
        NotifManager.CreateNotification("unimplemented yet *wink wink*");
    }
}