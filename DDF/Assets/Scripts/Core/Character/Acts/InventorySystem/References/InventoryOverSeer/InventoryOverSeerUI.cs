using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {

	public class InventoryOverSeerUI : InventoryOverSeer {
		protected static new InventoryOverSeerUI _instance { get; private set; }

		public new static InventoryOverSeerUI Getinstance() {
			if (_instance == null) {
				_instance = FindObjectOfType<InventoryOverSeerUI>();
				DragParentsUI.Init();
			}
			return _instance;
		}
	}
}