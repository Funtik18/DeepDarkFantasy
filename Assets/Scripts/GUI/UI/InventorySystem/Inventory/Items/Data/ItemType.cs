using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Inventory {
    [System.Serializable]
    public class ItemType : ScriptableObject {

	}

    public class ItemUseless : ItemType { }
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/Weapon", order = 1)]
    public class ItemConsumable : ItemType { }
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/Weapon", order = 1)]
    public class ItemWeapon : ItemType {
        public Weapon weapon = Weapon.Glaive;
    }
    public class ItemShields : ItemType { }
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/Armor", order = 1)]
    public class ItemArmor : ItemType { }
    public class ItemAccessories : ItemType { }
    public class ItemAmmo : ItemType { }
    public class ItemTomesAndLicenses : ItemType { }
    public class ItemKeyItem : ItemType { }
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/Money", order = 1)]
    public class ItemMoney : ItemType { }

    public enum Weapon {
        Glaive = 0,
        Katana = 1,
        Greathammer = 2,
    }
}