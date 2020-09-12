﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF {
	/// <summary>
	/// Класс ссылка, помогает при работе с ui.
	/// </summary>
	public class InventoryOverSeerUI : InventoryOverSeer {
		protected static new InventoryOverSeerUI _instance { get; private set; }

		public new static InventoryOverSeerUI GetInstance() {
			if (_instance == null) {
				_instance = FindObjectOfType<InventoryOverSeerUI>();
				DragParentsUI.Init();
			}
			return _instance;
		}
	}
}