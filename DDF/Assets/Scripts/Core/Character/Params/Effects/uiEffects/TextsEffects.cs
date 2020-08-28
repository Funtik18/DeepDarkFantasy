using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Effects {
	public class TextsEffects : MonoBehaviour {
		[SerializeField]
		private GameObject prefabEffect;
		[SerializeField]
		private Transform container;

		private List<Effect> currentEffects;
		private List<TextEffect> textsEffects;

		public void Init(List<Effect> newCurrentEffects) {
			currentEffects = newCurrentEffects;
		}
        public void UpdateAllTXT() {
            if (currentEffects.Count != textsEffects.Count) {
                CreateObjects();
            }
            for (int i = 0; i < textsEffects.Count; i++) {
                textsEffects[i].UpdateText();
            }
        }
        private void CreateObjects() {
            Help.HelpFunctions.TransformSeer.DestroyChildrenInParent(container);
            textsEffects.Clear();

            for (int i = 0; i < currentEffects.Count; i++) {
                TextEffect newEffect = Help.HelpFunctions.TransformSeer.CreateObjectInParent(container, prefabEffect, "Effect-" + currentEffects[i]).GetComponent<TextEffect>();
                newEffect.SetTrack(currentEffects[i]);
                textsEffects.Add(newEffect);
            }
        }
    }
}