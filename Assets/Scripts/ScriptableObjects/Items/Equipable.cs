using UnityEngine;

public enum EquipType
{
    None = -1,
    Head = 0,
    Body = 1,
    Leg = 2,
    Necklace = 3,
    Ring = 4, 
    Weapon = 5,
    
}
[CreateAssetMenu(menuName = "Felix/Items/Equipment")]
[InventoryCategory]
public class Equipable : Item
{
    public EquipType equipType;
    public StatBlock buff;
    
    public override void Use()
    {
        var currentlyEquiped = DataModel.Equipment[equipType];
        if (currentlyEquiped == name)
        {
            Unequip();
        }
        else
        {
            DataModel.Equipment.Equip(name, equipType);
            UIManager.Notifications.CreateNotification("you equiped " + visibleName);
            AudioManager.PlayAsSound("equip");
        }
    }
    public void Unequip()
    {
        DataModel.Equipment.Unequip(equipType);
        UIManager.Notifications.CreateNotification("you unequiped " + visibleName);
        AudioManager.PlayAsSound("equip");
    }
}