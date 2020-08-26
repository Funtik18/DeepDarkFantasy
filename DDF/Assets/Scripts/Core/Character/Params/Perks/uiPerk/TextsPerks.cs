 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Perks {
    public class TextsPerks : MonoBehaviour {
        [SerializeField]
        private GameObject prefabPerk;
        [SerializeField]
        private Transform container;

        private List<Perk> currentPerks;
        private List<TextPerk> textsPerks;

        public void Init( List<Perk> newPerks ) {
             currentPerks = newPerks;
             textsPerks = new List<TextPerk>();
            CreateObjects();
        }
        public void UpdateAllTXT() {
            if(currentPerks.Count != textsPerks.Count) {
                CreateObjects();
            }
            for(int i = 0; i< textsPerks.Count; i++) {
                textsPerks[i].UpdateText();
            }
		}
        private void CreateObjects() {
            Help.HelpFunctions.TransformSeer.DestroyChildrenInParent(container);
            textsPerks.Clear();

            for (int i = 0; i < currentPerks.Count; i++) {
                TextPerk newPerk = Help.HelpFunctions.TransformSeer.CreateObjectInParent(container, prefabPerk, "Perk-" + currentPerks[i]).GetComponent<TextPerk>();
                newPerk.SetTrack(currentPerks[i]);
                textsPerks.Add(newPerk);
            }
        }
    }
}