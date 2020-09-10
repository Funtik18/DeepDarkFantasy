using System;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    
    [Serializable]
	public class ItemTag : ScriptableObject, IComparable {
        public string tagName;

		public int CompareTo( object obj ) {
            ItemTag tag = obj as ItemTag;
            if(tag.GetType() == GetType()) {
                return 1;
			}
            return 0;
        }

		public ItemTag GetCopy() {
            return Instantiate(this);
        }
    }
}