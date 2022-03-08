﻿using System.Collections.Generic;
using UnityEngine.Tilemaps;

public abstract class CraftingStation : Agent, IUsableAgent, IDrawable, ICollision
{
    protected abstract string Crafting_Tag { get; }
    protected abstract CraftingType craftingType { get; }
    public CraftingStation()
    {
    }
    public Tile CurrentTile => DisplayManager.Instance[Crafting_Tag];
    public void Use(Player user)
    {
        UIManager.Crafting.Active = true;
        UIManager.Crafting.CraftingType = craftingType;
    }
}