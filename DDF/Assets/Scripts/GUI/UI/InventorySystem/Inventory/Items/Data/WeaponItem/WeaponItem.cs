using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using DDF.Character.Stats;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class WeaponItem : Item {
        public StatRegularFloat damage;
        [Header("Effects")]
        public List<Effect> effects;

        public void OnValidate() {
            damage = new StatRegularFloat("Damage", 0, 10);
        }
    }
}