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
            textsEffects = new List<TextEffect>();
        }
        public void UpdateAllTXT() {
            if (currentEffects.Count != textsEffects.Count) {
                CreateObjects(currentEffects);
            }
            for (int i = 0; i < textsEffects.Count; i++) {
                textsEffects[i].UpdateText();
            }
        }
        /// <summary>
        /// Обновляет существующий эффект ui.
        /// </summary>
        /// <param name="effect"></param>
        public void UpdateEffectTXT(Effect effect) {
            int index = currentEffects.IndexOf(effect);
            /*if(index == -1) {
                CreateEffect(effect);
                index = textsEffects.Count - 1;
            }*/
            if (index != -1)
                textsEffects[index].UpdateText();
        }
        public void RemoveEffect(Effect effect) {
            int index = currentEffects.IndexOf(effect);
            if(index != -1) {
                TextEffect textEffect = textsEffects[index];
                textsEffects.Remove(textEffect);
                Help.HelpFunctions.TransformSeer.DestroyObject(textEffect.gameObject);
            }
        }

        public void CreateEffect(Effect effect) {
            TextEffect newEffect = Help.HelpFunctions.TransformSeer.CreateObjectInParent(container, prefabEffect, "Effect-" + effect.effectName).GetComponent<TextEffect>();
            newEffect.SetTrack(effect);
            textsEffects.Add(newEffect);
        }
        private void CreateObjects(List<Effect> effects) {
            Help.HelpFunctions.TransformSeer.DestroyChildrenInParent(container);
            textsEffects.Clear();

            for (int i = 0; i < effects.Count; i++) {
                CreateEffect(effects[i]);
            }
        }
    }
}