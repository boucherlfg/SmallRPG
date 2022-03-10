using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(AgentData))]
[CanEditMultipleObjects]
public class AgentDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var obj = target as AgentData;
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(obj.visibleName)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(obj.agentType)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(obj.tile)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(obj.value)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(obj.statblock)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(obj.certainLoot)));
        var useCodexProp = serializedObject.FindProperty(nameof(obj.useCodexForPossibleLoot));
        EditorGUILayout.PropertyField(useCodexProp);
        if (!useCodexProp.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(obj.possibleLoot)));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

[CreateAssetMenu(menuName = "Felix/Agent")]
public class AgentData : ScriptableObject
{
    public enum AgentType
    {
        Bandit = 0,
        Carnivore = 1,
        Herbivore = 2,
        None = -1
    }
    public string visibleName;
    public AgentType agentType;
    public StatBlock statblock;
    public Tile tile;
    public float value = 3;
    public List<Item> certainLoot;
    
    

    public bool useCodexForPossibleLoot = true;
    public List<Item> possibleLoot;
}