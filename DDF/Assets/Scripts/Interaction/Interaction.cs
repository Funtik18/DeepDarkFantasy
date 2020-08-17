using DDF.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Environment {
	[RequireComponent(typeof(BoxCollider))]
	public class Interaction : MonoBehaviour, IInteraction {

		public UnityEvent currentEventEnter = new UnityEvent();
		public UnityEvent currentEventStay = new UnityEvent();
		public UnityEvent currentEventExit = new UnityEvent();

		public bool isInField = false;


		public virtual void OnTriggerEnter( Collider other ) {
			isInField = true;
			currentEventEnter?.Invoke();
		}

		public virtual void OnTriggerStay( Collider other ) {
			isInField = true;
			currentEventStay?.Invoke();
		}

		public virtual void OnTriggerExit( Collider other ) {
			isInField = false;
			currentEventExit?.Invoke();
		}

		private void OnDestroy() {
			currentEventEnter.RemoveAllListeners();
			currentEventStay.RemoveAllListeners();
			currentEventExit.RemoveAllListeners();
		}
	}
}