public class RepairStation : CraftingStation
{
    protected override string Crafting_Tag => "repair";

    protected override CraftingType craftingType => CraftingType.Repair;

    public override void Activate(IMovable user)
    {
        if (UIManager.Repair.Active) UIManager.Crafting.Active = false;
        UIManager.Repair.Toggle();
    }
}