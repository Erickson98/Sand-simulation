using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Plane))]
public class PlaneEditor : Editor
{
    private SerializedProperty property;
    private Plane plane;

    private void OnEnable()
    {
        plane = (Plane)target;
        property = serializedObject.GetIterator();

        var meshFilter = plane.GetComponent<MeshFilter>();
        var mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            plane.UpdateMesh();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        property.Reset();
        while (property.NextVisible(true))
        {
            EditorGUILayout.PropertyField(property);
        }

        bool propertyChanged = serializedObject.ApplyModifiedProperties();
        bool forcedUpdate = GUILayout.Button("Update");

        if (propertyChanged || forcedUpdate)
        {
            plane.UpdateMesh();
        }
    }
}