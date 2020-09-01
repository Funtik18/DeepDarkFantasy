using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    public class ItemType : ScriptableObject {
        [Tooltip("Главное событие")]
        [HideInInspector] public ItemTag primaryTag;
        [Tooltip("Все возможные события с предметом.")]
        [HideInInspector] public List<ItemTag> tags;

        public override bool Equals( object obj ) {
            if (obj is ItemType objectType) {
                return this.GetType() == objectType.GetType();
            }
            return false;
        }
    }
}