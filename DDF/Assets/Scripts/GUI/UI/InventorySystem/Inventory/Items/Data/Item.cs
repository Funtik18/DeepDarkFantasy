using DDF.Atributes;
using System;
using Unity.Collections;
using UnityEngine;


namespace DDF.Inventory {

    [DisallowMultipleComponent]
    [AddComponentMenu("DDF/Inventory/Item", 1)]
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/Item", order = 1)]
    public class Item : ScriptableObject {
        [Header("Information")]
        public string name;

        [Multiline]
        public string description;

        [Multiline]
        public string anotation;


        [Header("Icon")]
        //public IconAssetType IconType = IconAssetType.Sprite;

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

        [Header("Stacking")]
        [InfoBox("Если y == -1 тогда возможное максимальное количество, максимально.", InfoBoxType.Warning)]
        [InfoBox("x - текущее количество\ny - максимальное количество.", InfoBoxType.Normal)]
        public Vector2Int StackCount = new Vector2Int(1, 3);

        [Header("Misc")]
        public ItemType itemType;


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

    public class ItemType : ScriptableObject {}
    #region Other
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/UselessItem", order = 1)]
    public class UselessItem : ItemType { }
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/ConsumableItem", order = 1)]
    public class ConsumableItem : ItemType {
        public Conumable conumable = Conumable.Potion;
    }
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/MoneyItem", order = 1)]
    public class MoneyItem : ItemType {
        public Money money = Money.Gold;
    }

    public class CraftingItem : ItemType {
        public Crafting crafting = Crafting.CraftingMaterial;
    }

    public enum Conumable {
        Potion,
    }
    public enum Money {
        Gold,
        Silver,
        Copper,
	}
    
    public enum Crafting {
        CraftingMaterial,
        BlacksmithPlan,
        JewelerDesign,
        PageOfTraining,
        Dye,
        Gem,
    }

    #endregion
    #region Weapons
    public class WeaponItem : ItemType { }

    public class OneHandedItem : WeaponItem { 
        public OneHanded oneHanded = OneHanded.Sword;
    }
    public class TwoHandedItem : WeaponItem {
        public TwoHanded twoHanded = TwoHanded.Sword;
    }
    public class RangedItem : WeaponItem {
        public Ranged ranged = Ranged.Bow;
    }

    public enum OneHanded {
        Axe,
        Dagger,
        Maces,
        Spear,
        Sword,
        Flail,
    }
    public enum TwoHanded {
        Axe,
        Maces,
        Polearms,
        Stave,
        Sword,
    }
    public enum Ranged {
        Bow,
        Crossbow,
    }


    #endregion
    #region Armor
    public class HeadItem : ItemType {
        public Head head = Head.Helm;
    }
    public class SholderItem : ItemType { }
    public class TorsoItem : ItemType {
        public Torso torso = Torso.ChestArmor;
    }
    public class WristItem : ItemType { }
    public class HandItem : ItemType { }
    public class WaistItem : ItemType { }
    public class LegItem : ItemType { }
    public class FeetItem : ItemType { }
    public class JewelryItem : ItemType { }

    public class FollowerSpecial : ItemType {

	}

    public enum Head {
        Helm,
    }
    public enum Torso {
        ChestArmor,
	}
    /*Belt,
        BodyArmor,
        Boot,
        Glove,
    Circlet,
        Shield,
        Amulet,
        Ring,*/
    #endregion


}
