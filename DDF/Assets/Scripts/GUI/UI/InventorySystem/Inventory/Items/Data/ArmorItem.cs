using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "ArmorItem", menuName = "DDF/Inventory/Items/ArmorItem", order = 1)]
    public class ArmorItem : Item {
        [Header("Type")]
        public Armor itemType = Armor.HeadType; 
        [Header("Stats")]
        public int stat;
        [Header("Effects")]
        public List<Effect> effects;
    }

    public enum Armor {
        HeadType,
        TorsoType,
        LegType,
        FeetType,
        WaistType,
        SholderType,
        JewelryType,
        HandType,
        WristType,
    }
}
