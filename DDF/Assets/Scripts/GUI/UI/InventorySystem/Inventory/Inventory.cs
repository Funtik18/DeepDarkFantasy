using System;
using UnityEngine;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
    public class Inventory : MonoBehaviour {

		public string inventoryID = System.Guid.NewGuid().ToString();

		public string InventoryName = "Inventory";
        public InventoryView view;
        public InventoryContainer container;

		

		private void Start() {
			inventoryID = System.Guid.NewGuid().ToString();

			container.Init();
		}
	}
}