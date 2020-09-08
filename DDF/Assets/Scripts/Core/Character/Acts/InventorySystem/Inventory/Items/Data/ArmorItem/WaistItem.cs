using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "ArmorWaistItem", menuName = "DDF/Inventory/Items/ArmorWaistItem")]
    public class WaistItem : ArmorItem {
        public WaistType waistType = WaistType.Belt;
    }
    public enum WaistType {
        Belt,
	}
}
