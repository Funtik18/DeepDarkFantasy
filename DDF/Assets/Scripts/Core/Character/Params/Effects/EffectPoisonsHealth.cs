using System.Collections;
using UnityEngine;

namespace DDF.Character.Effects {
	[CreateAssetMenu(fileName = "EffectPoisonsHealth", menuName = "DDF/Character/Effects/EffectPoisonsHealth")]
	public class EffectPoisonsHealth : Effect {
		public float posionAmount = -10f;

		public float actionAmount = 10f;
		public float actionStep = 1f;
		public float actionDelay = 1f;

		private float actionCurrentStep = 0f;
		public override void Execute() {
			coroutineObject.Start();
		}

		public override string Output() {
			return effectName + "|" + actionAmount + "/" + actionCurrentStep;
		}

		protected override IEnumerator EffectExecution() {
			onStart?.Invoke(this);
			while (actionCurrentStep < actionAmount) {
				entity.RestoreHealth(posionAmount);
				onUpdate?.Invoke(this);
				yield return new WaitForSeconds(actionDelay);
				actionCurrentStep += actionStep;
			}
			onEnd?.Invoke(this);
			onDelete?.Invoke(this);
		}
	}
}