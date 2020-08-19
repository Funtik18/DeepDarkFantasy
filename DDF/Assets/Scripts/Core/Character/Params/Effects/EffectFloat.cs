using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
	public class EffectFloat : Effect {

		public new delegate void Funk( float param );
		protected new Funk mainFunk;

		float param;

		public EffectFloat( string effectName, MonoBehaviour owner, EffectTime neweffectTime, float _param ) : base(effectName, owner, neweffectTime) {
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
