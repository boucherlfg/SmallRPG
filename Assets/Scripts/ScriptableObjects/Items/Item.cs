using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string visibleName;
    public Sprite sprite;
    public string description;
    public float value;

    public virtual void Equip()
    {
        UIManager.Notifications.CreateNotification("you are now using " + visibleName);
        DataModel.Equipment.Tool = this;
    }
    public virtual void Unequip()
    {
        UIManager.Notifications.CreateNotification("you are now empty handed");
        DataModel.Equipment.Tool = null;
    }
    public abstract void Use();
}