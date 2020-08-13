using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DDF.UI.Customization {
	[RequireComponent(typeof(CanvasGroup))]
    public class Page : MonoBehaviour {
        public RectTransform page;

		private CanvasGroup canvasGroup;

		public Button nextBtn;
        public Button backBtn;

		private void Awake() {
			if (nextBtn == null) Debug.LogError("Error");

			canvasGroup = GetComponent<CanvasGroup>();
		}

		public void SetActions(UnityAction back, UnityAction next ) {
			backBtn?.onClick.AddListener(back);
			nextBtn.onClick.AddListener(next);
		}
		public void DeleteActions() {
			backBtn?.onClick.RemoveAllListeners();
			nextBtn.onClick.RemoveAllListeners();
		}

		public void EnablePage() {
			Help.HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup, true);
		}
		public void DisablePage() {
			Help.HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);
		}
	}
}