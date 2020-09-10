using DDF.UI.ScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Perks {

    public class TextPerkBufCustomization : MonoBehaviour {
        public TMPro.TextMeshProUGUI txt;
        public TMPro.TextMeshProUGUI txtAmount;
        public Perk perk;

        public ScrollViewManipulatorCustomization scrollView;
        public void DestroyPerk() {
            scrollView.RemoveObject(gameObject);
            Help.HelpFunctions.TransformSeer.DestroyObject(gameObject);
        }

        public void UpdateText( string dop = "" ) {
            txt.text = "";
            txtAmount.text = "";

            string output = perk.PerkBuffs();

            if (output.Contains("\n")) {
                string[] txtsdop = output.Split('\n');
                for (int i = 0; i < txtsdop.Length; i++) {
                    string[] txts = txtsdop[i].Split('|');
                    txt.text += txtsdop[0] + "\n";
                    txtAmount.text += txtsdop[1] + "\n";
                }
            } else {
                string[] txts = output.Split('|');
                txt.text = txts[0];
                txtAmount.text = txts[1];
            }
        }
    }
}