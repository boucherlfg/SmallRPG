using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string visibleName;
    public string description;
    public float value;

    public abstract void Use();
}