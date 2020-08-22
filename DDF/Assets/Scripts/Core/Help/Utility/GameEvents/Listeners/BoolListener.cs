using DDF.Help;
using UnityEngine;

namespace DDF.Events {
	public class BoolListener : BaseGameEventListener<bool, BoolEvent, UnityBoolEvent> {
		public BoolListener( MonoBehaviour newowner) {
			unityEventResponse = new UnityBoolEvent();
			owner = newowner;
			coroutineObject = new CoroutineObject(owner, EffectExecution);
		}
	}
}