using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Perks {
    public class Perks {

        public Entity entity;

        #region Perks
        protected PerkInt Savant;
        protected PerkInt Cupboard;
        #endregion

        private Dictionary<string, Perk> mainPerks;

        public Perks(Entity newEntity) {
            mainPerks = new Dictionary<string, Perk>();
            entity = newEntity;
        }

        public virtual void Init() {
            Savant = new PerkInt("Савант", entity.stats.Intelligence, 3, -2);
            Cupboard = new PerkInt("Шкаф", entity.stats.Strength, 3, -1);

            mainPerks.Add("Savant", Savant);
            mainPerks.Add("Cupboard", Cupboard);
        }
        private Perk GetPerk(string perkName) {
            Perk perk;
            mainPerks.TryGetValue(perkName, out perk);
            return perk;
		}
        public Dictionary<string, Perk> GetAllPerks() {
            return mainPerks;
		}




        /// <summary>
        /// Добавляет перк текущей сущности.
        /// </summary>
        /// <param name="perkName"></param>
        public virtual void AddPerk(string perkName) {
            entity.currentPerks.Add(GetPerk(perkName));
        }
        public virtual void AddPerk( Perk perk ) {
            entity.currentPerks.Add(perk);
        }
        public virtual void RemovePerk( string perkName ) {
            entity.currentPerks.Remove(GetPerk(perkName));
        }
        public virtual void RemovePerk( Perk perk ) {
            entity.currentPerks.Remove(perk);
        }

    }
}