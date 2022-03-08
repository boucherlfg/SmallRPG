public class Workbench : CraftingStation
{
    protected override string Crafting_Tag => "workbench";

    protected override CraftingType craftingType => CraftingType.Bench;
}