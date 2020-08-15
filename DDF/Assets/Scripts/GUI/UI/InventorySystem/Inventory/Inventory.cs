using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
    public class Inventory : MonoBehaviour {

		[HideInInspector] public string inventoryID;

		/// <summary>
		/// если тру то 3д если фалсе то 2д.
		/// </summary>
		[HideInInspector]
		public bool is3dOr2d = true;
		[Tooltip("Если тру то в этом контейнере возможно положить только один предиет размером с контейнер.")]
		public bool isRestrictions = false;
		public List<ItemType> storageTypes;
		public string InventoryName = "Inventory";
        public InventoryView view;
		[SerializeField]
        public InventoryContainer container;
		public List<Item> currentItems;


		public void CreateNewID() {
			inventoryID = System.Guid.NewGuid().ToString();
		}
		private void Awake() {
			if (inventoryID == "")
				CreateNewID();
		}

		private void Start() {
			container.Init();
		}
	}
}