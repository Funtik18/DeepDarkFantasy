using DDF.Atributes;
using DDF.Help;
using System;
using System.Collections;
using UnityEngine;

namespace DDF.Character.Effects {
	[Serializable]
    public class Effect {
		[SerializeField]
		protected string effectName;
		[SerializeField]
		protected EffectTime effectTime;

		private CoroutineObject coroutineObject;

		protected Action onStartEffect;
		protected Funk mainFunk;
		protected Action onEndEffect;

		public delegate void Funk();


		public bool isEffectProcessing { get { return coroutineObject.IsProcessing; } }

		public Effect(string _effectName, MonoBehaviour owner, EffectTime neweffectTime) {
			effectName = _effectName;
			effectTime = neweffectTime;
			coroutineObject = new CoroutineObject(owner, EffectExecution);
		}
		public void Subscribe( Funk onMain, Action onStart = null, Action onEnd = null) {
			onStartEffect = onStart;
			mainFunk = onMain;
			onEndEffect = onEnd;
		}

		protected virtual IEnumerator EffectExecution() {
			onStartEffect?.Invoke();
			while (effectTime.actionCurrentTime <= effectTime.actionTime) {
				mainFunk();
				yield return new WaitForSeconds(effectTime.actionDelay);
				effectTime.actionCurrentTime += effectTime.actionDelay;
			}
			onEndEffect?.Invoke();
		}

		public virtual void StartEffect() {
			coroutineObject.Start();
		}
		public virtual void StopEffect() {
			coroutineObject.Stop();
		}
	}
	[Serializable]
	public struct EffectTime {
		public float actionTime;
		[ReadOnly]
		public float actionCurrentTime;
		public float actionDelay;

		public EffectTime(float _actionTime, float _actionCurrentTime, float _actionDelay ) {
			actionTime = _actionTime;
			actionCurrentTime = _actionCurrentTime;
			actionDelay = _actionDelay;
		}
	}
}