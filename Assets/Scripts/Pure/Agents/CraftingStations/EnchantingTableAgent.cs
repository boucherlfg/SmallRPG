public class EnchantingTableAgent : CraftingStation
{
    protected override string Crafting_Tag => "enchanting";

    protected override CraftingType craftingType => CraftingType.Enchanting;
}