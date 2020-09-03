using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using DDF.Character.Stats;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class ArmorItem : Item {
        [Tooltip("Броня")]
        public VarMinMaxFloat armor = new VarMinMaxFloat("Armor", 0, 10);
        [Tooltip("Прочность")]
        public VarMinMaxInt duration = new VarMinMaxInt("Duration", 100, 100);
        [Header("Effects")]
        public List<Effect> primaryEffects;
        public List<Effect> secondaryEffects;
	}
}
