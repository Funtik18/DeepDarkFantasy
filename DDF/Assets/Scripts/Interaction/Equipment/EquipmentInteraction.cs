using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Environment {
	public class EquipmentInteraction : MonoBehaviour {
		[SerializeField]
		private Interaction interaction;

		private void Awake() {
			interaction.currentEventEnter.AddListener(delegate { EnterInteraction(); });
			interaction.currentEventStay.AddListener(delegate { StayInteraction(); });
			interaction.currentEventExit.AddListener(delegate { ExitInteraction(); });
		}
		public virtual void EnterInteraction() {
		}
		public virtual void StayInteraction() {
		}
		public virtual void ExitInteraction() {
		}
	}
}