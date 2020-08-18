using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class ItemType : ScriptableObject {
        [Tooltip("Главное событие")]
        [HideInInspector] public ItemTag primaryTag;
        [Tooltip("Все возможные события с предметом.")]
        [HideInInspector] public List<ItemTag> tags;
    }
}