﻿using DDF.UI;
using UnityEngine;

namespace DDF.Environment {
    public class CaseInteraction : MonoBehaviour {
        [SerializeField]
        private Interaction interaction;
        [SerializeField]
        private HintInteraction hint;

        private void Awake() {
            interaction.currentEventEnter.AddListener(delegate { OpenCase(); });
            interaction.currentEventStay.AddListener(delegate { StayCase(); });
            interaction.currentEventExit.AddListener(delegate { CloseCase(); });
        }
        
		public void OpenCase() {
            hint.OpenHint();
        }
        public void StayCase() {
            hint.LookAtCamera(Camera.main);
		}
        public void CloseCase() {
            hint.CloseHint();
        }
    }

}
