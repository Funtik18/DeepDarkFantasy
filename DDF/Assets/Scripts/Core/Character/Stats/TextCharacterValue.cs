using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    public class TextCharacterValue : MonoBehaviour {

        public Value trackValue;

        public CharacterStats character;
        
        private void UpdateText() {

            string str = character.stats.GetText(trackValue);

            GetComponent<TMPro.TextMeshProUGUI>().text = str;

        }

		private void Start() {
            character.stats.SubscribeOnChange(UpdateText, trackValue);
            UpdateText();
        }
	}
}