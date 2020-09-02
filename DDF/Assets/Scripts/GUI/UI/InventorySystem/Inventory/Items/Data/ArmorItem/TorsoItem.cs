using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "ArmorTorsoItem", menuName = "DDF/Inventory/Items/ArmorTorsoItem")]
    public class TorsoItem : ArmorItem {
        public TorsoType torsoType = TorsoType.ChestArmor;
    }
    public enum TorsoType {
        ChestArmor,
    }
}