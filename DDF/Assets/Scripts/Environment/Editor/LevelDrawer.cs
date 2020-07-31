using DDF.UI.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

[CustomEditor(typeof(LevelDesignManager))]
public class LevelDrawer : Editor { 

    LevelDesignManager design;

    Material floorMaterial;
    Material wallMaterial;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        design = target as LevelDesignManager;

        if(floorMaterial != design.floorMaterial || wallMaterial!=design.wallMaterial) {
            Refresh();
		}


		if (GUILayout.Button("Update")) {
            Refresh();
            floorMaterial = design.floorMaterial;
            wallMaterial = design.wallMaterial;
        }

    }

    public void Refresh() {
        design.RefreshRoom();

    }
}
