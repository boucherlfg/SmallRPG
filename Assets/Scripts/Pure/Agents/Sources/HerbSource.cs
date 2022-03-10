using UnityEngine.Tilemaps;

public class HerbSource : Source
{
    public HerbSource() : base(SourceType.Foraging)
    {
        possibleItems.Add(Codex.Items["weeds"]);
    }
    public override UseType UseType => UseType.Harvesting;
    public override string Name => "herb";
    public override Tile CurrentTile => DisplayManager.Instance[Name];
}