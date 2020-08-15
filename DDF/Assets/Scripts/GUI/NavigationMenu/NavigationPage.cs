using UnityEngine;
using UnityEngine.Events;

namespace DDF.UI.GUI {
    [RequireComponent(typeof(CanvasGroup))]
    public class NavigationPage : MonoBehaviour {
		private CanvasGroup canvasGroup;

		[HideInInspector]public int pageId;
		public string pageName;
		public NavigationButton navigationButton;

		public UnityAction<int> onClick;

		private void Awake() {
			canvasGroup = GetComponent<CanvasGroup>();
			navigationButton.button.onClick.AddListener(() => onClick?.Invoke(pageId));
		}

		public void OpenPage() {
			Help.HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup, true);
		}
		public void ClosePage() {
			Help.HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);
		}
	}
}