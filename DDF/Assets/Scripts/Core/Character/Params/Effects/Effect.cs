using DDF.Help;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Character.Effects {
	public abstract class Effect : ScriptableObject {
		public Sprite effectIcon;
		public string effectName = "Effect";

		public UnityAction<Effect> onStart;
		public UnityAction<Effect> onUpdate;
		public UnityAction<Effect> onEnd;
		public UnityAction<Effect> onDelete;


		protected Entity entity;
		public bool IsEffectProcessing { get { return coroutineObject.IsProcessing; } }
		protected CoroutineObject coroutineObject;

		public virtual void Init( Entity newentity ) {
			entity = newentity;
			coroutineObject = new Help.CoroutineObject(entity, EffectExecution);
		}

		public abstract void Execute();

		public abstract string Output();

		protected abstract IEnumerator EffectExecution();
	}
}
