using DDF.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

[CustomEditor(typeof(InventoryGrid))]
public class GridDrawer : Editor {

    private int width;
    private int height;

    private Vector2 cellSize;
    private Vector2 cellSpace;

    private Vector2 offset;

    InventoryGrid grid;

	public override void OnInspectorGUI() {
        DrawDefaultInspector();

        grid = target as InventoryGrid;

        Refresh();
        
        if (GUILayout.Button("Создать - обновить сетку")) {
            grid.ConstructGrid();
        }
        if (GUILayout.Button("Удалить сетку")) {
            grid.DisposeGrid();
        }
    }

    public void Refresh() {
        if (grid.width != width || grid.height != height || grid.cellSize != cellSize || grid.cellSpace != cellSpace || grid.offset != offset) {
            width = grid.width;
            height = grid.height;

            cellSize = grid.cellSize;
            cellSpace = grid.cellSpace;

            offset = grid.offset;

            grid.ConstructGrid();
        }
    }

    public void OnSceneGUI() {
    }
}
