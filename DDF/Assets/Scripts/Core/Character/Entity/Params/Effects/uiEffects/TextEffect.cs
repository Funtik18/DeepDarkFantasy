using UnityEngine;
using UnityEngine.UI;

namespace DDF.Character.Effects {

    public class TextEffect : MonoBehaviour {
        [SerializeField]
        private Image icon;
        [SerializeField]
        private TMPro.TextMeshProUGUI txt;

        private Effect currentEffect;

        public void SetTrack(Effect currEffect) {
            currentEffect = currEffect;
        }

        public void UpdateText(string dop = "") {
            icon.sprite = currentEffect.effectIcon;
            txt.text = currentEffect.Output();
        }
    }
}