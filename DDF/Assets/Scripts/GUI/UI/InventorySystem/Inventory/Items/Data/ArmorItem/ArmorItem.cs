using System.Collections.Generic;
using DDF.Character.Effects;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class ArmorItem : Item {
        [Tooltip("Броня")]
        public VarFloat armor = new VarFloat("Armor",  0f);
        [Tooltip("Прочность")]
        public VarMinMax<int> duration = new VarMinMax<int>("Duration",100, 100);
        [Header("Effects")]
        public List<Effect> primaryEffects;
        public List<Effect> secondaryEffects;
	}
}
