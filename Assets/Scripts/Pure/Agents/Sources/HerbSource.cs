using UnityEngine.Tilemaps;

public class HerbSource : Source
{
    const string herb_tag = "herb";
    public HerbSource() : base(ResourceType.Herb)
    {
        possibleItems.Add(Codex.Items["weeds"]);
    }
    public override UseType UseType => UseType.Harvesting;
    public override string Name => "herbs";
    public override Tile CurrentTile => DisplayManager.Instance[herb_tag];
}