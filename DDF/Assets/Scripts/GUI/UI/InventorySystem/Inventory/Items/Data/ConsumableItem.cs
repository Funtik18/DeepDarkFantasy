using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "ConsumableItem", menuName = "DDF/Inventory/Items/ConsumableItem", order = 1)]
    public class ConsumableItem : Item {
        public Consumable consumable = Consumable.Potion;
    }
    public enum Consumable {
        Potion,
        Food,
    }
}