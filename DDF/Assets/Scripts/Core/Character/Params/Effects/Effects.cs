using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
    public class Effects : MonoBehaviour {
        public List<Effect> effects;

		private void Awake() {
			effects.Add(new Effect("Востановление", this, delegate { print("+"); }, 10f, 0.1f));

			effects[0].StartEffect();
		}
	}
}