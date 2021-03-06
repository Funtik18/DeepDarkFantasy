﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
    [CreateAssetMenu(fileName = "EffectPosionHealth", menuName = "DDF/Character/Effects/EffectPosionHealth")]
    public class EffectPoisonHealth : Effect {
		public float posionAmount = -10f;
		public override void Execute() {
			coroutineObject.Start();
		}
		public override string Output() {
			return effectName;
		}

		protected override IEnumerator EffectExecution() {
			onStart?.Invoke(this);
			entity.RestoreHealth(posionAmount);
			onUpdate?.Invoke(this);
			yield return null;
			onEnd?.Invoke(this);
			onDelete?.Invoke(this);
		}
	}
}