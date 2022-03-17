public class ScrapingTable : CraftingStation
{
    protected override string Crafting_Tag => "scraping";

    protected override CraftingType craftingType => CraftingType.Scraping;
    public override void Activate(IMovable user)
    {
        UIManager.Scraping.Toggle();
    }
}