﻿using DDF.Events;
using DDF.Help;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
    public class Effects {

		public Entity entity;

		private CoroutineObject coroutineObject;

		public Effect EffectDeath;
		public EffectFloat EffectRestoreHealth;
		public EffectFloat EffectTakeDamage;

		private FloatListener listenerRestoreHealth;
		private VoidListener voidListener;

		private Dictionary<string, Effect> mainEffects;
		public Effects( Entity newEntity ) {
			mainEffects = new Dictionary<string, Effect>();
			entity = newEntity;
		}
		public void Init() {

			//voidListener = new VoidListener();
			//voidListener.GameEvent = Resources.Load<VoidEvent>("Prefabs/ASSETS/ItemsEvents/VoidEvent_Print");
			//voidListener = Print);


			listenerRestoreHealth = new FloatListener();
			listenerRestoreHealth.GameEvent = Resources.Load<FloatEvent>("Prefabs/ASSETS/ItemsEvents/FloatEvent_RestoreHealth");
			listenerRestoreHealth.GameEvent.RegisterListener(listenerRestoreHealth);
			listenerRestoreHealth.AddEvent(entity.RestoreHealth);


			/*EffectDeath = new Effect("Смерть", entity, new EffectTime(1f, 0f, 1f));
			EffectDeath.Subscribe(DeathEffect, null, null);

			EffectRestoreHealth = new EffectFloat("Востановление", entity, new EffectTime(10f, 0f, 0.5f), 1);
			EffectRestoreHealth.Subscribe(RestoreHealthEffect, null, null);

			EffectTakeDamage = new EffectFloat("Отрава", entity, new EffectTime(10f, 0f, 0.5f), 2);
			EffectTakeDamage.Subscribe(TakeDamage, null, null);

			mainEffects.Add("EffectDeath", EffectDeath);
			mainEffects.Add("EffectRestoreHealth", EffectRestoreHealth);
			mainEffects.Add("EffectTakeDamage", EffectTakeDamage);

			coroutineObject = new CoroutineObject(entity, Wait);*/
		}
		private void Print(float v) {
			Debug.LogError("s = " + v);
		}


		private IEnumerator Wait() {
			while (true) {
				if (entity.currentEffects.Count == 0) break;
				for (int i = 0; i < entity.currentEffects.Count; i++) {
					if (entity.currentEffects[i].isEffectProcessing) {
						yield return null;
					} else {
						RemoveEffect(entity.currentEffects[i]);
					}
				}
			}
		}

		public void UpdateEffects() {
			for (int i = 0; i < entity.currentEffects.Count; i++) {
				entity.currentEffects[i]?.StartEffect();
			}
			coroutineObject.Start();
		}


		private Effect GetEffect( string effectName ) {
			Effect effect;
			mainEffects.TryGetValue(effectName, out effect);
			return effect;
		}
		public Dictionary<string, Effect> GetAllEffects() {
			return mainEffects;
		}

		public void AddEffect(string effectName) {
			entity.currentEffects.Add(GetEffect(effectName));
		}
		public void AddEffect( Effect effect ) {
			entity.currentEffects.Add(effect);
		}

		public void RemoveEffect( string effectName ) {
			entity.currentEffects.Remove(GetEffect(effectName));
		}
		public void RemoveEffect( Effect effect ) {
			entity.currentEffects.Remove(effect);
		}

		private void TakeDamage() {
			entity.TakeDamage(2);
		}
		private void TakeDamage(float damage) {
			entity.TakeDamage(damage);
		}
	}
}