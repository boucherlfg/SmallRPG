using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Item), editorForChildClasses:true), CanEditMultipleObjects]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var obj = target as Item;
        DrawDefaultInspector();
        obj.hasDurability = EditorGUILayout.Toggle("Has Durability", obj.hasDurability);
        if (obj.hasDurability)
        {
            obj.durability = EditorGUILayout.IntField("Durability", obj.durability);
        }
    }
}
#endif


[InventoryCategory]
public abstract class Item : ScriptableObject
{
    [HideInInspector]
    public bool hasDurability;
    [HideInInspector]
    public int durability;

    public string visibleName;
    public Sprite sprite;
    public string description;
    public float value;

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