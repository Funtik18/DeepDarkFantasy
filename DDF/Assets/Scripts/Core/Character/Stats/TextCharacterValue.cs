using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    public class TextCharacterValue : MonoBehaviour {

        public Value trackValue;

        public CharacterStats character;

        private TMPro.TextMeshProUGUI txt;

		private void Awake() {
            txt = GetComponent<TMPro.TextMeshProUGUI>();

        }

		private void Start() {
            character.stats.SubscribeOnChange(UpdateText, trackValue);
            UpdateText();
        }

        private void UpdateText() {

            string str = character.stats.GetText(trackValue);

            txt.text = str;
        }
    }
}