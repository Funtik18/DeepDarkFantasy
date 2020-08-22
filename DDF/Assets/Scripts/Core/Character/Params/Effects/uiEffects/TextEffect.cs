using UnityEngine;
using UnityEngine.UI;

namespace DDF.Character.Effects {

    public class TextEffect : MonoBehaviour {
        [SerializeField]
        private Image icon;
        [SerializeField]
        private TMPro.TextMeshProUGUI txt;

        public Effect currentEffect;

        public void UpdateText(string dop = "") {
            icon.sprite = currentEffect.effectIcon;
            txt.text = currentEffect.Output();
        }
    }
}