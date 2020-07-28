using System;
using Unity.Collections;
using UnityEngine;

namespace DDF.Inventory {

    [DisallowMultipleComponent]
    [AddComponentMenu("DDF/Inventory/Item", 1)]
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/Item", order = 1)]
    [Serializable]
    public class Item : ScriptableObject {
        [Header("Information")]
        [SerializeField]
        public string name;

        [SerializeField]
        [Multiline]
        public string description;

        [SerializeField]
        [Multiline]
        public string anotation;


        [Header("Icon")]
        [SerializeField]
        //public IconAssetType IconType = IconAssetType.Sprite;

        [Tooltip("The sprite that represents this item in an inventory view.")]
        //[SerializeField]
        public Sprite icon;

        [SerializeField]
        public Vector3 _IconOrientation = Vector3.zero;
        public Vector3 IconOrientation {
            get { return _IconOrientation; }
            set {
                _IconOrientation = value;
            }
        }


        [Header("Stats")]
        public Vector2Int size = Vector2Int.one;



        [Tooltip("The color to display in a slot's highlight when this item is stored.")]
        public Color Highlight = Color.clear;


        [Header("Stacking")]
        [Tooltip("The maximum number of items sharing the same StackID that can be stacked on top of each other in a single location.")]
        public uint MaxStack = 1;
        [Tooltip("The current stack size for this item. There is only one real instance of the item and all stacked instances are destroyed and cause this number to increment.")]
        public uint StackCount = 1;

        [Header("Misc")]
        [Tooltip("A special identifier used to determine.")]
        public ItemType type = ItemType.UselessItem;

        //public

        [Tooltip("A special identifier used to determine.")]
        [SerializeField]
        private string itemID = System.Guid.NewGuid().ToString();
        [Tooltip("Будет ли объект копироваться или будет один экземляр.")]
        [SerializeField]
        private bool onlyOne = false;

        public Item GetItem(bool copy = false) {
            if(!copy)
            itemID = System.Guid.NewGuid().ToString();
            if (onlyOne) return this;
            return Instantiate(this);
		}

        public string GetId() {
            return itemID;
		}
	}

    public enum ItemType : int {
        UselessItem = -1,
        Consumable = 0,
        Weapon = 1,
        Shields = 2,
        Armor = 3,
        Accessories = 4,
        Ammo = 5,
        TomesAndLicenses = 6,
        KeyItems = 7
    }
   /* static class ExItemTypes {
        public static String GetString( this ItemType itemType ) {
            switch (itemType) {
                case ItemType.UselessItem:
                return "Yeah!";
                case ItemType.Consumable:
                return "Okay!";
                case ItemType.Weapon:
                return "Yeah!";
                case ItemType.Shields:
                return "Okay!";
                case ItemType.Armor:
                return "Yeah!";
                case ItemType.Accessories:
                return "Okay!";
                case ItemType.Ammo:
                return "Yeah!";
                case ItemType.TomesAndLicenses:
                return "Okay!";
                case ItemType.KeyItems:
                return "Okay!";
                default:
                return "What?!";
            }

            return "";
        }
    }*/


    public enum Armor : int {
        Belt = 0,
        Boot = 1
	}
}
