public class ScrapingTable : CraftingStation
{
    protected override string Crafting_Tag => "scraping";

    protected override CraftingType craftingType => CraftingType.Scraping;
    public override void Activate(IMovable user)
    {
        if (UIManager.Scraping.Active) UIManager.Crafting.Active = false;
        UIManager.Scraping.Toggle();
    }
}