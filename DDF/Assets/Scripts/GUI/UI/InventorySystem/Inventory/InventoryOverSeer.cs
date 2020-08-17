using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {
    public class InventoryOverSeer : MonoBehaviour {
		public static InventoryOverSeer _instance { get; private set; }

		[HideInInspector] public List<Inventory> containers;


		[HideInInspector] public RectTransform buffer;

		[HideInInspector] public InventorySlot lastSlot;
		[HideInInspector] public InventorySlot rootSlot;

		[HideInInspector] public InventoryModel lastModel;
		[HideInInspector] public InventoryModel rootModel;

		[HideInInspector] public bool isDrag = false;

		[HideInInspector] public InventoryContainer from;//откуда взяли
		[HideInInspector] public InventoryContainer whereNow;//где сейчас находимся

		protected virtual void Awake() {
			containers = new List<Inventory>();
		}

		public void Show() {
			for (int i = 0; i < containers.Count; i++) {
				containers[i].ShowInventory();
			}
		}
		public void Hide() {
			for(int i = 0; i < containers.Count; i++) {
				containers[i].HideInventory();
			}
		}

		public void OrderRefresh() {
			//DragParents._instance.transform.SetAsLastSibling();
			ToolTip._instance.transform.SetAsLastSibling();
			MenuOptions._instance.transform.SetAsLastSibling();
		}

		public void RegistrationContainer( Inventory container ) {
			containers.Add(container);
		}
	}
}