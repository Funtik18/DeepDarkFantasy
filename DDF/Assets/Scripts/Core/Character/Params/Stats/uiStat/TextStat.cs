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
		private TMPro.TextMeshProUGUI txtAmount;
		[SerializeField]
		private Button increaseButton;
		[SerializeField]
		private Button decreaseButton;

		private UnityAction onIncrease;
		private UnityAction onDecrease;

		private CanvasGroup increaseCanvasGroup;
		private CanvasGroup decreaseCanvasGroup;

		[SerializeField]
		private bool enableIncrease = true;
		[SerializeField]
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
			if (increaseButton) {
				increaseCanvasGroup = increaseButton.GetComponent<CanvasGroup>();
				EnableIncrease = enableIncrease;
			}
			if (decreaseButton) {
				decreaseCanvasGroup = decreaseButton.GetComponent<CanvasGroup>();
				EnableDecrease = enableDecrease;
			}
		}
		public void SetTrack(Stat currentStat, UnityAction currentActionIncrease = null, UnityAction currentActionDecrease = null) {
			currenStat = currentStat;

			increaseButton?.onClick.AddListener(currentActionIncrease);
			decreaseButton?.onClick.AddListener(currentActionDecrease);
		}
		public void UpdateText(string dop = "") {
			txt.text = "";
			txtAmount.text = "";

			string[] txts = currenStat.Output().Split('|');
			txt.text = txts[0];

			txtAmount.text = txts[1];

			txts = null;
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