using DDF.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
	[CreateAssetMenu(fileName = "EffectRestoreHealth", menuName = "DDF/Character/Effects/EffectRestoreHealth")]
	public class EffectRestoreHealth : Effect {
		public float healAmount = 10f;
		public override void Execute() {
			coroutineObject.Start();
		}
		public override string Output() {
			return effectName;
		}

		protected override IEnumerator EffectExecution() {
			onStart?.Invoke();
			entity.RestoreHealth(healAmount);
			onUpdate?.Invoke();
			yield return null;
			onEnd?.Invoke();
			onDelete?.Invoke(this);
		}
	}
}