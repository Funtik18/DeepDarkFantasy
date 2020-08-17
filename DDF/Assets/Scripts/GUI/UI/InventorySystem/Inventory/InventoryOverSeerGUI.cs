  using DDF.UI.Inventory.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {
    public class InventoryOverSeerGUI : InventoryOverSeer {
		public static new InventoryOverSeerGUI _instance { get; private set; }

		public Inventory mainInventory;

		[Header("Toggles")]
		public bool DisableWorldDragging;
		public bool DisableWorldDropping;

		protected override void Awake() {
			base.Awake();
			DragParentsGUI.Init();
			if (_instance == null)
				_instance = this;
		}
	}
}