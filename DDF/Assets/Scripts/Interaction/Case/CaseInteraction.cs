using DDF.Atributes;
using DDF.Character;
using DDF.UI;
using UnityEngine;

namespace DDF.Environment {
    public class CaseInteraction : Interaction {

        [ReadOnly] [SerializeField] protected Entity interactEntity;
        [SerializeField] private HintInteraction hint;
        public bool IsInField { get { return isInField; } }

        public override void OnTriggerEnter(Collider other) {
            interactEntity = other.transform.root.GetComponent<Entity>();
            if (interactEntity) {
                base.OnTriggerEnter(other);
                OpenCase();
            }
        }

        public override void OnTriggerStay(Collider other) {
            if (interactEntity) {
                base.OnTriggerStay(other);
                StayCase();
            }
        }

        public override void OnTriggerExit(Collider other) {
            if (interactEntity) {
                base.OnTriggerExit(other);
                CloseCase();
                interactEntity = null;
            }
        }

        public virtual void OpenCase() {
            hint.OpenHint();
        }
        public virtual void StayCase() {
            hint.LookAtCamera(Camera.main);
		}
        public virtual void CloseCase() {
            hint.CloseHint();
        }
    }
}
