using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "ArmorJewerlyItem", menuName = "DDF/Inventory/Items/ArmorJewerlyItem")]
    public class JewerlyItem : ArmorItem {
        public JewerlyType jewerlyType = JewerlyType.Ring;
    }
    public enum JewerlyType {
        Amulet,
        Ring,
	}
}
