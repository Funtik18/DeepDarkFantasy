using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {

	public class InventoryOverSeerUI : InventoryOverSeer {
		public static new InventoryOverSeerUI _instance { get; private set; }

		protected override void Awake() {
			base.Awake();
			DragParentsUI.Init();
			if (_instance == null)
				_instance = this;
		}
	}
}