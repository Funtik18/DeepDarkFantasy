using System;
using UnityEngine;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
    public class Inventory : MonoBehaviour {

		public string inventoryID;

		public string InventoryName = "Inventory";
        public InventoryView view;
        public InventoryContainer container;

		private void Awake() {
			inventoryID = System.Guid.NewGuid().ToString();
		}

		private void Start() {
			

			container.Init();
		}
	}
}