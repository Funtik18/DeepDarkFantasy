using System.Collections.Generic;
using DDF.Inputs;
using DDF.UI.GUI;
using DDF.UI.Inventory;
using UnityEngine;

namespace DDF {
	/// <summary>
	/// Класс ссылка, помогает при работе с GUI.
	/// </summary>
    public class InventoryOverSeerGUI : InventoryOverSeer {
		private static InventoryOverSeerGUI _instance;

		private bool isOpen = false;

		public static InventoryOverSeerGUI GetInstance() {
			if (_instance == null) {
				_instance = FindObjectOfType<InventoryOverSeerGUI>();
				DragParentsGUI.Init();
				_instance.Init();
			}
			return _instance;
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