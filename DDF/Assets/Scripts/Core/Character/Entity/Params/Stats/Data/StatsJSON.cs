using DDF.IO;
using System;
using System.IO;
using UnityEngine;


namespace DDF.Character.Stats {
    [Serializable]
    public class StatsJSON {
        public int level;
        public int levelExperience;
        public int skillPoints;
        public float healthPoints;
        public float manaPoints;
        public int strength;
        public int agility;
        public int intelligence;
        public int physicalArmor;
        public int magicArmor;

        public StatsJSON(Stats stats) {
            /*level = stats.Level.amount;
            levelExperience = stats.CurrentLevelExperience;
            skillPoints = stats.CurrentSkillPoints;
            healthPoints = stats.CurrentHealthPoints;
            manaPoints = stats.CurrentManaPoints;
            strength = stats.CurrentStrength;
            agility = stats.CurrentAgility;
            intelligence = stats.CurrentIntelligence;
            physicalArmor = stats.CurrentPhysicalArmor;
            magicArmor = stats.CurrentMagicArmor;*/
		}

        public void SaveFile() {
            Session session = new Session();
            FileManager.SaveFileJSON(session.GetSession(), this);
        }
    }
}