using UnityEngine.Events;

namespace DDF.Events {
	public class FloatListener : BaseGameEventListener<float, FloatEvent, UnityFloatEvent> {
		public FloatListener() {
			unityEventResponse = new UnityFloatEvent();
		}
	}
}