using UnityEngine;

namespace DDF.Inventory.Items {
    [System.Serializable]
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/Item", order = 1)]
    public class ItemSize{
        [Range(1,10)]
        public int width = 1;
        [Range(1, 10)]
        public int height = 1;

        private Vector2Int vector2;

        public void SetSize() {

		}

        public Vector2Int GetSize() {
            if(vector2 == null) {
                vector2 = new Vector2Int(width, height);
            }
            return vector2;
		}
    }
}
