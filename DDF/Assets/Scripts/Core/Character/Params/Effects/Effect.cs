using DDF.Atributes;
using DDF.Help;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
	[Serializable]
    public class Effect {
        public string effectName;

		public float actionTime = 5f;
		[ReadOnly]
		public float actionCurrentTime = 0f;
		public float actionDelay = 0.5f;

		private CoroutineObject coroutineObject;
		private Action effect;

		public bool isEffectProcessing { get { return coroutineObject.IsProcessing; } }

		public Effect(string _effectName, MonoBehaviour owner, Action effect, float duration, float often) {
			effectName = _effectName;
			actionTime = duration;
			actionDelay = often;
			coroutineObject = new CoroutineObject(owner, EffectExecution);
		}

		private IEnumerator EffectExecution() {

			while (actionCurrentTime <= actionTime) {
				effect?.Invoke();
				yield return new WaitForSeconds(actionDelay);
				actionCurrentTime += actionDelay;
			}
		}


		public void StartEffect() {
			coroutineObject.Start();
		}
		public void StopEffect() {
			coroutineObject.Stop();
		}
	}
}