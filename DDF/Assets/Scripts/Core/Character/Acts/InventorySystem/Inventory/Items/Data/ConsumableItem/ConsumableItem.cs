using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class ConsumableItem : Item {
        [Header("Effects")]
        public List<Effect> effects;
    }
}