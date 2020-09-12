using DDF.Inputs;
using DDF.UI.GUI;
using UnityEngine;

namespace DDF {
	/// <summary>
	/// Класс ссылка, помогает при работе с GUI.
	/// </summary>
    public class InventoryOverSeerGUI : InventoryOverSeer {
		protected static new InventoryOverSeerGUI _instance { get; private set; }

		private bool isOpen = false;

		public new static InventoryOverSeerGUI GetInstance() {
			if (_instance == null) {
				_instance = FindObjectOfType<InventoryOverSeerGUI>();
				DragParentsGUI.Init();
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