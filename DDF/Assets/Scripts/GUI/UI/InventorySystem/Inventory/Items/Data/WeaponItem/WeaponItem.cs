using System.Collections.Generic;
using DDF.Character.Effects;
using DDF.Character.Variables;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class WeaponItem : Item {
        [Tooltip("Урон")]
        public VarMinMax<VarFloat> armor = new VarMinMax<VarFloat>("Armor", new VarFloat("Min", 0), new VarFloat("Max", 10));
        [Tooltip("Прочность")]
        public VarMinMax<VarInt> duration = new VarMinMax<VarInt>("Duration", new VarInt("Min", 100), new VarInt("Max", 100));
        [Header("Effects")]
        public List<Effect> primeryEffects;
        public List<Effect> secondaryEffects;
    }
}