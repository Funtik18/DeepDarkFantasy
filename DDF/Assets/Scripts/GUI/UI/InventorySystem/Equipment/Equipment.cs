using DDF.Character;
using DDF.UI.Inventory.Items;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {
	/// <summary>
	/// Обмундирование какой-то сущности.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class Equipment : MonoBehaviour {

		[HideInInspector] public Entity currentEntity;

		[HideInInspector]
		public VarFloat armorHead;
		[HideInInspector]
		public VarFloat armorTorso;
		[HideInInspector]
		public VarFloat armorLegs;

		[SerializeField]
		private Inventory headEquipment;
		[SerializeField]
		private Inventory chestEquipment;
		[SerializeField]
		private Inventory beltEquipment;
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
			allSlots.Add(beltEquipment);
			allSlots.Add(lHandEquipment);
			allSlots.Add(rHandEquipment);
			allSlots.Add(lBracletEquipment);
			allSlots.Add(rBracletEquipment);
			allSlots.Add(legEquipment);
			allSlots.Add(feetEquipment);
			allSlots.AddRange(rings);

			for(int i = 0; i < allSlots.Count; i++) {
				allSlots[i].inventorytype = InventoryTypes.Equipment;
				allSlots[i].onItemAdded = ItemAdded;
				allSlots[i].onItemRemoved = ItemRemoved;
			}

			armorHead = new VarFloat("Броня головы", 0);
			armorTorso = new VarFloat("Броня торса", 0);
			armorLegs = new VarFloat("Броня ног", 0);
		}

		public Item Equip(Item item ) {
			for (int i = 0; i < allSlots.Count; i++) {
				Item clone = CompareTypesEquip(allSlots[i], item);
				if (clone != null) return clone;
			}
			return null;
		}
		public Item TakeOff( Item item ) {
			for (int i = 0; i < allSlots.Count; i++) {
				Item clone = CompareTypesTakeoff(allSlots[i], item);
				if (clone != null) {
					allSlots[i].DeleteItem(item);
					return clone;
				}
			}
			return null;
		}
		public Item TakeOff(Item item, Inventory inventory) {
			Item clone = item.GetItemCopy();
			inventory.DeleteItem(item);
			return clone;
		}

		private Item CompareTypesEquip( Inventory inventory, Item item ) {
			if (inventory.IsEmpty) {
				for (int i = 0; i < inventory.storageTypes.Count; i++) {
					if (item.CompareType(inventory.storageTypes[i].ToString())) {
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
					return clone;
				}
			}
			return null;
		}

		/// <summary>
		/// Какой-то айтем добавлен, нужно уточнить что довлено и куда.
		/// Обмундирование накладывает "эффекты" на сущность.
		/// </summary>
		private void ItemAdded(Item item, Inventory inventory) {
			IncreaceStats(item);
		}
		/// <summary>
		/// Обмундирование убирает "эффекты" с сущности.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="inventory"></param>
		private void ItemRemoved(Item item, Inventory inventory) {
			DecreaceStats(item);
		}
		private void IncreaceStats(Item item) {
			if (item is ArmorItem armorItem) {
				currentEntity.CurrentPhysicalArmor += armorItem.armor.amount;
				return;		
			}
			if (item is WeaponItem weaponItem) {
				if (item is RangedItem) {
					currentEntity.MaxShotDamage += weaponItem.damage.max;
					currentEntity.MinShotDamage += weaponItem.damage.min;
					return;
				}
				currentEntity.MaxMeleeDamage += weaponItem.damage.max;
				currentEntity.MinMeleeDamage += weaponItem.damage.min;
				return;
			}
		}
		private void DecreaceStats(Item item) {
			if (item is ArmorItem armorItem) {
				currentEntity.CurrentPhysicalArmor -= armorItem.armor.amount;
			}
			if (item is WeaponItem weaponItem) {
				if(item is RangedItem) {
					currentEntity.MaxShotDamage -= weaponItem.damage.max;
					currentEntity.MinShotDamage -= weaponItem.damage.min;
					return;
				}
				currentEntity.MaxMeleeDamage -= weaponItem.damage.max;
				currentEntity.MinMeleeDamage -= weaponItem.damage.min;
				return;
			}
		}
	}
}