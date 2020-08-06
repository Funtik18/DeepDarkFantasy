using System;
using UnityEngine;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
    public class Inventory : MonoBehaviour {

		public string inventoryID;

		/// <summary>
		/// если тру то 3д если фалсе то 2д.
		/// </summary>
		[HideInInspector]
		public bool is3dOr2d = true;

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