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

		private List<Inventory> allSlots;

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

			allSlots = new List<Inventory>();
			allSlots.Add(headEquipment);
			allSlots.Add(chestEquipment);
			allSlots.Add(lHandEquipment);
			allSlots.Add(rHandEquipment);
			allSlots.Add(lBracletEquipment);
			allSlots.Add(rBracletEquipment);
			allSlots.Add(legEquipment);
			allSlots.Add(feetEquipment);
			allSlots.AddRange(rings);

			for(int i = 0; i < allSlots.Count; i++) {
				allSlots[i].inventorytype = InventoryTypes.Equipment;
			}
		}

		public Item Equip(Item item ) {
			for (int i = 0; i < allSlots.Count; i++) {
				Item clone = CompareTypesEquip(allSlots[i], item);
				if (clone != null) return clone;
			}
			return null;
		}
		public Item TakeOff( Item item ) {
			for(int i = 0; i < allSlots.Count; i++) {
				Item clone = CompareTypesTakeoff(allSlots[i], item);
				if (clone != null) return clone;
			}
			return null;
		}

		private Item CompareTypesEquip( Inventory inventory, Item item ) {
			if (inventory.IsEmpty) {
				for (int i = 0; i < inventory.storageTypes.Count; i++) {
					if (item.Equals(inventory.storageTypes[i].ToString())) {
						Item addeditem = inventory.AddItem(item);
						return addeditem;
					}
				}
			}
			return null;
		}
		private Item CompareTypesTakeoff( Inventory inventory, Item item ) {
			if (!inventory.IsEmpty) {
				if (inventory.currentItems.Contains(item)) {
					Item clone = item.GetItemCopy();
					inventory.DeleteItem(item);
					return clone;
				}
			}
			return null;
		}
	}
}