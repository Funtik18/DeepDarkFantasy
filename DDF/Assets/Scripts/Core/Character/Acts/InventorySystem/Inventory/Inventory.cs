using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
	public class Inventory : MonoBehaviour {

		[HideInInspector] public string inventoryID;

		public bool isDisposer = false;
		public bool isGUI = false;

		[HideInInspector] public InventoryTypes inventorytype = InventoryTypes.Simple;
		[HideInInspector] public bool isFull;
		[HideInInspector]
		public bool IsEmpty {
			get {
				return currentItems.Count == 0;
			}
		}

		/// <summary>
		/// Фильтр. Если размер 0 то принимает любой предмет.
		/// </summary>
		public List<StorageTypes> storageTypes;
		public string InventoryName = "Inventory";
		[HideInInspector] public InventoryOverSeer overSeer;
		public InventoryView view;
		public InventoryContainer container;
		public List<Item> currentItems;

		/// <summary>
		/// Событие. Происходит когда какой-то айтем добавился в какой-то контейнер.
		/// </summary>
		public UnityAction<Item, Inventory> onItemAdded;
		public UnityAction<Item, Inventory> onItemRemoved;

		private CanvasGroup canvasGroup;

		[HideInInspector] public ToolTip toolTip;

		public void CreateNewID() {
			inventoryID = System.Guid.NewGuid().ToString();
		}
		protected virtual void Awake() {
			canvasGroup = GetComponent<CanvasGroup>();
			if (inventoryID == "")
				CreateNewID();
			if (isGUI)
				overSeer = InventoryOverSeerGUI.GetInstance();
			else
				overSeer = InventoryOverSeerUI.GetInstance();
			toolTip = ToolTip.GetInstance();
		}

		protected void Start() {

			container.Init();
		}

		public virtual Item AddItem(Item item, bool enableModel = true) {
			if (item == null) { Debug.LogError("Error 404"); return null; }
			Item clone = item.GetItemCopy<Item>();
			return container.AddItem(clone, enableModel);
		}
		public void DeleteItem(Item item) {
			container.DeleteItem(item);
			container.DeleteModel(item);
		}

		public void ShowInventory() {
			Help.HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup, true);
			container.ShowContainer();
		}
		public void HideInventory() {
			container.HideContainer();
			Help.HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);
		}
	}
	public enum InventoryTypes {
		Simple,
		TrashCan,
		Equipment,
		Storage,
	}
	public enum StorageTypes {
		HeadItem,
		TorsoItem,
		WaistItem,
		LegsItem,
		FeetItem,
		WristItem,
		JewerlyItem,

		OffHandItem,
		OneHandedItem,
		RangedItem,
		TwoHandedItem,

		FoodItem,
		PotionItem,
	}
}