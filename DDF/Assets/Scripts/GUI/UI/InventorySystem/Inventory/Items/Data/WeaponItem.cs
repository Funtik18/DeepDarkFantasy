﻿using System.Collections;
using System.Collections.Generic;
using DDF.Character.Effects;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "WeaponItem", menuName = "DDF/Inventory/Items/WeaponItem", order = 1)]
    public class WeaponItem : Item {
        [Header("Stats")]
        public int stat;
        [Header("Effects")]
        public List<Effect> effects;
    }

    public enum WeaponType {
        OneHandedType,
        RangedType,
        TwoHandedType,
    }
}