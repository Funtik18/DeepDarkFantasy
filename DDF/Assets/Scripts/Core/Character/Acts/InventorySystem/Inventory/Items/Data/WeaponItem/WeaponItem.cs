using System.Collections.Generic;
using DDF.Character.Effects;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class WeaponItem : Item {
        [Tooltip("Урон")]
        public VarMinMax<float> damage;
        [Tooltip("Прочность")]
        public VarMinMax<int> duration;
        [Header("Effects")]
        public List<Effect> primaryEffects;
        public List<Effect> secondaryEffects;

        private void OnEnable() {
            damage = new VarMinMax<float>("Damage", 0, 10);
            duration = new VarMinMax<int>("Duration", 100, 100);
        }
    }
}