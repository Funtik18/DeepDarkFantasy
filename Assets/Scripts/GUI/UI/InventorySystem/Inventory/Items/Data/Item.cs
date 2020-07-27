using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace DDF.Inventory {

    [DisallowMultipleComponent]
    [AddComponentMenu("DDF/Inventory/Item", 1)]
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/Item", order = 1)]
    [Serializable]
    public class Item : ScriptableObject, IComparable {

        [Header("System information")]

        [SerializeField]
        private string itemID = System.Guid.NewGuid().ToString();
        [Tooltip("Будет ли объект копироваться или будет один экземляр.")]
        [SerializeField]
        private bool onlyOne = false;


        [Header("Information")]
        [SerializeField]
        public string itemName;

        [SerializeField]
        [TextArea]
        public string description;

        [SerializeField]
        [TextArea]
        public string anotation;

        [Tooltip("Если не принадлежит к какому то типу, тогда Item Useless")]
        public List<ItemType> types = new List<ItemType>();


        [Header("Graphics")]
        [Header("Icon")]
        //[SerializeField] public IconAssetType IconType = IconAssetType.Sprite;

        [SerializeField] public Sprite icon;

        [SerializeField]
        public Vector3 iconOrientation = Vector3.zero;


        [Header("Stats")]
        public Vector2Int size = Vector2Int.one;

        //public Color Highlight = Color.clear;

        [Header("Stacking")]
        [Tooltip("Если -1 то предмет 'бесконечный'")]
        public int MaxStack = 1;
        public uint StackCount = 1;


		public Item GetItem(bool copy = false) {
            if(!copy)
            itemID = System.Guid.NewGuid().ToString();
            if (onlyOne) return this;
            return Instantiate(this);
		}

        public string GetId() {
            return itemID;
		}

        /// <summary>
        /// /
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public int CompareTo( object obj ) {
            Item compareItem = (Item)obj;

            int parts = 0;

            if(itemName == compareItem.itemName) {
                parts++;
            }

            if(types.Count == compareItem.types.Count) {//проверка на типы
                int count = 0;
                for (int i = 0; i < types.Count; i++) {
                    if (types[i] == compareItem.types[i]) {
                        count++;
                    }
                }
                if (count == types.Count) {
                    parts++;
                }
            }
            


			if (parts == 2) {
                return 1;
			}

            return -1;
		}
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
