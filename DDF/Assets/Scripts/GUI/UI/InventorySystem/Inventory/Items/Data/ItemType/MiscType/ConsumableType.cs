﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "MiscConsumableType", menuName = "DDF/Inventory/ItemType/MiscConsumableType")]
    public class ConsumableType : ItemType {
        public Consumable conumable = Consumable.Potion;
        public List<Effect> effects;
        [Serializable]
        public struct Effect {
            public string effect;
            public float power;
        }
    }

    public enum Consumable {
        Potion,
        Food,
    }

    
}