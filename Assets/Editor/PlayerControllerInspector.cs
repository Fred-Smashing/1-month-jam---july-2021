using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
[CanEditMultipleObjects]
public class PlayerControllerInspector : Editor
{
    public override void OnInspectorGUI() {
        base.DrawDefaultInspector();

        PlayerController controller = (PlayerController)target;

        if (GUILayout.Button("Generate Mesh")) {
            controller.ExternalUpdateMesh();
        }
    }
}
