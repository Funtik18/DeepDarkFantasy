using UnityEngine;

namespace DDF.Inventory.Items {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/WeaponRangedType")]
    public class RangedType : WeaponType {
        public Ranged ranged = Ranged.Bow;
    }

    public enum Ranged {
        Bow,
        Crossbow,
    }
}