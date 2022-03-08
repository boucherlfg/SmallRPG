
public abstract class Consumable : Item
{
    public StatBlock heal;
    public StatBlock regen;
    public int duration;
    public abstract void Consume();
    public sealed override void Use()
    {
        Consume();
        DataModel.Equipment.ConsumeTool();
        if (DataModel.Inventory.HowMany(name) > 0)
        {
            DataModel.Equipment.Tool = this;
        }
        
    }
}