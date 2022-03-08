using UnityEngine.Tilemaps;

public class TreeSource : Source
{
    const string tree_tag = "tree";
    public TreeSource() : base(ResourceType.Wood)
    {
        possibleItems.Add(Codex.Items["roots"]);
    }

    public override UseType UseType => UseType.Woodcutting;

    public override Tile CurrentTile => DisplayManager.Instance[tree_tag];

    public override string Name => "wood source";
}