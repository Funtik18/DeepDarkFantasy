using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "ArmorWristItem", menuName = "DDF/Inventory/Items/ArmorWristItem")]
    public class WristItem : ArmorItem {
        public WristType wristType = WristType.Bracer;
    }
    public enum WristType {
        Bracer,
	}
}