using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string visibleName;
    public string description;
    public float value;

    public virtual void Equip()
    {
        NotifManager.CreateNotification("you are now using " + visibleName);
        PlayerData.Equipment.Tool = this;
    }
    public virtual void Unequip()
    {
        NotifManager.CreateNotification("you are now empty handed");
        PlayerData.Equipment.Tool = null;
    }
    public abstract void Use();
}