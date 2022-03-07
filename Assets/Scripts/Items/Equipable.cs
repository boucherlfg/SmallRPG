using UnityEngine;

public enum EquipType
{
    Head = 0,
    Body = 1,
    Leg = 2,
    Necklace = 3,
    Ring = 4
}
[CreateAssetMenu(menuName = "RPGItems/Equipment")]
public class Equipable : Item
{
    public EquipType equipType;
    public StatBlock buff;
    public override void Equip()
    {
        PlayerData.Equipment.Equip(name, equipType);
        NotifManager.CreateNotification("you equiped " + visibleName);
    }
    public override void Unequip()
    {
        PlayerData.Equipment.Unequip(equipType);
        NotifManager.CreateNotification("you unequiped " + visibleName);
    }
    public override void Use()
    {
        NotifManager.CreateNotification("you cant use that, but you can equip it");
    }
}