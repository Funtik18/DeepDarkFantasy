using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.Character.Perks {

    public class TextPerk : MonoBehaviour {
        [SerializeField]
        private Image image;

        [SerializeField]
        private TMPro.TextMeshProUGUI txt;

        private Perk currentPerk;

        public void SetTrack( Perk currPerk ) {
            currentPerk = currPerk;
        }

        public void UpdateText( string dop = "" ) {
            txt.text = currentPerk.perkName;
        }
    }
}