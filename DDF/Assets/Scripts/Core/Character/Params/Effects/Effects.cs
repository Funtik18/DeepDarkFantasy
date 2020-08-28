using DDF.Events;
using DDF.Help;
using DDF.UI.Inventory;
using DDF.UI.Inventory.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
	/// <summary>
	/// Класс предназначен для слежением за effects на протежении всей игры.
	/// </summary>
	public class Effects {

		private Entity entity;
		private List<Effect> currEffects;
		private CoroutineObject coroutineObject;
		
		public Effects( Entity newEntity ) {
			entity = newEntity;
			currEffects = entity.currentEffects;

			coroutineObject = new CoroutineObject(entity, EffectMonitoring);
			coroutineObject.Start();
		}

		/// <summary>
		/// Добавляет и сразу запускает эффект.
		/// </summary>
		public void AddEffect(Effect effect) {
			Debug.LogError(effect.effectName);
			effect.Init(entity);
			effect.onDelete = (x) => RemoveEffect(x);
			entity.currentEffects.Add(effect);
		}
		/// <summary>
		/// Удаление эффекта.
		/// </summary>
		public void RemoveEffect(Effect effect) {
			entity.currentEffects.Remove(effect);
		}

		private IEnumerator EffectMonitoring() {
			while (true) {
				for (int i = 0; i < currEffects.Count; i++) {
					if (currEffects[i].IsEffectProcessing) {
						//Debug.LogError(effects[i].Output());
					} else {
						currEffects[i].Execute();
					}
				}
				yield return null;
			}
		}
	}
}