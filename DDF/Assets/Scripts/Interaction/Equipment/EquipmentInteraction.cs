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
				hint.OpenHint();
			}
		}
		public override void OnTriggerStay(Collider other) {
			if (interactEntity) {
				base.OnTriggerEnter(other);
				hint.LookAtCamera(Camera.main);
			}
		}
		public override void OnTriggerExit(Collider other) {
			if (interactEntity) {
				base.OnTriggerEnter(other);
				hint.CloseHint();
				interactEntity = null;
			}
		}
	}
}