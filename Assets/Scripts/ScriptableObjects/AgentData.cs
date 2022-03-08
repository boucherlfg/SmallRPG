using System.Collections.Generic;
using UnityEngine;
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
    public string visibleName;
    public StatBlock statblock;
    public List<Item> certainLoot;
    public float value = 3;

    public bool useCodexForPossibleLoot = true;
    public List<Item> possibleLoot;
}