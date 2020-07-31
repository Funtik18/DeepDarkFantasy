using UnityEngine;

namespace DDF.Inventory.Items {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/WeaponTwoHandedType")]
    public class TwoHandedType : WeaponType {
        public TwoHanded twoHanded = TwoHanded.Sword;
    }

    public enum TwoHanded {
        Axe,
        Maces,
        Polearms,
        Stave,
        Sword,
    }
}