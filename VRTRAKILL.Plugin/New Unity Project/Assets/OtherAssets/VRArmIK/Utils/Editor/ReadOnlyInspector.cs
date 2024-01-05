using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DisplayOnlyAttribute))]
public class DisplayOnlyInspector : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		string valueStr;

		switch (prop.propertyType)
		{
			case SerializedPropertyType.Integer:
				valueStr = prop.intValue.ToString();
				break;
			case SerializedPropertyType.Boolean:
				valueStr = prop.boolValue.ToString();
				break;
			case SerializedPropertyType.Float:
				valueStr = prop.floatValue.ToString("0.00000");
				break;
			case SerializedPropertyType.String:
				valueStr = prop.stringValue;
				break;
			case SerializedPropertyType.Vector2:
				valueStr = prop.vector2Value.ToString();
				break;
			case SerializedPropertyType.Vector3:
				valueStr = prop.vector3Value.ToString();
				break;
			default:
				valueStr = "(not supported)";
				break;
		}

		EditorGUI.LabelField(position, label.text, valueStr);
	}
}