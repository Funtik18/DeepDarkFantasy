using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using DDF.Character.Stats;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class ArmorItem : Item {
        [Tooltip("Броня")]
        public VarMinMaxFloat armor;
        [Tooltip("Прочность")]
        public VarMinMaxInt duration;
        [Header("Effects")]
        public List<Effect> primaryEffects;
        public List<Effect> secondaryEffects;

        protected override void OnEnable() {
            base.OnEnable();
            armor = new VarMinMaxFloat("Armor", 0, 10);
            duration = new VarMinMaxInt("Duration", 100, 100);
        }
	}
}
