using UnityEngine;

public enum ResourceType
{
    Wood = 1,
    Ore = 2,
    Bar = 4,
    Tissue = 8,
    Gem = 16,
    Herb = 32,

}

[CreateAssetMenu(menuName = "Felix/Items/Resource")]
public class Resource : Item
{
    public ResourceType type;
    public override void Equip()
    {
        UIManager.Notifications.CreateNotification("you can't equip that.");
    }
    public override void Use()
    {
        UIManager.Notifications.CreateNotification("you can't use that right now.");
    }
}