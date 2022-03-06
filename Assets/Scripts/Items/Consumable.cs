
public abstract class Consumable : ItemWithStats
{
    public int duration;
    public abstract void Consume();
    public sealed override void Use()
    {
        Consume();
        PlayerData.Inventory.Delete(name);
    }
}