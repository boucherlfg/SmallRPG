using UnityEngine;

[CreateAssetMenu(menuName = "RPGItems/Scroll")]
public class Scroll : Item
{
    public override void Use()
    {
        NotifManager.CreateNotification("not much use to that");
    }
}