using System.Collections.Generic;
using DDF.Atributes;
using DDF.Character;
using DDF.UI.Inventory.Items;
using UnityEngine;

namespace DDF.Environment {
	[RequireComponent(typeof(Rigidbody))]
	public class Item3DModel : MonoBehaviour {
        public Entity owner;
        public Item item;

		[HideInInspector] public Rigidbody itemRigidbody;

		protected virtual void Awake() {
			itemRigidbody = GetComponent<Rigidbody>();
			if (item == null) {
				Debug.LogError("ERROR: 404 item");
			}
		}
		protected virtual void Start() {
			if (owner == null) {
				owner = transform.root.GetComponent<Entity>();
			}
		}
	}
}