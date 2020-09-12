using System.Collections.Generic;
using DDF.Character.Effects;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class WeaponItem : Item {
        [Tooltip("Урон")]
        public VarMinMax<float> damage = new VarMinMax<float>("Damage", 0, 10);
        [Tooltip("Прочность")]
        public VarMinMax<int> duration = new VarMinMax<int>("Duration", 100, 100);
        [Header("Effects")]
        public List<Effect> primaryEffects;
        public List<Effect> secondaryEffects;
    }
}