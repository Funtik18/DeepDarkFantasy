using System;
using UnityEngine;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
    public class Inventory : MonoBehaviour {

		public string inventoryID;

		public string InventoryName = "Inventory";
        public InventoryView view;
        public InventoryContainer container;

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