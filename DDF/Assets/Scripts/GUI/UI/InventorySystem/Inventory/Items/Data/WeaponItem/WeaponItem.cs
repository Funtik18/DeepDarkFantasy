using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using DDF.Character.Stats;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class WeaponItem : Item {
        [Tooltip("Урон")]
        public VarMinMaxFloat damage = new VarMinMaxFloat("Damage", 0, 10);
        [Tooltip("Прочность")]
        public VarMinMaxInt duration = new VarMinMaxInt("Duration", 100, 100);
        [Header("Effects")]
        public List<Effect> primeryEffects;
        public List<Effect> secondaryEffects;
    }
}