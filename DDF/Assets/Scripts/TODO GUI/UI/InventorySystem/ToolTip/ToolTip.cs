using DDF.Help;
using DDF.Inventory.Items;
using UnityEngine;

namespace DDF.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
	public class ToolTip : MonoBehaviour {

		public static ToolTip _instance;

		private CanvasGroup canvasGroup;


		[SerializeField]
		private TMPro.TextMeshProUGUI text;
		[SerializeField]
		private RectTransform background;
		[SerializeField]
		private Vector2 preferedSize = new Vector2(300, 100);

		private float textPaddingSize = 4f;

		private bool isHide = true;

		private void Awake() {
			_instance = this;
			canvasGroup = GetComponent<CanvasGroup>();
		}

		public void SetPosition( Vector2 newPos ) {
			transform.position = newPos;
		}


		public void ShowToolTip( string toolTipText ) {

			text.text = toolTipText;
			Resize();

			HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup);

			isHide = false;
		}


		public void SetItem( Item item ) {
			text.text = item.name;

			Resize();
		}
		public void ShowToolTip() {



			HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup);

			isHide = false;
		}


		public void HideToolTip() {
			HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);

			isHide = true;
		}



		private void Resize() {
			Vector2 backgroundSize = text.GetPreferredValues() + new Vector2(textPaddingSize * 2f, textPaddingSize * 2f);
			background.sizeDelta = backgroundSize;
		}
	}
}