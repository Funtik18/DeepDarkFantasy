using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using DDF.Character.Stats;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class WeaponItem : Item {
        [Tooltip("Урон")]
        public VarMinMaxFloat damage;
        [Tooltip("Прочность")]
        public VarMinMaxInt duration;
        [Header("Effects")]
        public List<Effect> primeryEffects;
        public List<Effect> secondaryEffects;

        protected override void OnEnable() {
            base.OnEnable();
            damage = new VarMinMaxFloat("Damage", 0, 10);
            duration = new VarMinMaxInt("Duration", 100, 100);
        }
    }
}