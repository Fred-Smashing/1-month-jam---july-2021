using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyController))]
[CanEditMultipleObjects]
public class EnemyControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        EnemyController controller = (EnemyController)target;

        if (GUILayout.Button("Generate Mesh"))
        {
            controller.ExternalUpdateMesh();
        }
    }
}