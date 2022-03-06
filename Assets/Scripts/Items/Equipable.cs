using UnityEngine;

public enum EquipType
{
    Head = 0,
    Body = 1,
    Leg = 2,
    Weapon = 3,
    Necklace = 4,
    Ring = 5
}
[CreateAssetMenu(menuName = "RPGItems/Equipment")]
public class Equipable : ItemWithStats
{
    public EquipType equipType;

    public override void Use()
    {
        PlayerData.Equipment.Equip(name, equipType);
        NotifManager.CreateNotification("you equiped " + visibleName);
    }
}