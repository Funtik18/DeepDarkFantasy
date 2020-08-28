using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Perks {
    public class Perks {

        private Entity entity;

        #region Perks
        public PerkComplex Talented;
        public PerkInt Cupboard;
        public PerkInt Prompt;
        public PerkInt Savant;
        #endregion
        private PerkInt increaceStrength;
        private PerkInt increaceAgility;
        private PerkInt increaceIntelligence;

        private PerkInt decreaceStrength;
        private PerkInt decreaceAgility;
        private PerkInt decreaceIntelligence;

        private Dictionary<string, Perk> mainPerks;

        public Perks(Entity newEntity) {
            mainPerks = new Dictionary<string, Perk>();
            entity = newEntity;
        }

        public void Init() {
            #region Perks помогаторы
            increaceStrength = new PerkInt(entity.stats.Strength.statName, entity.stats.Strength, 1, 0);
            increaceAgility = new PerkInt(entity.stats.Agility.statName, entity.stats.Agility, 1, 0);
            increaceIntelligence = new PerkInt(entity.stats.Intelligence.statName, entity.stats.Intelligence, 1, 0);

            decreaceStrength = new PerkInt(entity.stats.Strength.statName, entity.stats.Strength, -1, 0);
            decreaceAgility = new PerkInt(entity.stats.Agility.statName, entity.stats.Agility, -1, 0);
            decreaceIntelligence = new PerkInt(entity.stats.Intelligence.statName, entity.stats.Intelligence, -1, 0);
            #endregion


            Talented = new PerkComplex("Талантливый", new List<Perk>() { increaceStrength, increaceAgility, increaceIntelligence }, -3);
            Cupboard = new PerkInt("Шкаф", entity.stats.Strength, 3, -2);
            Prompt = new PerkInt("Проворный", entity.stats.Agility, 3, -2);
            Savant = new PerkInt("Савант", entity.stats.Intelligence, 3, -2);

            mainPerks.Add("Talented", Talented);
            mainPerks.Add("Cupboard", Cupboard);
            mainPerks.Add("Prompt", Prompt);
            mainPerks.Add("Savant", Savant);
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
        public virtual void AddPerk(string perkName) {
            entity.currentPerks.Add(GetPerk(perkName));
        }
        /// <summary>
        /// Добавляет перк текущей сущности.
        /// </summary>
        public virtual void AddPerk( Perk perk ) {
            entity.currentPerks.Add(perk);
        }
        /// <summary>
        /// Удаляет перк у текущей сущности.
        /// </summary>
        public virtual void RemovePerk( string perkName ) {
            entity.currentPerks.Remove(GetPerk(perkName));
        }
        /// <summary>
        /// Удаляет перк у текущей сущности.
        /// </summary>
        public virtual void RemovePerk( Perk perk ) {
            entity.currentPerks.Remove(perk);
        }
    }
}