using System.Collections;
using System.Collections.Generic;
using DDF.UI;
using UnityEngine;

namespace DDF.Environment {
	public class EquipmentInteraction : MonoBehaviour {
		[SerializeField]
		private Interaction interaction;
		[SerializeField]
		private HintInteraction hint;

		private void Awake() {
			//for(int i = 0; i < interactions.Length; i++) {
			interaction.currentEventEnter.AddListener(delegate { EnterInteraction(); });
			interaction.currentEventStay.AddListener(delegate { StayInteraction(); });
			interaction.currentEventExit.AddListener(delegate { ExitInteraction(); });
			//}
		}
		public virtual void EnterInteraction() {
			hint.OpenHint();
		}
		public virtual void StayInteraction() {
			hint.LookAtCamera(Camera.main);
		}
		public virtual void ExitInteraction() {
			hint.CloseHint();
		}
	}
}