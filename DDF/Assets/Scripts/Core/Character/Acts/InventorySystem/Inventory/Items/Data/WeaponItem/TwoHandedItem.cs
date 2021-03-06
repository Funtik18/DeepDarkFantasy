﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "WeaponTwoHandedItem", menuName = "DDF/Inventory/Items/WeaponTwoHandedItem", order = 1)]
    public class TwoHandedItem : WeaponItem {
        public TwoHandedType twoHandedType = TwoHandedType.Sword;
    }
    public enum TwoHandedType {
        Axe,
        Mace,
        Polearm,
        Sword,
        Staff,
    }
}