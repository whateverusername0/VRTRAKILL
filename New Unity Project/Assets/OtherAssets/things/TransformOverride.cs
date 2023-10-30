#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Transform))]
class TransformOverride : Editor
{
    public override void OnInspectorGUI()
    {
        Transform transform = (Transform)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Copy world Position"))
        {
            GUIUtility.systemCopyBuffer = $"Vector3{transform.position}";
        }
        if (GUILayout.Button("Copy World Euler Angles"))
        {
            GUIUtility.systemCopyBuffer = $"Vector3{transform.eulerAngles}";
        }
    }
}
#endif
