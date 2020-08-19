using UnityEngine;

namespace DDF.Events {
	public class BoolListener : BaseGameEventListener<bool, BoolEvent, UnityBoolEvent> {
		public BoolListener( MonoBehaviour newowner = null, EventTime neweventTime = null ) {
			unityEventResponse = new UnityBoolEvent();
			owner = newowner;
			eventTime = neweventTime;
		}
	}
}