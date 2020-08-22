using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
	public class TextsEffects : MonoBehaviour {
		private TextEffect prefabTextEffect;

		private List<Effect> currentEffects;

		public void Init(List<Effect> newCurrentEffects) {
			currentEffects = newCurrentEffects;
		}

		public void UpdateAllTXT() {

		}
	}
}