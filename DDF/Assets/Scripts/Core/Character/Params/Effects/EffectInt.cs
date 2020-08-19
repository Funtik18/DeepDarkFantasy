using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
	public class EffectInt : Effect {

		public new delegate void Funk( int param );
		protected new Funk mainFunk;

		int param;

		public EffectInt( string effectName, MonoBehaviour owner, EffectTime neweffectTime, int _param ) : base(effectName, owner, neweffectTime) {
			param = _param;
		}

		public void Subscribe( Funk onMain, Action onStart = null, Action onEnd = null ) {
			onStartEffect = onStart;
			mainFunk = onMain;
			onEndEffect = onEnd;
		}
		protected override IEnumerator EffectExecution() {
			onStartEffect?.Invoke();
			while (effectTime.actionCurrentTime <= effectTime.actionTime) {
				mainFunk(param);
				yield return new WaitForSeconds(effectTime.actionDelay);
				effectTime.actionCurrentTime += effectTime.actionDelay;
			}
			onEndEffect?.Invoke();
		}
	}
}