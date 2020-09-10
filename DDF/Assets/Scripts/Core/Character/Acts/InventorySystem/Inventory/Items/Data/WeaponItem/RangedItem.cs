using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "WeaponRangedItem", menuName = "DDF/Inventory/Items/WeaponRangedItem")]
    public class RangedItem : WeaponItem {
        public RangedType rangedType = RangedType.Bow;
    }
    public enum RangedType {
        Bow,
        CrossBow,
	}
}
