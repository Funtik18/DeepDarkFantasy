using DDF.Help;
using UnityEngine;

namespace DDF.Events {
	public class VoidListener : BaseGameEventListener<Void, VoidEvent, UnityVoidEvent> {
		public VoidListener( MonoBehaviour newowner) {
			unityEventResponse = new UnityVoidEvent();
			owner = newowner;
			coroutineObject = new CoroutineObject(owner, EffectExecution);
		}
	}
}