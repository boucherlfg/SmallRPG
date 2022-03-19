
public abstract class Consumable : Item
{
    public StatBlock heal;
    public StatBlock regen;
    public StatBlock resolve;
    public int duration;
    public abstract void Consume();
    public sealed override void Use()
    {
        Consume();
        DataModel.Inventory.Delete(this.name);
        
    }
}