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
    public abstract class BaseGameEventListener<T, E, UER> : EventListener<T> where E: BaseGameEvent<T> where UER : UnityEvent<T> {
        [SerializeField]
        private E gameEvent;
		public E GameEvent { get => this.gameEvent; set => this.gameEvent = value; }

		private T item;

		[SerializeField] protected UER unityEventResponse;

		/*private void OnEnable() {
			GameEvent?.RegisterListener(this);
		}
		private void OnDisable() {
			GameEvent?.UnRegisterListener(this);
		}*/

		public void AddEvent(UnityAction<T> call ) {
			unityEventResponse.AddListener(call);
		}
		public override void AddActions( UnityAction call0, UnityAction call1, UnityAction call2 ) {
			OnStartEvent = call0;
			OnUpdateEvent = call1;
			OnEndEvent = call2;
		}
		public void RemoveEvent( UnityAction<T> call ) {
			unityEventResponse.RemoveListener(call);
		}
		public void RemoveAllEvents() {
			unityEventResponse.RemoveAllListeners();
		}

		public override void OnEventRaised( T newitem ) {
			item = newitem;
			if (owner != null) {
				//if (eventTime.actionCurrentStep == 0) eventTime = new EventTime(0.1f, 0.1f, 0, 0);
				eventTime = new EventTime(1f, 1f, 1f, 0);
				coroutineObject.Start();
			} else {
				unityEventResponse?.Invoke(item);
			}
		}
		protected virtual IEnumerator EffectExecution() {
			//OnStartEvent?.Invoke();
			while (eventTime.actionCurrentStep < eventTime.actionTime) {
				Debug.LogError(" = " + eventTime.actionTime + "/" + eventTime.actionCurrentStep);

				unityEventResponse?.Invoke(item);
			///OnUpdateEvent?.Invoke();
				yield return new WaitForSeconds(eventTime.actionDelay);
				eventTime.actionCurrentStep += eventTime.actionStep;
			}
			//OnEndEvent?.Invoke();
		}
	}
	
}
