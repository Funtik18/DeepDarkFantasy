using DDF.Events;
using DDF.Help;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
    public class Effects {

		public Entity entity;

		private CoroutineObject coroutineObject;

		private FloatListener listenerRestoreHealth;
		private FloatListener listenerRegenerateHealth;

		private FloatListener listenerRestoreMana;
		private FloatListener listenerRegenerateMana;

		//public Dictionary<string, EventTime> currentEffects;

		public List<object> effects;

		public Effects( Entity newEntity ) {
			effects = new List<object>();
			entity = newEntity;
		}
		public void Init() {

			listenerRestoreHealth = new FloatListener(entity);
			listenerRestoreHealth.GameEvent = Resources.Load<FloatEvent>("Prefabs/ASSETS/ItemsEvents/FloatEvent_RestoreHealth");
			listenerRestoreHealth.GameEvent.RegisterListener(listenerRestoreHealth);
			listenerRestoreHealth.AddEvent(entity.RestoreHealth);

			listenerRegenerateHealth = new FloatListener(entity, new EventTime(10, 1, 1));
			listenerRegenerateHealth.GameEvent = Resources.Load<FloatEvent>("Prefabs/ASSETS/ItemsEvents/FloatEvent_RegenerateHealth");
			listenerRegenerateHealth.GameEvent.RegisterListener(listenerRegenerateHealth);
			listenerRegenerateHealth.AddEvent(entity.RestoreHealth);

			listenerRestoreMana = new FloatListener(entity);
			listenerRestoreMana.GameEvent = Resources.Load<FloatEvent>("Prefabs/ASSETS/ItemsEvents/FloatEvent_RestoreMana");
			listenerRestoreMana.GameEvent.RegisterListener(listenerRestoreMana);
			listenerRestoreMana.AddEvent(entity.RestoreMana);

			listenerRegenerateMana = new FloatListener(entity, new EventTime(10, 1, 1));
			listenerRegenerateMana.GameEvent = Resources.Load<FloatEvent>("Prefabs/ASSETS/ItemsEvents/FloatEvent_RegenerateMana");
			listenerRegenerateMana.GameEvent.RegisterListener(listenerRegenerateMana);
			listenerRegenerateMana.AddEvent(entity.RestoreMana);
			listenerRegenerateMana.AddEvent(Add);

			//effects.Add(listenerRestoreHealth);
			///effects.Add(listenerRegenerateHealth);
			//effects.Add(listenerRestoreMana);
			//effects.Add(listenerRegenerateMana);

			//coroutineObject = new CoroutineObject(entity, Wait);
		}
		private void Add(float temp) {
			effects.Add(listenerRegenerateMana);
		}
		private IEnumerator Wait() {
			while (true) {
				if (effects.Count == 0) break;
				yield return null;
				for(int i =0; i< effects.Count; i++) {

				}
			}
		}
	}
}