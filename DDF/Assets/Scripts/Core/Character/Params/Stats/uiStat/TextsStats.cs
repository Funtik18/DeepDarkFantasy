using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Character.Stats {
    public class TextsStats : MonoBehaviour {
        [SerializeField] private TextStat textStatLevel;
        [SerializeField] private TextStat textStatLevelExperience;
        [SerializeField] private TextStat textStatSkillPoints;
        [SerializeField] private TextStat textStatHealthPoints;
        [SerializeField] private TextStat textStatManaPoints;
        [SerializeField] private TextStat textStatStrength;
        [SerializeField] private TextStat textStatAgility;
        [SerializeField] private TextStat textStatIntelligence;
        [SerializeField] private TextStat textStatLuck;
        [SerializeField] private TextStat textStatPhysicalArmor;
        [SerializeField] private TextStat textStatMagicArmor;
        [SerializeField] private TextStat textStatСhanceCriticalStrike;
        [SerializeField] private TextStat textStatСhanceCriticalShot;
        [SerializeField] private TextStat textStatСhanceAvoid;
        [SerializeField] private TextStat textStatMeleeDamage;
        [SerializeField] private TextStat textStatShotDamage;
        [SerializeField] private TextStat textStatMagicDamage;

        private List<TextStat> textsStats;
        public void Init( Entity currentEntity ) {
            Stats stats = currentEntity.stats;

            textsStats = new List<TextStat>();

            //track
            textStatLevel?.SetTrack(stats.Level);
            textStatLevelExperience?.SetTrack(stats.LevelExperience);
            textStatSkillPoints?.SetTrack(stats.SkillPoints);

            textStatHealthPoints?.SetTrack(stats.HealthPoints);
            textStatManaPoints?.SetTrack(stats.ManaPoints);

            textStatStrength?.SetTrack(stats.Strength, currentEntity.IncreaseStrength, currentEntity.DecreaseStrength);
            textStatAgility?.SetTrack(stats.Agility, currentEntity.IncreaseAgility, currentEntity.DecreaseAgility);
            textStatIntelligence?.SetTrack(stats.Intelligence, currentEntity.IncreaseIntelligence, currentEntity.DecreaseIntelligence);
            textStatLuck?.SetTrack(stats.Luck, currentEntity.IncreaseLuck, currentEntity.DecreaseLuck);

            textStatPhysicalArmor?.SetTrack(stats.PhysicalArmor);
            textStatMagicArmor?.SetTrack(stats.MagicArmor);

            textStatMeleeDamage?.SetTrack(stats.MeleeDamage);
            textStatShotDamage?.SetTrack(stats.ShotDamage);
            textStatMagicDamage?.SetTrack(stats.MagicDamage);

            textStatСhanceCriticalStrike?.SetTrack(stats.СhanceCriticalStrike);
            textStatСhanceCriticalShot?.SetTrack(stats.СhanceCriticalShot);

            textStatСhanceAvoid?.SetTrack(stats.ChanceAvoid);

            //to list
            textsStats.Add(textStatLevel);
            textsStats.Add(textStatLevelExperience);
            textsStats.Add(textStatSkillPoints);

            textsStats.Add(textStatHealthPoints);
            textsStats.Add(textStatManaPoints);

            textsStats.Add(textStatStrength);
            textsStats.Add(textStatAgility);
            textsStats.Add(textStatIntelligence);
            textsStats.Add(textStatLuck);

            textsStats.Add(textStatPhysicalArmor);
            textsStats.Add(textStatMagicArmor);

            textsStats.Add(textStatMeleeDamage);
            textsStats.Add(textStatShotDamage);
            textsStats.Add(textStatMagicDamage);
            textsStats.Add(textStatСhanceCriticalStrike);
            textsStats.Add(textStatСhanceCriticalShot);
            textsStats.Add(textStatСhanceAvoid);
        }
        public void UpdateAllTXT() {
            for (int i = 0; i < textsStats.Count; i++) {
                textsStats[i]?.UpdateText();
            }
        }
    }
}