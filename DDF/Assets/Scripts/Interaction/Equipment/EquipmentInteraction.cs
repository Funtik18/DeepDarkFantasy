using DDF.Atributes;
using DDF.Character;
using DDF.UI;
using UnityEngine;

namespace DDF.Environment {
	public class EquipmentInteraction : Interaction {

		[ReadOnly] [SerializeField] protected Entity interactEntity;
		[SerializeField] private HintInteraction hint;
		public bool IsInField { get { return isInField; } }

		public override void OnTriggerEnter(Collider other) {
			interactEntity = other.transform.root.GetComponent<Entity>();
			if (interactEntity) {
				base.OnTriggerEnter(other);
				OnEnter();
			}
		}
		public override void OnTriggerStay(Collider other) {
			if (interactEntity) {
				base.OnTriggerStay(other);
				OnStay();
			}
		}

		public override void OnTriggerExit(Collider other) {
			if (interactEntity) {
				base.OnTriggerExit(other);
				OnExit();
				interactEntity = null;
			}
		}
		protected virtual void OnEnter() {
			hint.OpenHint();
		}
		protected virtual void OnStay() {
			hint.LookAtCamera(Camera.main);
		}
		protected virtual void OnExit() {
			hint.CloseHint();
		}
	}
}