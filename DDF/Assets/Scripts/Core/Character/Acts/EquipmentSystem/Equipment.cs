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

		public Transform placeLHand, placeRHand;

		[HideInInspector] public StatRegularFloat lHandDamage = new StatRegularFloat("Урон о.р.", 0, 0, "-");
		[HideInInspector] public StatRegularFloat rHandDamage = new StatRegularFloat("Урон c.р.", 0, 0, "-");

		[HideInInspector] public StatFloat armorHead = new StatFloat("Броня голова", 0);
		[HideInInspector] public StatFloat armorTorso = new StatFloat("Броня торс", 0);
		[HideInInspector] public StatFloat armorBelt = new StatFloat("Броня пояс", 0);
		[HideInInspector] public StatFloat armorLegs = new StatFloat("Броня портки", 0);
		[HideInInspector] public StatFloat armorFeet = new StatFloat("Броня ноги", 0);

		[SerializeField]
		public Inventory headEquipment;
		[SerializeField]
		public Inventory chestEquipment;
		[SerializeField]
		public Inventory beltEquipment;
		[SerializeField]
		public Inventory lHandEquipment;
		[SerializeField]
		public Inventory rHandEquipment;
		[SerializeField]
		public Inventory lBracletEquipment;
		[SerializeField]
		public Inventory rBracletEquipment;
		[SerializeField]
		public Inventory legsEquipment;
		[SerializeField]
		public Inventory feetEquipment;

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


		private List<Inventory> allSlots;
		private List<Inventory> weaponSlots;
		private List<Inventory> armorSlots;
		private List<Inventory> rings;

		private HumanoidEntity currentEntity;
		private void Awake() {

			currentEntity = transform.root.GetComponent<HumanoidEntity>();

			rings = new List<Inventory>(10);
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

			armorSlots = new List<Inventory>(17);
			armorSlots.Add(headEquipment);
			armorSlots.Add(chestEquipment);
			armorSlots.Add(beltEquipment);
			armorSlots.Add(lBracletEquipment);
			armorSlots.Add(rBracletEquipment);
			armorSlots.Add(legsEquipment);
			armorSlots.Add(feetEquipment);
			armorSlots.AddRange(rings);

			weaponSlots = new List<Inventory>(2);
			weaponSlots.Add(lHandEquipment);
			weaponSlots.Add(rHandEquipment);

			allSlots = new List<Inventory>(19);
			allSlots.AddRange(weaponSlots);
			allSlots.AddRange(armorSlots);

			for(int i = 0; i < allSlots.Count; i++) {
				allSlots[i].onItemAdded = ItemAdded;
				allSlots[i].onItemRemoved = ItemRemoved;
			}
		}

		public Item Equip(Item item, Inventory from ) {

			bool enableUI = from == null ? false : from.isGUI;

			Inventory cashSlot;

			if (item is WeaponItem weaponItem) {
				if (weaponItem is OneHandedItem) {
					if (lHandEquipment.IsEmpty) {
						cashSlot = lHandEquipment;
					} else if (rHandEquipment.IsEmpty) {
						if (lHandEquipment.currentItems[0] is TwoHandedItem) return null;
						cashSlot = rHandEquipment;
					} else {
						return null;
					}

					return cashSlot.AddItem(item, enableUI);
				}
				if (weaponItem is TwoHandedItem) {
					if (lHandEquipment.IsEmpty && rHandEquipment.IsEmpty) {
						cashSlot = lHandEquipment;
					} else {
						return null;
					}

					return cashSlot.AddItem(item, enableUI);
				}
				if (weaponItem is RangedItem) {
					//
				}
			} else if (item is ArmorItem) {
				for (int i = 0; i < armorSlots.Count; i++) {
					if (CompareTypesEquip(armorSlots[i], item)) {
						Item clone = armorSlots[i].AddItem(item, enableUI);
						if (clone != null)
							return clone;
					}
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
				if (legsEquipment == inventory) {
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

			AttachToEquipment(inventory, item);
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
				if (legsEquipment == inventory) {
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

			DisposeEquipment(inventory);
		}

		/// <summary>
		/// Привязывает 3д модель к персонажу.
		/// </summary>
		private void AttachToEquipment(Inventory inventory, Item item) {
			if (inventory == rHandEquipment) {
				Item3DModel Rweapon = Instantiate(item.item3DModel, placeRHand.position, placeRHand.rotation).GetComponent<Item3DModel>();
				Rweapon.transform.parent = placeRHand;
				Rweapon.itemRigidbody.isKinematic = true;
			}
			if (inventory == lHandEquipment) {
				Item3DModel Lweapon = Instantiate(item.item3DModel, placeLHand.position, placeLHand.rotation).GetComponent<Item3DModel>();
				Lweapon.transform.parent = placeLHand;
				Lweapon.itemRigidbody.isKinematic = true;
			}
		}
		/// <summary>
		/// Очищает место где должна находиться 3д модель.
		/// </summary>
		private void DisposeEquipment(Inventory inventory) {
			if (inventory == rHandEquipment) {
				Help.HelpFunctions.TransformSeer.DestroyChildrenInParent(placeRHand);
			}
			if (inventory == lHandEquipment) {
				Help.HelpFunctions.TransformSeer.DestroyChildrenInParent(placeLHand);
			}
		}
	}
}