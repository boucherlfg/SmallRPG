using UnityEngine;


[InventoryCategory]
public abstract class Item : ScriptableObject
{
   

    public string visibleName;
    public Sprite sprite;
    public string description;
    public float value;

    public int durability;

    static int position = 1;
    public void Prepare()
    {
        if (this is Equipable)
        {
            Use();
        }
        else
        {
            DataModel.Hotbar[position] = this.name;
            position = position >= 5 ? 1 : position + 1;
        }
    }
    public abstract void Use();
}