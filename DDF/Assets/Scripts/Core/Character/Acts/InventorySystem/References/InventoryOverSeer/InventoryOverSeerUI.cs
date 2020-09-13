using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF {
	/// <summary>
	/// Класс ссылка, помогает при работе с ui.
	/// </summary>
	public class InventoryOverSeerUI : InventoryOverSeer {
		protected static InventoryOverSeerUI _instance;

		public static InventoryOverSeerUI GetInstance() {
			if (_instance == null) {
				_instance = FindObjectOfType<InventoryOverSeerUI>();
				DragParentsUI.Init();
				_instance.Init();
			}
			return _instance;
		}
	}
}