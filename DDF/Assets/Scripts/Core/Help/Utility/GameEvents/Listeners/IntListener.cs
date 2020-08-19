using UnityEngine;

namespace DDF.Events {
    public class IntListener : BaseGameEventListener<int, IntEvent, UnityIntEvent> {
		public IntListener( MonoBehaviour newowner = null, EventTime neweventTime = null ) {
			unityEventResponse = new UnityIntEvent();
			owner = newowner;
			eventTime = neweventTime;
		}
	}
}