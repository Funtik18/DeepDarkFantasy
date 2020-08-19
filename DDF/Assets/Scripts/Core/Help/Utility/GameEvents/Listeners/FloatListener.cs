using UnityEngine;

namespace DDF.Events {
	public class FloatListener : BaseGameEventListener<float, FloatEvent, UnityFloatEvent> {
		public FloatListener(MonoBehaviour newowner = null, EventTime neweventTime = null) {
			unityEventResponse = new UnityFloatEvent();
			owner = newowner;
			eventTime = neweventTime;
		}
	}
}