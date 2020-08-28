using DDF.Help;
using DDF.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace DDF.Character.Effects {
	/// <summary>
	/// Класс предназначен для слежением за effects на протежении всей игры.
	/// </summary>
	public class Effects {

		private Entity entity;
		private List<Effect> currEffects;
		private CoroutineObject coroutineObject;

		public UnityAction<Effect> effectOnStart;
		public UnityAction<Effect> effectOnUpdate;
		public UnityAction<Effect> effectOnEnd;
		public UnityAction<Effect> effectOnDelete;

		public Effects( Entity newEntity ) {
			entity = newEntity;
			currEffects = entity.currentEffects;

			effectOnDelete += (x) => RemoveEffect(x);

			coroutineObject = new CoroutineObject(entity, EffectMonitoring);
			coroutineObject.Start();
		}

		/// <summary>
		/// Добавляет и сразу запускает эффект.
		/// </summary>
		public void AddEffect(Effect effect) {
			effect.Init(entity);
			effect.onStart = effectOnStart;
			effect.onUpdate = effectOnUpdate;
			effect.onEnd = effectOnEnd;
			effect.onDelete = effectOnDelete;
			entity.currentEffects.Add(effect);
		}
		/// <summary>
		/// Загрузка эффекта из ресурсов и добавление.
		/// </summary>
		/// <param name="effect"></param>
		public void AddEffect(string effect) {
			AddEffect(ScriptableObject.Instantiate(Resources.Load<Effect>(FileManager.EFFECTS_PATH + "/" + effect)));
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