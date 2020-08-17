using System;
using UnityEngine;


namespace DDF.Inputs {
    public class InputManager : MonoBehaviour {
		public static Action<int> onGUIOpen;
		public static Action onGUIClose;

		public static Action onUIOpen;
		public static Action onUIClose;

		public static Action onUse;


		private bool isCloseOnFirstTime = true;

		private void Update() {
			if(isCloseOnFirstTime == true) {//костыль
				onGUIClose?.Invoke();
				isCloseOnFirstTime = false;
			}


			if (Input.GetButtonDown("InventoryPage")) {
				OpenGUIPage(0);
			}
			if (Input.GetButtonDown("MapPage")) {
				OpenGUIPage(1);
			}
			if (Input.GetButtonDown("QuestsPage")) {
				OpenGUIPage(2);
			}
			if (Input.GetButtonDown("AlchemyPage")) {
				OpenGUIPage(3);
			}
			if (Input.GetButtonDown("JournalPage")) {
				OpenGUIPage(4);
			}
			if (Input.GetButtonDown("GlossariesPage")) {
				OpenGUIPage(5);
			}

			if (Input.GetButtonDown("Use")) {
				onUse?.Invoke();
				onUse = null;
			}

			if (Input.GetButtonDown("ESC")) {
				CloseGUI();
				CloseUI();
			}
		}
		private void OpenGUIPage(int pageId) {
			if (GameProcess.State == GameState.stream) {
				GameProcess.Pause();
				onGUIOpen?.Invoke(pageId);
				onUIClose?.Invoke();
				return;
			}
			CloseGUI();
			onUIOpen?.Invoke();
		}
		private void CloseGUI() {
			if (GameProcess.State == GameState.pause) {
				onGUIClose?.Invoke();
				GameProcess.Resume();
			}
		}
		private void CloseUI() {
			onUIClose?.Invoke();
		}
	}
}