  using DDF.UI.Inventory.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {

    public class InventoryOverSeer : MonoBehaviour {
        public static InventoryOverSeer _instance;

		[HideInInspector] public Canvas mainCanvas;

		public Inventory mainInventory;


		[HideInInspector] public RectTransform buffer;

		[HideInInspector] public InventorySlot lastSlot;
		[HideInInspector] public InventorySlot rootSlot;

		[HideInInspector] public InventoryModel lastModel;
		[HideInInspector] public InventoryModel rootModel;

		[HideInInspector] public bool isDrag = false;


		[HideInInspector] public List<Inventory> containers;


		[HideInInspector] public InventoryContainer from;//откуда взяли
		[HideInInspector] public InventoryContainer whereNow;//где сейчас находимся

		[HideInInspector] public RenderMode renderMode;

		private void Awake() {
			_instance = this;

			mainCanvas = GetComponentInParent<Canvas>();
			containers = new List<Inventory>();

			renderMode = mainCanvas.renderMode;

		}

		public void OrderRefresh() {
			DragParents._instance.transform.SetAsLastSibling();
			ToolTip._instance.transform.SetAsLastSibling();
			MenuOptions._instance.transform.SetAsLastSibling();

		}

		public void RegistrationContainer( Inventory container ) {
			if(renderMode == RenderMode.ScreenSpaceOverlay) {//3d
				container.is3dOr2d = true;
			}
			if (renderMode == RenderMode.ScreenSpaceCamera) {//2d
				container.is3dOr2d = false;
			}
			if (renderMode == RenderMode.WorldSpace) {//does not mater
				container.is3dOr2d = false;
			}

			containers.Add(container);
		}
	}
}