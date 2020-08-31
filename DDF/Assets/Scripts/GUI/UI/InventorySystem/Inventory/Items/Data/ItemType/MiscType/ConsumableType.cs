using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "MiscConsumableType", menuName = "DDF/Inventory/ItemType/MiscConsumableType")]
    public class ConsumableType : ItemType {
        public Consumable consumable = Consumable.Potion;
        
        
    }

    public enum Consumable {
        Potion,
        Food,
    }

    
}