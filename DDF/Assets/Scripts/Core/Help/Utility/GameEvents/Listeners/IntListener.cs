using DDF.Help;
using UnityEngine;

namespace DDF.Events {
    public class IntListener : BaseGameEventListener<int, IntEvent, UnityIntEvent> {
		public IntListener( MonoBehaviour newowner) {
			unityEventResponse = new UnityIntEvent();
			owner = newowner;
			coroutineObject = new CoroutineObject(owner, EffectExecution);
		}
	}
}