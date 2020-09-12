using System.Collections.Generic;
using DDF.Atributes;
using DDF.Character;
using DDF.UI.Inventory.Items;
using UnityEngine;

namespace DDF.Environment {
    public class Item3DModel : MonoBehaviour {
        public Entity owner;
        public Item item;

		protected virtual void Awake() {
			if (owner == null) {
				owner = transform.root.GetComponent<Entity>();
			}
			if (item == null) {
				Debug.LogError("ERROR: 404 item");
			}
		}
	}
}