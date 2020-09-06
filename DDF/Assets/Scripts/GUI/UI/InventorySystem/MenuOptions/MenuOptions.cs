using DDF.Character;
using DDF.Character.Stats;
using DDF.Help;
using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.UI.Inventory {

	[RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
	[RequireComponent(typeof(ItemsTags))]
	public class MenuOptions : MonoBehaviour {

        public static MenuOptions _instance;

		private CanvasGroup canvasGroup;

		public RectTransform rect { get { return GetComponent<RectTransform>(); } }

		public GameObject optionPrefab;
		public List<MenuOption> options;

		private bool isHide = true;
		public bool IsHide { get { return isHide; } }

		private Item currentItem;
		private Inventory currentInventory;

		private void Awake() {
			_instance = this;
			ItemsTags.Init();
			canvasGroup = GetComponent<CanvasGroup>();
			options = new List<MenuOption>();
		}

		public void AddNewOption(string optionName, UnityAction call) {
			GameObject obj = HelpFunctions.TransformSeer.CreateObjectInParent(transform, optionPrefab);
			obj.name = optionName + "-option-" + options.Count;

			Transform objTrans = obj.transform;

			objTrans.localPosition = new Vector3(objTrans.localPosition.x, -((objTrans as RectTransform).sizeDelta.y * options.Count));


			MenuOption option = obj.GetComponent<MenuOption>();
			option.Option = optionName;
			option.SetAction(call);

			options.Add(option);
		}
		
		private void OptionDivision() {
			uint itemCount = currentItem.itemStackCount;
			int itemCountSize = currentItem.itemStackSize;

			ItemDivision division = ItemDivision._instance;

			division.SetCurrentItem(currentItem);
			division.SetPosition(transform.position);
			division.OpenDivision();
		}

		public void SetPosition(Vector3 position ) {
			transform.position = position;
		}

		public void SetCurrentItem( Item item, Inventory from ) {
			currentItem = item;
			currentInventory = from;
		}

		public UnityAction<Item, Inventory> DetermineAction(ItemTag tag) {
			switch (tag) {
				case TagTake t: return OptionTake;
				case TagThrow t: return OptionThrow;
				case TagEquip t: return OptionEquip;
				case TagTakeOff t: return OptionTakeOff;
				case TagEat t: return OptionEat;
				case TagDrink t: return OptionDrink;

				default: return DefaultOption;
			}
		}
		private void OptionTake( Item item, Inventory inventory ) {
			CharacterEntity._instance.Take(item, inventory);
		}
		private void OptionThrow( Item item, Inventory inventory ) {
			//CharacterEntity._instance.UEquip(item, inventory);
		}
		private void OptionEquip( Item item, Inventory inventory ) {
			CharacterEntity._instance.Equip(item, inventory);
		}
		private void OptionTakeOff( Item item, Inventory inventory ) {
			CharacterEntity._instance.TakeOff(item, inventory);
		}
		private void OptionEat( Item item, Inventory inventory ) {
			//CharacterEntity._instance.RestoreHealth()
			//InventoryOverSeerGUI._instance.mainInventory.AddItem(currentItem);
		}
		private void OptionDrink( Item item, Inventory inventory ) {
			CharacterEntity._instance.Drink(item as ConsumableItem, inventory);
		}

		private void DefaultOption(Item item, Inventory inventory) {
			Debug.LogError("default");
		}


		public void OpenMenu() {

			HelpFunctions.TransformSeer.DestroyChildrenInParent(transform);
			options.Clear();

			List<ItemTag> tags = currentItem.tags;
			for (int i = 0; i < tags.Count; i++) {
				UnityAction<Item, Inventory> call = DetermineAction(tags[i]);

				AddNewOption(tags[i].tagName, delegate { call?.Invoke(currentItem, currentInventory); CloseMenu(); });
			}

			HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup, true);

			isHide = false;
		}
		public void CloseMenu() {
			HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);

			isHide = true;
		}


		#region Tags work
		private Item curItem = null;
		private List<ItemTag> curTags = null;
		/// <summary>
		/// Даём айтему тэги по типу.(хлеб нужно есть, воду надо пить)
		/// </summary>
		/// <param name="item"></param>
		/// <param name="isGUI"></param>
		public void ItemTagSetup( Item item, InventoryTypes pressets ) {
			curItem = item;
			curTags = item.tags;
			curTags.Clear();

			CheckConsumableType();
			CheckArmorType();

			AssignPrimaryTag();

			//общие
			if (pressets == InventoryTypes.Equipment) {
				FreeTag<TagEquip>(curTags);
				AssignTag<TagTakeOff>(curTags);
				curItem.primaryTag = GetTag<TagTakeOff>(curTags);
			}
			if (pressets == InventoryTypes.Storage) {
				AssignTag<TagTake>(curTags);
				curItem.primaryTag = GetTag<TagTake>(curTags);
			}
			

			curTags.Sort();

			curItem = null;
			curTags = null;
		}
		private void CheckConsumableType() {
			if (curItem is ConsumableItem item) {
				if (item is FoodItem) {
					AssignTag<TagEat>(curTags);
				}
				if (item is PotionItem) {
					AssignTag<TagDrink>(curTags);
				}
			}
		}
		private void CheckArmorType() {
			if (curItem is ArmorItem || curItem is WeaponItem) {
				AssignTag<TagEquip>(curTags);
			}
		}
		private void AssignPrimaryTag() {
			if (curItem is ConsumableItem item) {
				if (item is FoodItem) {
					curItem.primaryTag = GetTag<TagEat>(curTags);
				}
				if (item is PotionItem) {
					curItem.primaryTag = GetTag<TagDrink>(curTags);
				}
			}
			if (curItem is ArmorItem || curItem is WeaponItem) {
				curItem.primaryTag = GetTag<TagEquip>(curTags);
			}
		}
		public void AssignTag<T>( List<ItemTag> tags ) {
			bool result = tags.OfType<T>().Any();
			if (result == false) {
				tags.Add(ItemsTags._instance.GetTag<T>());
			}
		}

		public void FreeTag<T>( List<ItemTag> tags ) {
			bool result = tags.OfType<T>().Any();
			if (result == true) {
				tags.Remove(GetTag<T>(tags));
			}
		}
		public ItemTag GetTag<T>( List<ItemTag> tags ) {
			return tags[tags.FindIndex(x => x is T)];
		}


		public enum TagPressets {
			ForInventory,
			ForEquipment,
			ForChest,
		}
		#endregion
	}
}