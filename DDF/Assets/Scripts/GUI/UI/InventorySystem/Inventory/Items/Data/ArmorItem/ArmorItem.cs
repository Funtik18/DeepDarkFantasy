using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using DDF.Character.Stats;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class ArmorItem : Item {
        public StatMinMaxFloat armor;
        [Header("Effects")]
        public List<Effect> primaryEffects;
        public List<Effect> secondaryEffects;

        private void OnEnable() {
            armor = new StatMinMaxFloat("Armor", 0, 10);
        }
	}

    public enum Armor {
        WaistType,
        SholderType,
        JewelryType,
        HandType,
        WristType,
    }
}
