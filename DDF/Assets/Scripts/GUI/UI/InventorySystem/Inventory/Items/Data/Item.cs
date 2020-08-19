﻿#if UNITY_EDITOR
using DDF.Atributes;
using DDF.Events;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
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
        public int stat;

        [Header("Events")]
        
        public ItemEvents events;


        [InfoBox("Если itemType == null тогда тип объекта равен UselessType.", InfoBoxType.Normal)]
        [Header("Misc")]
        [SerializeField]
        private ItemType itemType;
        public ItemTag primaryTag { get { return itemType.primaryTag; } set { itemType.primaryTag = value; } }
        public List<ItemTag> tags { get { return itemType.tags; } }

        [SerializeField]
        private string itemID = System.Guid.NewGuid().ToString();

        [InfoBox("Будет ли объект копироваться или будет один экземляр.", InfoBoxType.Normal)]
        [SerializeField]
        private bool onlyOne = false;

        //Get

        /// <summary>
        /// Если copy == false то возвращает другой объект-копию.
        /// </summary>
        /// <param name="copy"></param>
        /// <returns></returns>
        public Item GetItemCopy() {
            if (onlyOne) return this;

            Item clone = Instantiate(this);
            clone.itemType = Instantiate(itemType);
            clone.itemType.name = itemType.name;
            clone.itemID = System.Guid.NewGuid().ToString();
            return clone;
		}

        [NonSerialized]
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

        public ItemType GetItemType() {
            return itemType;
        }

        public string GetId() {
            return itemID;
		}

        //Compare

        public int CompareItem(Item item) {

            if (CompareItemType(item.itemType) == 1) {
                return 1;
            }
            return 0;
		}
        public int CompareItemType(ItemType itemTypeCompare) {
            if (itemTypeCompare.ToString() == itemType.ToString()) {
                return 1;
            }
            return 0;
        }
    }

    [Serializable]
    public class ItemEvents {
        public List<ItemIntEvent> intEvents;
        public List<ItemFloatEvent> floatEvents;
        public List<ItemBoolEvent> boolEvents;
        public List<ItemVoidEvent> voidEvents;

        public void EventsExecute() {
            ListEventExecute(intEvents);
            ListEventExecute(floatEvents);
            ListEventExecute(boolEvents);
            ListEventExecute(voidEvents);
        }
        private void ListEventExecute<T>(List<T> events ) where T : ItemEvent {
            for (int i = 0; i < events.Count; i++) {
                events[i].Execute();
            }
        }
    }
    public abstract class ItemEvent {
        public abstract void Execute();
    }
    [Serializable]
    public class ItemIntEvent : ItemEvent {
        public IntEvent intEvent;
        public int value;

        public override void Execute() {
            intEvent?.Raise(value);
        }
    }
    [Serializable]
    public class ItemFloatEvent : ItemEvent {
        public FloatEvent floatEvent;
        public float value;
        public override void Execute() {
            floatEvent?.Raise(value);
        }
    }
    [Serializable]
    public class ItemBoolEvent : ItemEvent {
        public BoolEvent boolEvent;
        public bool value;
        public override void Execute() {
            boolEvent?.Raise(value);
        }
    }
    [Serializable]
    public class ItemVoidEvent : ItemEvent {
        public VoidEvent voidEvent;
        public override void Execute() {
            voidEvent?.Raise();
        }
    }
}
