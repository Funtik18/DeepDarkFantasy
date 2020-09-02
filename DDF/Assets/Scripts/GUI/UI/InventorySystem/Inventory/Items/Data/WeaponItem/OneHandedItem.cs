using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "WeaponOneHandedItem", menuName = "DDF/Inventory/Items/WeaponOneHandedItem")]
    public class OneHandedItem : WeaponItem {
        public OneHandedType handedType = OneHandedType.Axe;
    }
    public enum OneHandedType {
        Axe,
        Dagger,
        Mace,
        Spear,
        Sword,
    }
}
