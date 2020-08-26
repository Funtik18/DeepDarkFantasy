using DDF.Inputs;
using DDF.UI.GUI;
using DDF.UI.Inventory.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {
    public class InventoryOverSeerGUI : InventoryOverSeer {
		public static new InventoryOverSeerGUI _instance { get; private set; }

		public Inventory mainInventory;
		public Equipment mainEquipment;

		/*[Header("Toggles")]
		public bool DisableWorldDragging;
		public bool DisableWorldDropping;*/

		private bool isOpen = false;

		protected override void Awake() {
			base.Awake();
			DragParentsGUI.Init();
			if (_instance == null)
				_instance = this;
		}

		private void Update() {
			if (Input.GetButtonDown(InputManager.ButtonInventoryPage)) {
				if (isOpen) {
					CloseGUI();
				} else {
					OpenGUI(navigation.startPage);
				}
			}
			if (Input.GetButtonDown(InputManager.ButtonESC) && isOpen) {
				CloseGUI();
			}
		}
		[SerializeField]
		private CanvasGroup GUINavigator;

		[SerializeField]
		private NavigationBar navigation;
		private void ChoosePage( int pageId ) {
			navigation.SetCurrentPage(pageId);
		}
		public void OpenGUI( int pageId ) {
			GameProcess.Pause();
			ChoosePage(pageId);
			Help.HelpFunctions.CanvasGroupSeer.EnableGameObject(GUINavigator, true);
			Show();
			isOpen = true;
		}
		public void CloseGUI() {
			Hide();
			Help.HelpFunctions.CanvasGroupSeer.DisableGameObject(GUINavigator);
			GameProcess.Resume();
			isOpen = false;
		}
	}
}