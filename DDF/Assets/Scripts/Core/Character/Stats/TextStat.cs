using DDF.Character.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.Character.Stats {
	[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
	public class TextStat : MonoBehaviour {
		public Stat stat;

		TMPro.TextMeshProUGUI txt;

		private void Awake() {
			txt = GetComponent<TMPro.TextMeshProUGUI>();
			UpdateText();
		}
		public void UpdateText() {
			if (stat == null) return;
			txt.text = stat.name + " = " + stat.amount;
		}
	}
}