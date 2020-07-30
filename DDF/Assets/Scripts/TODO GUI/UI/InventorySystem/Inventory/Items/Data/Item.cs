#if UNITY_EDITOR
using DDF.Atributes;
#endif
using UnityEngine;

namespace DDF.Inventory.Items {
    [AddComponentMenu("DDF/Inventory/Item", 1)]
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/Item", order = 1)]
    public class Item : ScriptableObject {
        [Header("Information")]
        public string itemName;

        [TextArea]
        public string itemDescription;

        [TextArea]
        public string itemAnotation;
        
        [Header("Icon")]
        public Sprite itemIcon;

        [SerializeField]
        public Vector3 itemIconOrientation = Vector3.zero;
        public Vector3 IconOrientation {
            get { return itemIconOrientation; }
            set {
                itemIconOrientation = value;
            }
        }
        [Header("Stacking")]
        public Vector2Int itemSize = Vector2Int.one;
        public Vector2Int itemStackCount = Vector2Int.one;

        [Header("Stats")]


        [InfoBox("Если itemType == null тогда тип объекта равен UselessType.", InfoBoxType.Normal)]
        [Header("Misc")]
        public ItemType itemType;

        [SerializeField]
        private string itemID = System.Guid.NewGuid().ToString();

        [SerializeField]
        [InfoBox("Будет ли объект копироваться или будет один экземляр.", InfoBoxType.Normal)]
        private bool onlyOne = false;

        /// <summary>
        /// Если copy == false то возвращает другой объект-копию.
        /// </summary>
        /// <param name="copy"></param>
        /// <returns></returns>
        public Item GetItem(bool copy = false) {
            if (onlyOne) return this;
            if (!copy)
            itemID = System.Guid.NewGuid().ToString();
            return Instantiate(this);
		}

        public string GetId() {
            return itemID;
		}
    }
}
