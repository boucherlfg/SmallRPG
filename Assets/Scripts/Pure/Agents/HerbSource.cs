using UnityEngine.Tilemaps;

public class HerbSource : Resource
{
    const string herb_tag = "herb";
    public HerbSource() : base(ResourceType.Herb)
    {
    }

    public override string Name => "herbs";
    public override Tile CurrentTile => DisplayManager.Instance[herb_tag];
}