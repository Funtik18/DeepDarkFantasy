using DDF.Events;
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

        [SerializeField] public UER unityEventResponse;

		private void OnEnable() {
			if (gameEvent == null) return;

			GameEvent.RegisterListener(this);
		}
		private void OnDisable() {
			if (gameEvent == null) return;

			GameEvent.UnRegisterListener(this);
		}

		public void AddEvent(UnityAction<T> call ) {
			unityEventResponse.AddListener(call);
		}

		public void OnEventRaised( T item ) {
			unityEventResponse?.Invoke(item);
		}
	}
}
