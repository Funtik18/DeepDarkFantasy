using UnityEngine;

namespace DDF.Events {
	public class VoidListener : BaseGameEventListener<Void, VoidEvent, UnityVoidEvent> {
		public VoidListener( MonoBehaviour newowner = null, EventTime neweventTime = null ) {
			unityEventResponse = new UnityVoidEvent();
			owner = newowner;
			eventTime = neweventTime;
		}
	}
}