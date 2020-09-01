using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class ArmorItem : Item {
        [Header("Stats")]
        public int stat;
        [Header("Effects")]
        public List<Effect> effects;
    }

    public enum Armor {
        WaistType,
        SholderType,
        JewelryType,
        HandType,
        WristType,
    }
}
