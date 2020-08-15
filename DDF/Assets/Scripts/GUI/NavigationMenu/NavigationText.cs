using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.GUI {
    public class NavigationText : MonoBehaviour {
        public TMPro.TextMeshProUGUI txt;

        public Color currenColor;
        

        public void SetText(string newText, Color newColor) {
            txt.text = newText;
            currenColor = newColor;

            UpdtateText();
        }
        private void UpdtateText() {
            txt.color = currenColor;

        }
    }
}