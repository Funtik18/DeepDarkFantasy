using DDF.UI.Inventory;
using UnityEngine;

namespace DDF.Editor {
	using System;
	using UnityEditor;
    [CustomEditor(typeof(InventoryGrid))]
    public class GridDrawer : Editor {
        /// <summary>
        /// при изменении родительского объекта, якоря должны быть 0,5 0,5.
        /// а изменяемы объект  0 0
        ///                     1 1
        /// </summary>
        private int width;
        private int height;

        private Vector2 cellSize;
        private Vector2 cellSpace;

        private Vector2 offset;

        InventoryGrid grid;
        RectTransform parent;
        Vector2 anchorMinParent;
        Vector2 anchorMaxParent;

        bool triger = true;

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            grid = ( target as InventoryGrid );
            parent = grid.transform.parent.GetComponent<RectTransform>();

            triger = GUILayout.Toggle(triger, "Resize parent");

			try {
                Refresh();
            } catch (InvalidOperationException prefabException) {
                Debug.LogError("No Panic-" + prefabException.Message);
			}

            if (GUILayout.Button("Создать - обновить сетку")) {
                Refresh();
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

				if (triger) {
                    parent.sizeDelta = new Vector2((cellSize.x * width) + offset.x*2 + cellSpace.x*(width-1), (cellSize.y * height) - offset.y*2 - cellSpace.y*(height-1));
				}
            }
        }

        public void OnSceneGUI() {
        }
    }
}