using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LabelOverride))]
public class ThisPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var propertyAttribute = attribute as LabelOverride;
		label.text = propertyAttribute?.label;
		EditorGUI.PropertyField(position, property, label);
	}
}