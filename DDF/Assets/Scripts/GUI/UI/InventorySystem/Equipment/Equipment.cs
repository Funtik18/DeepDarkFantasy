using DDF.UI.Inventory.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
	public class Equipment : MonoBehaviour {
		[SerializeField]
		private Inventory headEquipment;
		[SerializeField]
		private Inventory chestEquipment;
		[SerializeField]
		private Inventory lHandEquipment;
		[SerializeField]
		private Inventory rHandEquipment;
		[SerializeField]
		private Inventory lBracletEquipment;
		[SerializeField]
		private Inventory rBracletEquipment;
		[SerializeField]
		private Inventory legEquipment;
		[SerializeField]
		private Inventory feetEquipment;

		[SerializeField]
		private Inventory lRing0Equipment;
		[SerializeField]
		private Inventory lRing1Equipment;
		[SerializeField]
		private Inventory lRing2Equipment;
		[SerializeField]
		private Inventory lRing3Equipment;
		[SerializeField]
		private Inventory lRing4Equipment;

		[SerializeField]
		private Inventory rRing0Equipment;
		[SerializeField]
		private Inventory rRing1Equipment;
		[SerializeField]
		private Inventory rRing2Equipment;
		[SerializeField]
		private Inventory rRing3Equipment;
		[SerializeField]
		private Inventory rRing4Equipment;

		private List<Inventory> rings;

		private void Awake() {
			rings = new List<Inventory>();
			rings.Add(lRing0Equipment);
			rings.Add(lRing1Equipment);
			rings.Add(lRing2Equipment);
			rings.Add(lRing3Equipment);
			rings.Add(lRing4Equipment);
			rings.Add(rRing0Equipment);
			rings.Add(rRing1Equipment);
			rings.Add(rRing2Equipment);
			rings.Add(rRing3Equipment);
			rings.Add(rRing4Equipment);
		}

		public bool Equip(Item item ) {
			if (CompareTypes(headEquipment, item)) return true;
			if (CompareTypes(chestEquipment, item)) return true;
			if (CompareTypes(lHandEquipment, item)) return true;
			if (CompareTypes(rHandEquipment, item)) return true;
			if (CompareTypes(lBracletEquipment, item)) return true;
			if (CompareTypes(rBracletEquipment, item)) return true;
			if (CompareTypes(legEquipment, item)) return true;
			if (CompareTypes(feetEquipment, item)) return true;

			for(int i = 0; i < rings.Count; i++) {
				if (CompareTypes(rings[i], item)) return true;
			}
			return false;
		}
		private bool CompareTypes( Inventory inventory, Item item ) {
			if (inventory.IsEmpty) {
				for (int i = 0; i < inventory.storageTypes.Count; i++) {
					if (item.CompareItemType(inventory.storageTypes[i]) == 1) {
						inventory.AddItem(item);
						return true;
					}
				}
			}
			return false;
		}
	}
}