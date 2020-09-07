using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "ArmorLegsItem", menuName = "DDF/Inventory/Items/ArmorLegsItem")]
    public class LegsItem : ArmorItem {
        public LegsType legsType = LegsType.Pants;
    }
    public enum LegsType {
        Pants,
	}
}