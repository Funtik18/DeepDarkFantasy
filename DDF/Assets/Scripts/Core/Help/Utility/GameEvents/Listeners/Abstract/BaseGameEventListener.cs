using DDF.Events;
using DDF.Help;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Events {
	/// <summary>
	/// Базовый событийный слушатель.
	/// </summary>
	/// <typeparam name="T">T-type</typeparam>
	/// <typeparam name="E">E-event</typeparam>
	/// <typeparam name="UER">UER-unity event</typeparam>
    public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T> where E: BaseGameEvent<T> where UER : UnityEvent<T> {
        [SerializeField]
        private E gameEvent;
		public E GameEvent { get => this.gameEvent; set => this.gameEvent = value; }

		private T item;
		
		[SerializeField] protected UER unityEventResponse;

		protected MonoBehaviour owner;
		protected CoroutineObject coroutineObject;
		protected EventTime eventTime;
		public bool isEffectProcessing { get { return coroutineObject.IsProcessing; } }

		private void OnEnable() {
			GameEvent?.RegisterListener(this);
		}
		private void OnDisable() {
			GameEvent?.UnRegisterListener(this);
		}

		public void AddEvent(UnityAction<T> call ) {
			unityEventResponse.AddListener(call);
		}
		public void RemoveEvent( UnityAction<T> call ) {
			unityEventResponse.RemoveListener(call);
		}
		public void RemoveAllEvents() {
			unityEventResponse.RemoveAllListeners();
		}

		public void OnEventRaised( T newitem ) {
			item = newitem;
			if (owner != null) {
				if (eventTime == null) eventTime = new EventTime(0.1f, 0.1f, 0);
				coroutineObject = new CoroutineObject(owner, EffectExecution);
				coroutineObject.Start();
			} else {
				unityEventResponse?.Invoke(item);
			}
		}
		protected virtual IEnumerator EffectExecution() {
			//onStartEffect?.Invoke();
			while (eventTime.actionCurrentStep < eventTime.actionTime) {
				unityEventResponse?.Invoke(item);
				yield return new WaitForSeconds(eventTime.actionDelay);
				eventTime.actionCurrentStep += eventTime.actionStep;
			}
			//onEndEffect?.Invoke();
		}
	}
	[Serializable]
	public class EventTime {
		/// <summary>
		/// Когда закончиться.
		/// </summary>
		public float actionTime;
		/// <summary>
		/// Задержка.
		/// </summary>
		public float actionDelay;
		/// <summary>
		/// Шаг.
		/// </summary>
		public float actionStep;
		/// <summary>
		/// Текущий шаг.
		/// </summary>
		public float actionCurrentStep;

		public EventTime( float _actionTime, float _actionStep, float _actionDelay ) {
			actionTime = _actionTime;
			actionStep = _actionStep;
			actionDelay = _actionDelay;
		}
	}
}
