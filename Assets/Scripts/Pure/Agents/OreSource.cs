using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OreSource : Source
{
    const string ore_source_tag = "ore_source";
    public override string Name => "rock";

    public override Tile CurrentTile => DisplayManager.Instance[ore_source_tag];
    public override UseType UseType => UseType.Mining;
    public OreSource() : base(ResourceType.Gem | ResourceType.Ore)
    {
        possibleItems.Add(ItemsCodex.Instance["pebble"]);
    }
    
    
}