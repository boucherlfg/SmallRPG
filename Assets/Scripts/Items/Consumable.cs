
public abstract class Consumable : Item
{
    public StatBlock heal;
    public StatBlock regen;
    public int duration;
    public abstract void Consume();
    public sealed override void Use()
    {
        Consume();
        PlayerData.Equipment.ConsumeTool();
        if (PlayerData.Inventory.HowMany(name) > 0)
        {
            PlayerData.Equipment.Tool = this;
        }
        
    }
}