using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "MiscPouchType", menuName = "DDF/Inventory/ItemType/MiscPouchType")]
    public class PouchType : ItemType {
        public Inventory inventory;
        [HideInInspector]
        public string inventoryReference;
    }
}