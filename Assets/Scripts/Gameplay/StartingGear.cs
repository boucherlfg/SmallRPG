using System.Collections.Generic;

public class StartingGear : MonoSingleton<StartingGear>
{
    public List<Item> inventory;
    public List<Equipable> equipment;
    public Item tool;

    public static List<Item> Inventory => _instance.inventory;
    public static List<Equipable> Equipment => _instance.equipment;
    public static Item Tool => _instance.tool;
}