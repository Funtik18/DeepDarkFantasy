using DDF.UI.Inventory.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {

    public class InventoryOverSeer : MonoBehaviour {
        public static InventoryOverSeer _instance;

		public Inventory mainInventory;


		[HideInInspector]
		public RectTransform buffer;

		[HideInInspector]
		public InventorySlot lastSlot;
		[HideInInspector]
		public InventorySlot rootSlot;

		[HideInInspector]
		public InventoryModel lastModel;
		[HideInInspector]
		public InventoryModel rootModel;

		[HideInInspector]
		public bool isDrag = false;


		[HideInInspector]
		public List<Inventory> containers;


		[HideInInspector]
		public InventoryContainer from;//откуда взяли
		[HideInInspector]
		public InventoryContainer whereNow;//где сейчас находимся

		private void Awake() {
			_instance = this;

			containers = new List<Inventory>();

		}

		public void OrderRefresh() {
			DragParents._instance.transform.SetAsLastSibling();
			ToolTip._instance.transform.SetAsLastSibling();
			MenuOptions._instance.transform.SetAsLastSibling();

		}

		public void RegistrationContainer( Inventory container ) {
			containers.Add(container);
		}
	}
}