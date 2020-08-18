using DDF.Help;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
    public class Effect : MonoBehaviour {
        public string effectName;

		public float actionTime = 5f;
		public float actionDelay = 0.5f;

		private Coroutine routine;

		private CoroutineObject coroutineObject;
		private Func<IEnumerator> routine;

		public bool isEffectProcessing { get { return coroutineObject.IsProcessing; } }

		private void Awake() {

			routine += EffectExecution;

			coroutineObject = new CoroutineObject(this, routine);
		}
		private void Update() {
			print(isEffectProcessing);
		}

		private IEnumerator EffectExecution() {

			while (true) {
				yield return new WaitForSeconds(actionDelay);

			}

			print("-");
			yield return break;
			print("-----");

		}


		public void StartEffect() {
			coroutineObject.Start();
		}
		public void StopEffect() {
			coroutineObject.Stop();
		}
	}
}