using DDF.Events;
using DDF.Help;
using DDF.UI.Inventory;
using DDF.UI.Inventory.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {

	public class Effects {

		private Entity entity;
		private List<Effect> effects;
		private CoroutineObject coroutineObject;
		
		public Effects( Entity newEntity ) {
			entity = newEntity;
			effects = entity.currentEffects;

			coroutineObject = new CoroutineObject(entity, EffectMonitoring);
			coroutineObject.Start();
		}

		private IEnumerator EffectMonitoring() {
			while (true) {
				for (int i = 0; i < effects.Count; i++) {
					if (effects[i].IsEffectProcessing) {
						Debug.LogError(effects[i].Output());
					} else {
						effects[i].Execute();
					}
				}
				yield return null;
			}
		}
	}
}