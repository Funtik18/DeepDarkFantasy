using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.Character.Perks {
    public class TextPerk : MonoBehaviour {
        public Button button;
		public Image image;
		public TMPro.TextMeshProUGUI txt;
		public TMPro.TextMeshProUGUI txtAmount;
		public Perk perk;


		public void UpdateText( string dop = "" ) {
			txt.text = "";
			txtAmount.text = "";

			string[] txts = perk.Output().Split('|');
			txt.text = txts[0];

			txtAmount.text = txts[1];
		}
	}
}