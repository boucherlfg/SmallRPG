using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string visibleName;
    public Sprite sprite;
    public string description;
    public float value;

    public virtual void Equip()
    {
        AudioManager.PlayAsSound("equip");
        UIManager.Notifications.CreateNotification("you have just equiped " + visibleName);
        DataModel.Equipment.Tool = this;
    }
    public virtual void Unequip()
    {
        AudioManager.PlayAsSound("equip");
        UIManager.Notifications.CreateNotification("you are now empty handed");
        DataModel.Equipment.Tool = null;
    }
    public abstract void Use();
}