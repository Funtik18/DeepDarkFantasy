using DDF.UI;
using UnityEngine;

namespace DDF.Environment {
    public class CaseInteraction : MonoBehaviour {
        [SerializeField]
        private Interaction interaction;
        [SerializeField]
        private HintInteraction hint;

        protected bool IsInField { get { return interaction.isInField; } }

        private void Awake() {
            interaction.currentEventEnter.AddListener(delegate { OpenCase(); });
            interaction.currentEventStay.AddListener(delegate { StayCase(); });
            interaction.currentEventExit.AddListener(delegate { CloseCase(); });
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
