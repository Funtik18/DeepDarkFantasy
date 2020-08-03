using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.UI {
    [RequireComponent(typeof(Button))]
    public class ButtonCloseUI : MonoBehaviour {
        public CanvasGroup canvasGroup;

		private void Awake() {
			Button btn = GetComponent<Button>();

			btn.onClick.AddListener(delegate { Close(); });
		}

		protected virtual void Close() {
			Help.HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);
		}

	}
}