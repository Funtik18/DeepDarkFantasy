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
        [Range(1,10)]
        [SerializeField]
        private int itemWidth = 1;
        [Range(1, 10)]
        [SerializeField]
        private int itemHeight = 1;

        [Range(1, 64)]
        public uint itemStackCount = 1;
        [Tooltip("Если itemStackSize == -1 тогда возможное максимальное количество, максимально.")]
        [Range(-1, 10)]
        public int itemStackSize = 1;//дефолтное любое, кроме нуля

        [Header("Stats")]


        [InfoBox("Если itemType == null тогда тип объекта равен UselessType.", InfoBoxType.Normal)]
        [Header("Misc")]
        public ItemType itemType;

        [SerializeField]
        private string itemID = System.Guid.NewGuid().ToString();

        [InfoBox("Будет ли объект копироваться или будет один экземляр.", InfoBoxType.Normal)]
        [SerializeField]
        private bool onlyOne = false;

        /// <summary>
        /// Если copy == false то возвращает другой объект-копию.
        /// </summary>
        /// <param name="copy"></param>
        /// <returns></returns>
        public Item GetItem(bool copy = false) {
            if (onlyOne) return this;

            Item clone = Instantiate(this);
            if (!copy)
                clone.itemID = System.Guid.NewGuid().ToString();
            return clone;
		}

        [SerializeField][HideInInspector]
        private Vector2 vector2;
        /// <summary>
        /// Возвращает размер в предмета в инвентаре.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetSize() {
            if(vector2 == null || vector2.x == 0 || vector2.y == 0) {
                vector2 = new Vector2(itemWidth, itemHeight);
			}

            return vector2;
		}

        public string GetId() {
            return itemID;
		}

    }
}
