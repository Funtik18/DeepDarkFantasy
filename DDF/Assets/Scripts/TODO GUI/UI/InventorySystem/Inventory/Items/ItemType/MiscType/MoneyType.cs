﻿using UnityEngine;

namespace DDF.Inventory.Items {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/MiscMoneyType")]
    public class MoneyType : ItemType {
        public Money money = Money.Gold;
    }
    public enum Money {
        Gold,
        Silver,
        Copper,
    }
}