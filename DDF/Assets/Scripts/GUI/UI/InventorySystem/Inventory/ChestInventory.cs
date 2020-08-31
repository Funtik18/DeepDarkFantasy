using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.UI.Inventory {
    public class ChestInventory : Inventory {
        public Button buttonClose;
        public Button buttonTakeAll;

        protected override void Awake() {
            base.Awake();
            inventorytype = InventoryTypes.Storage;
        }
    }
}