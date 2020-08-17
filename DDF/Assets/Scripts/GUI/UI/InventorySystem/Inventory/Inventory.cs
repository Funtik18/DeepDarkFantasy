using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
    public class Inventory : MonoBehaviour {

		[HideInInspector] public string inventoryID;

		[Tooltip("Если тру то в этом контейнере возможно положить только один предиет размером с контейнер.")]
		public bool isRestrictions = false;
		public bool isGUI = true;

		[HideInInspector] public bool isFull;
		[HideInInspector] public bool IsEmpty {
			get {
				return currentItems.Count == 0;
			}
		}

		public List<ItemType> storageTypes;
		public string InventoryName = "Inventory";
		[HideInInspector]public InventoryOverSeer overSeer;
		public InventoryView view;
        public InventoryContainer container;
		public List<Item> currentItems;

		[HideInInspector]private CanvasGroup canvasGroup;

		public void CreateNewID() {
			inventoryID = System.Guid.NewGuid().ToString();
		}
		protected void Awake() {
			canvasGroup = GetComponent<CanvasGroup>();
			if (inventoryID == "")
				CreateNewID();
			if(isGUI)
				overSeer = InventoryOverSeerGUI._instance;
			else
				overSeer = InventoryOverSeerUI._instance;
		}

		protected void Start() {
			container.Init();
		}

		public int AddItem(Item item, bool enableModel = true) {
			int result = container.AddItem(item, enableModel);
			return result;
		}
		public void AddItems() {

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
}