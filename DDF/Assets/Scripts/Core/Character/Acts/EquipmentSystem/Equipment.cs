using DDF.Character;
using DDF.Character.Stats;
using DDF.Environment;
using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {
	/// <summary>
	/// Обмундирование какой-то сущности.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class Equipment : MonoBehaviour {

		List<EquipmentModel> models;

		[HideInInspector] public StatRegularFloat lHandDamage = new StatRegularFloat("Урон о.р.", 0, 0, "-");
		[HideInInspector] public StatRegularFloat rHandDamage = new StatRegularFloat("Урон c.р.", 0, 0, "-");

		[HideInInspector] public StatFloat armorHead = new StatFloat("Броня голова", 0);
		[HideInInspector] public StatFloat armorTorso = new StatFloat("Броня торс", 0);
		[HideInInspector] public StatFloat armorBelt = new StatFloat("Броня пояс", 0);
		[HideInInspector] public StatFloat armorLegs = new StatFloat("Броня портки", 0);
		[HideInInspector] public StatFloat armorFeet = new StatFloat("Броня ноги", 0);

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

		private HumanoidEntity currentEntity;
		private void Awake() {

			currentEntity = transform.root.GetComponent<HumanoidEntity>();

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
		}

		public Item Equip(Item item, Inventory from ) {
			for (int i = 0; i < allSlots.Count; i++) {
				if( CompareTypesEquip(allSlots[i], item)) {
					Item clone;
					if (from == null)//значит айтем взят из физ мира.
						clone = allSlots[i].AddItem(item, false);
					else
						clone = allSlots[i].AddItem(item, from.isGUI);
					if (clone != null) return clone;
				}
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
			Item clone = item.GetItemCopy<Item>();
			inventory.DeleteItem(item);
			return clone;
		}

		private bool CompareTypesEquip( Inventory inventory, Item item ) {
			if (inventory.IsEmpty) {
				for (int i = 0; i < inventory.storageTypes.Count; i++) {
					if (item.CompareType(inventory.storageTypes[i].ToString())) {
						return true;
					}
				}
			}
			return false;
		}
		private Item CompareTypesTakeoff( Inventory inventory, Item item ) {
			if (!inventory.IsEmpty) {
				if (inventory.currentItems.Contains(item)) {
					Item clone = item.GetItemCopy<Item>();
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
			if (item is ArmorItem armorItem) {
				#region uistats
				if (headEquipment == inventory) {
					armorHead.amount = armorItem.armor.amount;
				}
				if (chestEquipment == inventory) {
					armorTorso.amount = armorItem.armor.amount;
				}
				if (beltEquipment == inventory) {
					armorBelt.amount = armorItem.armor.amount;
				}
				if (legEquipment == inventory) {
					armorLegs.amount = armorItem.armor.amount;
				}
				if (feetEquipment == inventory) {
					armorFeet.amount = armorItem.armor.amount;
				}
				#endregion
				currentEntity.CurrentPhysicalArmor += armorItem.armor.amount;
			}else if (item is WeaponItem weaponItem) {
				#region uistats
				if (lHandEquipment == inventory) {
					lHandDamage.amount = weaponItem.damage.max;
					lHandDamage.currentInamount = weaponItem.damage.min;
				}
				if (rHandEquipment == inventory) {
					rHandDamage.amount = weaponItem.damage.max;
					rHandDamage.currentInamount = weaponItem.damage.min;
				}
				#endregion
				if (item is RangedItem) {
					currentEntity.MaxShotDamage += weaponItem.damage.max;
					currentEntity.MinShotDamage += weaponItem.damage.min;
				} else {
					currentEntity.MaxMeleeDamage += weaponItem.damage.max;
					currentEntity.MinMeleeDamage += weaponItem.damage.min;
				}
				
			}
		}
		/// <summary>
		/// Обмундирование убирает "эффекты" с сущности.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="inventory"></param>
		private void ItemRemoved(Item item, Inventory inventory) {
			if (item is ArmorItem armorItem) {
				#region uistats
				if (headEquipment == inventory) {
					armorHead.amount = 0;
				}
				if (chestEquipment == inventory) {
					armorTorso.amount = 0;
				}
				if (beltEquipment == inventory) {
					armorBelt.amount = 0;
				}
				if (legEquipment == inventory) {
					armorLegs.amount = 0;
				}
				if (feetEquipment == inventory) {
					armorFeet.amount = 0;
				}
				#endregion
				currentEntity.CurrentPhysicalArmor -= armorItem.armor.amount;
			}else if (item is WeaponItem weaponItem) {
				#region uistats
				if (lHandEquipment == inventory) {
					lHandDamage.amount = 0;
					lHandDamage.currentInamount = 0;
				}
				if (rHandEquipment == inventory) {
					rHandDamage.amount = 0;
					rHandDamage.currentInamount = 0;
				}
				#endregion
				if (item is RangedItem) {
					currentEntity.MaxShotDamage -= weaponItem.damage.max;
					currentEntity.MinShotDamage -= weaponItem.damage.min;
				} else {
					currentEntity.MaxMeleeDamage -= weaponItem.damage.max;
					currentEntity.MinMeleeDamage -= weaponItem.damage.min;
				}
			}
		}


		private class EquipmentModel {
			public GameObject phisycalSlot;
			public Item3DWeaponModel physicalModel;
		}
	}
}