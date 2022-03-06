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

[CreateAssetMenu(menuName = "RPGItems/Resource")]
public class ResourceItem : Item
{
    public ResourceType type;
    public override void Use()
    {
        NotifManager.CreateNotification("Start crafting by using a crafting station");
    }
}