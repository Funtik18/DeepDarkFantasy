using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.UI.Bar {
	[RequireComponent(typeof(CanvasGroup))]
    public class BarBase : MonoBehaviour{

		[Header("Colors")]
		public Color barColor = new Color32(187, 77, 62, 255);
		public Color backgroundColor = new Color32(127, 223, 192, 255);


		[SerializeField]
        private Image imageBar;
		[SerializeField]
		private Image imagebackground;
		protected CanvasGroup canvasGroup;

		protected float maxvalue = 100;

		protected float lastvalue = 0;

		/// <summary>
		/// События, если вышел за минимум.
		/// </summary>
		[HideInInspector] public UnityAction beyondMin;

		/// <summary>
		/// Событие, если преодалел максимум.
		/// </summary>
		[HideInInspector] public UnityAction overMax;

		/// <summary>
		/// Событие, если преодалел максимум повторно.
		/// </summary>
		[HideInInspector] public UnityAction overMaxLastMax;


		public float CurrentCount { 
			get { return imageBar.fillAmount; }
			set {
				if (value <= 0) value = 0;
				if (value >= 1) value = 1;
				imageBar.fillAmount = value;
			}
		}

		protected virtual void Awake() {
			canvasGroup = GetComponent<CanvasGroup>();
			imageBar.color = barColor;
			imagebackground.color = backgroundColor;
		}

		protected void IncreaseOn(float val) {
			CurrentCount += val;
		}
		protected void DecreaseOn( float val ) {
			CurrentCount -= val;
		}


		public virtual void SetMaxValue( float maximum ) {
			maxvalue = maximum;
		}

		public void UpdateBar( float currentValue ) {
			float normalized = 1 / ( maxvalue / currentValue );//в проеделах от 0 до 1
			CurrentCount = normalized;

			SeeForActions();
		}

		private void SeeForActions() {
			//...
			lastvalue = CurrentCount;
		}
	}
}
