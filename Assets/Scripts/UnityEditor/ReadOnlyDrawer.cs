using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Disable the GUI to make the field non-editable
        GUI.enabled = false;

        // Draw the property using the standard method
        EditorGUI.PropertyField(position, property, label, true);

        // Re-enable the GUI for other fields
        GUI.enabled = true;
    }
}