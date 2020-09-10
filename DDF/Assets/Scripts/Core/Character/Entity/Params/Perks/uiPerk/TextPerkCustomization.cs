using DDF.UI.ScrollView;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.Character.Perks {
	public class TextPerkCustomization : MonoBehaviour {
        public Button button;
		public Image image;
		public TMPro.TextMeshProUGUI txt;
		public TMPro.TextMeshProUGUI txtAmount;
		
		public Perk perk;
		public TextPerkBufCustomization buff;

		public bool isDestoyObject = true;
		public bool isClickObject = true;


		public Action<TextPerkCustomization> onClickPerk;
		public Action onDestroyPerk;

		public ScrollViewManipulatorCustomization scrollView;

		private void Awake() {
			onDestroyPerk = DestroyPerk;
			button.onClick.AddListener(delegate { 
				if(isClickObject)
					onClickPerk?.Invoke(this);
				if(isDestoyObject)
					onDestroyPerk?.Invoke();
			});
		}
		public void DestroyPerk() {
			Help.HelpFunctions.TransformSeer.DestroyObject(buff.gameObject);
			scrollView.RemoveObject(gameObject);
			Help.HelpFunctions.TransformSeer.DestroyObject(gameObject);
		}
		
		public void UpdateText( string dop = "" ) {
			txt.text = "";
			txtAmount.text = "";

			string[] txts = perk.PerkNameAndCost().Split('|');
			txt.text = txts[0];

			txtAmount.text = txts[1];
		}

	}
}