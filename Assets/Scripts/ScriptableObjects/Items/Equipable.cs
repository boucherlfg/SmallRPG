using UnityEngine;

public enum EquipType
{
    Head = 0,
    Body = 1,
    Leg = 2,
    Necklace = 3,
    Ring = 4, 
    Weapon = 5,
}
[CreateAssetMenu(menuName = "Felix/Items/Equipment")]
public class Equipable : Item
{
    public EquipType equipType;
    public StatBlock buff;
    public override void Equip()
    {
        AudioManager.PlayAsSound("equip");
        DataModel.Equipment.Equip(name, equipType);
        UIManager.Notifications.CreateNotification("you equiped " + visibleName);
    }
    public override void Unequip()
    {
        AudioManager.PlayAsSound("equip");
        DataModel.Equipment.Unequip(equipType);
        UIManager.Notifications.CreateNotification("you unequiped " + visibleName);
    }
    public override void Use()
    {
        UIManager.Notifications.CreateNotification("you cant use that, but you can equip it");
    }
}