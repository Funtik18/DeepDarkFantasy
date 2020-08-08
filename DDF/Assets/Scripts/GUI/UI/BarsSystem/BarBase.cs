using UnityEngine.UI;
using UnityEngine;

namespace DDF.UI.Bar {
    [RequireComponent(typeof(Image))]
    public class BarBase : MonoBehaviour{
        private Image imageBar;

		public float CurrentCount { 
			get { return imageBar.fillAmount; }
			set {
				if (value <= 0) value = 0;
				if (value >= 1) value = 1;
				imageBar.fillAmount = value;
			}
		}

		private void Awake() {
			imageBar = GetComponent<Image>();
		}

		protected void IncreaseOn(float val) {
			CurrentCount += val;
		}
		protected void DecreaseOn( float val ) {
			CurrentCount -= val;
		}
	}
}
