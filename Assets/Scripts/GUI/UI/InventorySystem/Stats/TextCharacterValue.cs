using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Inventory.Stats {

    public class TextCharacterValue : MonoBehaviour {

        public Value trackValue;

        public Character character;
        
        private void UpdateText() {

            string str = character.stats.GetText(trackValue);

            GetComponent<TMPro.TextMeshProUGUI>().text = str;

        }

		private void Start() {
            character.stats.Subscribe(UpdateText, trackValue);
            UpdateText();
        }
	}
}