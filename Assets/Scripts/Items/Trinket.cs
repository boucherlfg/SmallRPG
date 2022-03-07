using UnityEngine;

[CreateAssetMenu(menuName = "RPGItems/Junk")]
public class Trinket : Item
{
    public override void Equip()
    {
        NotifManager.CreateNotification("you can't equip that");
    }
    public override void Use()
    {
        NotifManager.CreateNotification("You can't use that right now.");
    }
}