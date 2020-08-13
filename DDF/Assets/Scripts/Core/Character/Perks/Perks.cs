using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Perks {

    public class Perks {
        #region Perks
        protected Perk Savant;
        protected Perk Cupboard;
        #endregion

        protected Dictionary<string, Perk> perks;

        public Perks() {

            Savant = new PerkInt(Intelligence, 3);
            Cupboard = new PerkInt(Strength, 3);

            Add("Savant", Savant);
            Add("Cupboard", Cupboard);
        }
        private void Add(string perkName, Perk perk) {
            perks.Add(perkName, perk);
        }

        public Perk GetPerk(string perkName) {
            Perk perk;
            perks.TryGetValue(perkName, out perk);
            return perk;
        }
    }
}