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
		}
		private void Start() {
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
		
		private void OptionOpen() {
			ItemType type = currentItem.GetItemType();
			if (type is PouchType) {
				string findId = ( type as PouchType ).inventoryReference;
				print(findId);
				List<Inventory> inventories = InventoryOverSeerGUI._instance.containers;
				Inventory finder = inventories.Find(x => x.inventoryID == findId);

				if(finder == null) {
					Debug.LogError("ERROR i cant find this id - " + findId);
					return;
				}
				CanvasGroup obj = finder.GetComponent<CanvasGroup>();

				HelpFunctions.CanvasGroupSeer.EnableGameObject(obj, true);
			} else {
				Debug.LogError("ERROR");
			}
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
				//case TagThrow t: return OptionThrow;
				case TagEquip t: return OptionEquip;
				//case TagTakeOff t: return OptionTakeOff;
				//case TagEat t: return OptionEat;
				case TagDrink t: return OptionDrink;

				default: return DefaultOption;
			}
		}
		private void OptionTake( Item item, Inventory inventory ) {
			CharacterEntity._instance.Take(item, inventory);
		}
		private void OptionThrow( Item item ) {
			//currentInventory.DeleteItem(item);
		}
		private void OptionEquip( Item item, Inventory inventory ) {
			CharacterEntity._instance.Equip(item, inventory);
		}
		private void OptionTakeOff( Item item ) {
			//InventoryOverSeerGUI._instance.mainInventory.AddItem(currentItem);
		}
		private void OptionEat( Item item ) {
			//CharacterEntity._instance.RestoreHealth()
			//InventoryOverSeerGUI._instance.mainInventory.AddItem(currentItem);
		}
		private void OptionDrink( Item item, Inventory inventory ) {
			CharacterEntity._instance.Drink(item, inventory);
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
		/// <summary>
		/// Даём айтему тэги по типу.(хлеб нужно есть, воду надо пить)
		/// </summary>
		/// <param name="item"></param>
		/// <param name="isGUI"></param>
		public void ItemTagSetup( Item item, bool isGUI = true ) {
			List<ItemTag> tags = item.tags;


			ItemType itemType = item.GetItemType();


			if (itemType is ConsumableType) {
				ConsumableType consumable = itemType as ConsumableType;
				if (consumable.conumable == Consumable.Food) {
					AssignTag<TagEat>(tags);
				}
				if (consumable.conumable == Consumable.Potion) {
					AssignTag<TagDrink>(tags);
				}
			}
			if (itemType is ArmorType || itemType is WeaponType) {
				AssignTag<TagEquip>(tags);
			}

			//общие
			if (isGUI) {
				FreeTag<TagTake>(tags);
				AssignTag<TagThrow>(tags);
				if (itemType is ArmorType || itemType is WeaponType) {
					item.primaryTag = GetTag<TagEquip>(tags);
				}
				if (itemType is ConsumableType) {
					ConsumableType consumable = itemType as ConsumableType;
					if (consumable.conumable == Consumable.Food) {
						item.primaryTag = GetTag<TagEat>(tags);
					}
					if (consumable.conumable == Consumable.Potion) {
						item.primaryTag = GetTag<TagDrink>(tags);
					}
				}
			} else {
				AssignTag<TagTake>(tags);
				FreeTag<TagThrow>(tags);

				item.primaryTag = GetTag<TagTake>(tags);
			}


			tags.Sort();
		}
		private void AssignTag<T>( List<ItemTag> tags ) {
			bool result = tags.OfType<T>().Any();
			if (result == false) {
				tags.Add(ItemsTags._instance.GetTag<T>());
			}
		}

		private void FreeTag<T>( List<ItemTag> tags ) {
			bool result = tags.OfType<T>().Any();
			if (result == true) {
				tags.Remove(GetTag<T>(tags));
			}
		}
		private ItemTag GetTag<T>( List<ItemTag> tags ) {
			return tags[tags.FindIndex(x => x is T)];

		}
		#endregion
	}
}