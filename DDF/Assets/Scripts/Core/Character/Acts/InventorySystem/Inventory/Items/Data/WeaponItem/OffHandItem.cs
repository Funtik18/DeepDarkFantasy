
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "WeaponOffHandItem", menuName = "DDF/Inventory/Items/WeaponOffHandItem")]
    public class OffHandItem : ArmorItem{
        /// <summary>
        /// TODO: block chance
        /// </summary>
        public OffHandType offHandType = OffHandType.Shield;
    }
    public enum OffHandType {
        Shield,
	}
}
