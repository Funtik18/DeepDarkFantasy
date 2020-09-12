using DDF.UI.ScrollView;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.Character.Perks {
    public class TextsPerksCustomization : MonoBehaviour {
        [SerializeField] private TextPerkCustomization textPerkPrefab;
        [SerializeField] private TextPerkBufCustomization textPerkBufPrefab;

        [SerializeField]
        private ScrollViewManipulatorCustomization scrollViewFrom;
        [SerializeField]
        private ScrollViewManipulatorCustomization scrollViewTo;
        [SerializeField]
        private ScrollViewManipulatorCustomization scrollViewBufTo;

        private List<TextPerkCustomization> textPerks;
        private List<TextPerkBufCustomization> textPerksBuf;

        private Dictionary<string, Perk> perks;

        public void Init( Dictionary<string, Perk> curperks ) {
            textPerks = new List<TextPerkCustomization>();
            textPerksBuf = new List<TextPerkBufCustomization>();
            perks = curperks;

            foreach (KeyValuePair<string, Perk> perk in perks) {
                TextPerkCustomization newTextPerk = CreatePerk(scrollViewFrom, perk.Value);
                if (newTextPerk == null) continue;

                textPerks.Add(newTextPerk);
            }
            UpdateAllTXT();
        }
        public void SetPerkParent( TextPerkCustomization textPerk ) {
            TextPerkCustomization newTextPerk = CreatePerk(scrollViewTo, textPerk.perk);
            if (newTextPerk == null) return;

            if (scrollViewFrom.isDestoyObjects) {
                textPerks.Remove(textPerk);
                textPerk.onDestroyPerk?.Invoke();
                scrollViewFrom.RemoveObject(textPerk.gameObject);
            }

            textPerks.Add(newTextPerk);
            newTextPerk.buff = CreateBuf(scrollViewBufTo, newTextPerk.perk);
            UpdateAllTXT();
        }
        private TextPerkCustomization CreatePerk( ScrollViewManipulatorCustomization parent, Perk perk) {
            if (parent.isNoDuplicate) {
                if (scrollViewTo.IsContains(perk.varName)) return null;
            }

            TextPerkCustomization newPerk = Help.HelpFunctions.TransformSeer.CreateObjectInParent(parent.content, textPerkPrefab.gameObject, perk.varName).GetComponent<TextPerkCustomization>();
            newPerk.perk = perk;
            newPerk.scrollView = parent;

            newPerk.isClickObject = parent.isClickObjects;
            newPerk.isDestoyObject = parent.isDestoyObjects;

            if(parent.isClickObjects) {
                newPerk.onClickPerk = SetPerkParent;
			} else {
                newPerk.onClickPerk = null;
            }
            parent.AddObject(newPerk.gameObject);
            return newPerk;
        }

        private TextPerkBufCustomization CreateBuf( ScrollViewManipulatorCustomization parent, Perk perk ) {
            if (parent.isNoDuplicate) {
                if (scrollViewTo.IsContains(perk.varName)) return null;
            }

            TextPerkBufCustomization newPerk = Help.HelpFunctions.TransformSeer.CreateObjectInParent(parent.content, textPerkBufPrefab.gameObject, perk.varName).GetComponent<TextPerkBufCustomization>();
            newPerk.perk = perk;
            newPerk.scrollView = parent;

            parent.AddObject(newPerk.gameObject);
            textPerksBuf.Add(newPerk);
            return newPerk;
        }
        public void UpdateAllTXT() {
            for (int i = 0; i < textPerks.Count; i++) {
                textPerks[i]?.UpdateText();
            }
            for (int i = 0; i < textPerksBuf.Count; i++) {
                textPerksBuf[i]?.UpdateText();
            }
        }

       /* private Tuple<Stat, UnityAction, UnityAction> GetStatByString( string id ) {
            Tuple<Stat, UnityAction, UnityAction> tuple;
            stats.TryGetValue(id, out tuple);
            return tuple;
        }*/

    }
}