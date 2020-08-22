using DDF.Help;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Events {
    
    public abstract class GameEventListener {
		public UnityAction OnStartEvent;
		public UnityAction OnUpdateEvent;
		public UnityAction OnEndEvent;

        protected MonoBehaviour owner;
        protected CoroutineObject coroutineObject;
        public EventTime eventTime;

        public bool isEffectProcessing { get { return coroutineObject.IsProcessing; } }

        public abstract void AddActions( UnityAction call0, UnityAction call1, UnityAction call2 );
		
    }
	/// <summary>
	/// Интерфейс слушателя событий.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class EventListener<T> : GameEventListener {
		public abstract void OnEventRaised( T item );
	}


	[Serializable]
	public struct EventTime {
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

		public EventTime( float _actionTime, float _actionStep, float _actionDelay, float _actionCurrentStep ) {
			actionTime = _actionTime;
			actionStep = _actionStep;
			actionDelay = _actionDelay;
			actionCurrentStep = _actionCurrentStep;
		}
	}
}