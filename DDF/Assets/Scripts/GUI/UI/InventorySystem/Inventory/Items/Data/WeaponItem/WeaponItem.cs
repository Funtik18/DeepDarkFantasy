using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using DDF.Character.Stats;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class WeaponItem : Item {
        public StatMinMaxFloat damage;
        [Header("Effects")]
        public List<Effect> primeryEffects;
        public List<Effect> secondaryEffects;

        private void OnEnable() {
            damage = new StatMinMaxFloat("Damage", 0, 10);
        }
    }
}