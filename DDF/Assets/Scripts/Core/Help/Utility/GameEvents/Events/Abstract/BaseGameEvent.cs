using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Events {
    public abstract class BaseGameEvent<T>: ScriptableObject {
        private readonly List<EventListener<T>> eventListeners = new List<EventListener<T>>();

        public string eventName;

        public void Raise(T item ) {
			for (int i = eventListeners.Count - 1; i >= 0; i--) {
				eventListeners[i].OnEventRaised(item);
			}
		}

		public void RegisterListener( EventListener<T> listener ) {
            if (!eventListeners.Contains(listener)) eventListeners.Add(listener);
        }
        public void UnRegisterListener( EventListener<T> listener ) {
            if (!eventListeners.Contains(listener)) eventListeners.Remove(listener);
        }
	}
}