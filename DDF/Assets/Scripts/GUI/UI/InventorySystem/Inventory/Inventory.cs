using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
    public class Inventory : MonoBehaviour {

		[HideInInspector] public string inventoryID;

		public bool isDisposer = false;
		public bool isGUI = false;

		[HideInInspector] public InventoryTypes inventorytype = InventoryTypes.Simple;


		[HideInInspector] public bool isFull;
		[HideInInspector] public bool IsEmpty {
			get {
				return currentItems.Count == 0;
			}
		}

		public List<StorageTypes> storageTypes;
		public string InventoryName = "Inventory";
		[HideInInspector]public InventoryOverSeer overSeer;
		public InventoryView view;
		public InventoryContainer container;
		public List<Item> currentItems;

		private CanvasGroup canvasGroup;

		public void CreateNewID() {
			inventoryID = System.Guid.NewGuid().ToString();
		}
		protected virtual void Awake() {
			canvasGroup = GetComponent<CanvasGroup>();
			if (inventoryID == "")
				CreateNewID();
			if (isGUI)
				overSeer = InventoryOverSeerGUI.Getinstance();
			else
				overSeer = InventoryOverSeerUI.Getinstance();
		}

		protected void Start() {
			print(overSeer == null);
			container.Init();
		}

		public virtual Item AddItem(Item item, bool enableModel = true) {
			Item clone = item.GetItemCopy();
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
		LegsItem,
		FeetItem,

		OneHandedItem,
		RangedItem,
		TwoHandedItem,
	}
}