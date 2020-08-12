using DDF.Help;
using System;
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

		private UnityAction onIncrease;
		private UnityAction onDecrease;

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
		public void SetTrack( Tuple<Stat, UnityAction, UnityAction> stat) {
			currenStat = stat.Item1;

			increaseButton.onClick.AddListener(stat.Item2);
			decreaseButton.onClick.AddListener(stat.Item3);
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
				float amount = statRegularFloat.amount;
				float currentInamount = statRegularFloat.currentInamount;

				txt.text = currenStat.name;
				txt.text += " " + currentInamount + "/" + amount;
			}
			if (currenStat is StatRegularInt statRegularInt) {
				int amount = statRegularInt.amount;
				int currentInamount = statRegularInt.currentInamount;

				txt.text = currenStat.name;
				txt.text += " " + currentInamount + "/" + amount;
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