using UnityEngine;

namespace DDF.Inventory.Items {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/WeaponOneHandedType")]
    public class OneHandedType : WeaponType {
        public OneHanded oneHanded = OneHanded.Sword;
    }

    public enum OneHanded {
        Axe,
        Dagger,
        Maces,
        Spear,
        Sword,
        Flail,
    }
}
