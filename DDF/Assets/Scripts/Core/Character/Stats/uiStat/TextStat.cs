using DDF.Help;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DDF.Character.Stats {
	public class TextStat : MonoBehaviour {
		[SerializeField]
		private TMPro.TextMeshProUGUI txt;
		[SerializeField]
		private Button increaseButton;
		[SerializeField]
		private Button decreaseButton;

		private CanvasGroup increaseCanvasGroup;
		private CanvasGroup decreaseCanvasGroup;

		private bool enableIncrease = true;
		private bool enableDecrease = true;

		private Stat currenStat;

		public bool EnableIncrease {
			get {
				return enableIncrease;
			} set {
				enableIncrease = value;

				if (enableIncrease) {
					EnableIncreaseButton();
				} else {
					DisableIncreaseButton();
				}
			} 
		}
		public bool EnableDecrease {
			get {
				return enableDecrease;
			} set {
				enableIncrease = value;

				if (enableIncrease) {
					EnableDecreaseButton();
				} else {
					DisableDecreaseButton();
				}
			} 
		}


		private void Awake() {
			increaseCanvasGroup = increaseButton.GetComponent<CanvasGroup>();
			decreaseCanvasGroup = decreaseButton.GetComponent<CanvasGroup>();
		}
		public void SetTrack(Stat stat, UnityAction increase = null, UnityAction decrease = null) {
			currenStat = stat;

			if (increase != null && increaseButton != null)
				increaseButton.onClick.AddListener(increase);
			else EnableIncrease = false;
			if (decrease != null && decreaseButton != null)
				decreaseButton.onClick.AddListener(decrease);
			else EnableDecrease = false;
		}
		public void UpdateText() {
			if (currenStat is StatFloat statFloat) {
				float trackCurrentNum = statFloat.amount;

				txt.text = currenStat.name;
				txt.text += " " + trackCurrentNum;
			}
			if (currenStat is StatInt statInt) {
				int trackCurrentNum = statInt.amount;

				txt.text = currenStat.name;
				txt.text += " " + trackCurrentNum;
			}
			if (currenStat is StatRegularFloat statRegularFloat) {
				float trackCurrentNum = statRegularFloat.amount;
				float trackMax = statRegularFloat.max;

				txt.text = currenStat.name;
				txt.text += " "+ trackMax + "/" + trackCurrentNum;
			}
			if (currenStat is StatRegularInt statRegularInt) {
				int trackCurrentNum = statRegularInt.amount;
				int trackMax = statRegularInt.max;

				txt.text = currenStat.name;
				txt.text += " " + trackMax + "/" + trackCurrentNum;
			}
		}

		public void EnableIncreaseButton() {
			HelpFunctions.CanvasGroupSeer.EnableGameObject(increaseCanvasGroup, true);

		}
		public void DisableIncreaseButton() {
			HelpFunctions.CanvasGroupSeer.DisableGameObject(increaseCanvasGroup);
		}
		public void EnableDecreaseButton() {
			HelpFunctions.CanvasGroupSeer.EnableGameObject(decreaseCanvasGroup, true);

		}
		public void DisableDecreaseButton() {
			HelpFunctions.CanvasGroupSeer.DisableGameObject(decreaseCanvasGroup);
		}
	}
}