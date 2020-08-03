using DDF.UI.Inventory;
using UnityEngine;

namespace DDF.Editor {
    using UnityEditor;
    [CustomEditor(typeof(InventoryGrid))]
    public class GridDrawer : Editor {

        private int width;
        private int height;

        private Vector2 cellSize;
        private Vector2 cellSpace;

        private Vector2 offset;

        InventoryGrid grid;
        RectTransform parent;

        bool triger = true;

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            grid = ( target as InventoryGrid );
            parent = grid.transform.parent.GetComponent<RectTransform>();

            triger = GUILayout.Toggle(triger, "Resize parent");


            Refresh();

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