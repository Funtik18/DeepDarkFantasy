using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class ItemType : ScriptableObject {
        [Tooltip("Все возможные события с предметом.")]
        public List<ItemTag> tags = new List<ItemTag>();
    }
}