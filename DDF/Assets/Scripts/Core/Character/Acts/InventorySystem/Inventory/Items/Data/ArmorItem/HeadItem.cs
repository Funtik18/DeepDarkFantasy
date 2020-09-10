using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "ArmorHeadItem", menuName = "DDF/Inventory/Items/ArmorHeadItem")]
    public class HeadItem : ArmorItem {
        public HeadType headType = HeadType.Helm;
    }
    public enum HeadType {
        Helm,
	}
}