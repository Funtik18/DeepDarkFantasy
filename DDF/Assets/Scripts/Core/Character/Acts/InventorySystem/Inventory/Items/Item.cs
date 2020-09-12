using DDF.Atributes;
using DDF.Environment;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class Item : ScriptableObject {
        [InfoBox("Будет ли объект копироваться или будет один экземляр.", InfoBoxType.Normal)]
        [Header("System")]
        [SerializeField]
        private bool onlyOne = false;
        [ReadOnly]
        [SerializeField]
        private string itemID = System.Guid.NewGuid().ToString();

        [Header("Information")]
        public string itemName;

        [TextArea]
        public string itemDescription;

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

        public Item3DModel item3DModel;

        [Header("Stacking")]
        [Range(1, 10)]
        [SerializeField]
        public int itemWidth = 1;
        [Range(1, 10)]
        [SerializeField]
        public int itemHeight = 1;

        [Range(1, 64)]
        public uint itemStackCount = 1;
        [Tooltip("Если itemStackSize == -1 тогда возможное максимальное количество, максимально.")]
        [Range(-1, 10)]
        public int itemStackSize = 1;//дефолтное любое, кроме нуля

        [Tooltip("Главное событие")]
        [HideInInspector] public ItemTag primaryTag;
        [Tooltip("Все возможные события с предметом.")]
        [HideInInspector] public List<ItemTag> tags;

        [NonSerialized]
        private Vector2 vector2;
        /// <summary>
        /// Возвращает размер в предмета в инвентаре.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetSize() {
            if (vector2 == null || vector2.x == 0 || vector2.y == 0) {
                vector2 = new Vector2(itemWidth, itemHeight);
            }

            return vector2;
        }


        public string GetId() {
            return itemID;
        }

        public bool CompareItem(object obj) {
            if (obj is Item obj1) {
                return GetId() == obj1.GetId();
            }
            return false;
        }
        public bool CompareType(object obj) {
            if (obj is Item objectType1) {
                return this.GetType() == objectType1.GetType();
            }
            if (obj is string objectType2) {
                return this.GetType().Name == objectType2;
            }
            return false;
        }
        /// <summary>
        /// Если copy == false то возвращает другой объект-копию.
        /// </summary>
        /// <param name="copy"></param>
        /// <returns></returns>
        public T GetItemCopy<T>() where T : Item{
            if (onlyOne) return this as T;

            Item clone = Instantiate(this);
            clone.itemID = System.Guid.NewGuid().ToString();
            return clone as T;
        }
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => base.ToString();

        [Header("Stats")]
        public ItemRarity rarity = ItemRarity.Common;
        [Tooltip("Вес")]
        public VarFloat weight = new VarFloat("Вес", 1f);
    }

    public enum ItemRarity {
        Common,
        Rare,
        Epic,
        Legendary,
        Set,
    }
}
