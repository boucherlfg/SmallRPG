using UnityEngine;

[CreateAssetMenu(menuName = "RPGItems/Junk")]
public class Junk : Item
{
    public override void Use()
    {
        NotifManager.CreateNotification("not much use to that");
    }
}