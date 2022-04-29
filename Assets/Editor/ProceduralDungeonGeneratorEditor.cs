using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class ProceduralDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator generator;

    private void Awake() {
        this.generator = (AbstractDungeonGenerator)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon")) {
            generator.Generate();
        }
    }
}
