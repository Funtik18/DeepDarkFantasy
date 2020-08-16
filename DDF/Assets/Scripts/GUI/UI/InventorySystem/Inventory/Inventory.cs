﻿using DDF.UI.Inventory.Items;
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
		
		public List<ItemType> storageTypes;
		public string InventoryName = "Inventory";
		[HideInInspector]public InventoryOverSeer overSeer;
		public InventoryView view;
        public InventoryContainer container;
		public List<Item> currentItems;


		public void CreateNewID() {
			inventoryID = System.Guid.NewGuid().ToString();
		}
		private void Awake() {
			if (inventoryID == "")
				CreateNewID();
			if(isGUI)
				overSeer = InventoryOverSeerGUI._instance;
			else
				overSeer = InventoryOverSeerUI._instance;
		}

		private void OnEnable() {
		}

		private void Start() {
			container.Init();
		}

		public void ShowInventory() {
			container.ShowContainer();
		}
		public void HideInventory() {
			container.HideContainer();
		}
	}
}