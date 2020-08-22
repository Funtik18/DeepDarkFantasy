using DDF.Help;
using UnityEngine;

namespace DDF.Events {
	public class FloatListener : BaseGameEventListener<float, FloatEvent, UnityFloatEvent> {
		public FloatListener(MonoBehaviour newowner) {
			unityEventResponse = new UnityFloatEvent();
			owner = newowner;
			coroutineObject = new CoroutineObject(owner, EffectExecution);
		}
	}
}