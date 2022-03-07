﻿using UnityEngine;

public enum ResourceType
{
    Wood = 1,
    Ore = 2,
    Bar = 4,
    Tissue = 8,
    Gem = 16,
    Herb = 32,

}

[CreateAssetMenu(menuName = "RPGItems/Resource")]
public class Resource : Item
{
    public ResourceType type;
    public override void Equip()
    {
        NotifManager.CreateNotification("you can't equip that.");
    }
    public override void Use()
    {
        NotifManager.CreateNotification("you can't use that right now.");
    }
}