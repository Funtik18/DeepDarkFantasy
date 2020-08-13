using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Perks {

    public class TextsPerks : MonoBehaviour {
        [SerializeField] private TextPerk textPerkPrefab;
        [SerializeField] private Transform parent;

        private List<TextPerk> textPerks;

        private Dictionary<string, Perk> perks;
        public void Init( Dictionary<string, Perk> curperks ) {
            textPerks = new List<TextPerk>();
            perks = curperks;

			foreach (KeyValuePair<string, Perk> perk in perks) {
                TextPerk newTextPerk = Help.HelpFunctions.TransformSeer.CreateObjectInParent(parent, textPerkPrefab.gameObject, "Perk-" + perk.Key).GetComponent<TextPerk>();
                newTextPerk.perk = perk.Value;
                textPerks.Add(newTextPerk);
            }

            UpdateAllTXT();
        }

        public void UpdateAllTXT() {
            for (int i = 0; i < textPerks.Count; i++) {
                textPerks[i]?.UpdateText();
            }
        }

       /* private Tuple<Stat, UnityAction, UnityAction> GetStatByString( string id ) {
            Tuple<Stat, UnityAction, UnityAction> tuple;
            stats.TryGetValue(id, out tuple);
            return tuple;
        }*/

    }
}