using UnityEngine.Tilemaps;

public class Smelter : CraftingStation, IDrawable, ICollision
{
    
    protected override CraftingType craftingType => CraftingType.Smelting;

    protected override string Crafting_Tag => "smelter";
}