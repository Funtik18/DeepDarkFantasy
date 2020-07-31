using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {

    public class InventoryOverSeer : MonoBehaviour {
        public static InventoryOverSeer _instance;

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
		public List<InventoryContainer> containers;
		[HideInInspector]
		public List<Transform> dragParents;


		[HideInInspector]
		public InventoryContainer from;//откуда взяли
		[HideInInspector]
		public InventoryContainer whereNow;//где сейчас находимся

		private void Awake() {
			_instance = this;

			containers = new List<InventoryContainer>();
		}

		public void RegistrationContainer(InventoryContainer container) {
			containers.Add(container);
			dragParents.Add(container.view.dragParent);
		}
	}
}