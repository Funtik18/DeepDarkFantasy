using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/WeaponRangedType")]
    public class RangedType : WeaponType {
        public Ranged ranged = Ranged.Bow;
    }

    public enum Ranged {
        Bow,
        Crossbow,
    }
}