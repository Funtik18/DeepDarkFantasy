using DDF.Atributes;
using UnityEngine;

namespace DDF.Inventory.Items {
    [System.Serializable]
    public class ItemStack {
        [Range(1, 64)]
        public uint current = 1;

        [InfoBox("Если max == -1 тогда возможное максимальное количество, максимально.", InfoBoxType.Warning)]
        [Range(-1, 10)]
        public int max = 1;//дефолтное любое, кроме нуля

        private Vector2Int vector2;

        public void SetSize() {

        }

        public Vector2Int GetSize() {
            if (vector2 == null) {
                vector2 = new Vector2Int((int)current, (int)max);
            }
            return vector2;
        }
    }
}