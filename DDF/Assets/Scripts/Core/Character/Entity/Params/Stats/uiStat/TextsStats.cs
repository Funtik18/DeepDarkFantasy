using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Character.Stats {
    public class TextsStats : MonoBehaviour {
        [Header("Level")]
        [SerializeField] private TextStat textStatLevel;
        [SerializeField] private TextStat textStatLevelExperience;
        [SerializeField] private TextStat textStatSkillPoints;
        [Header("Live")]
        [SerializeField] private TextStat textStatHealthPoints;
        [SerializeField] private TextStat textStatManaPoints;

        [Header("Resistance")]
        [SerializeField] private TextStat textStatResistFire;
        [SerializeField] private TextStat textStatResistIce;
        [SerializeField] private TextStat textStatResistPoison;


        [Header("Stats")]
        [SerializeField] private TextStat textStatStrength;
        [SerializeField] private TextStat textStatAgility;
        [SerializeField] private TextStat textStatIntelligence;
        [SerializeField] private TextStat textStatLuck;

        [Header("Armor")]
        [SerializeField] private TextStat textStatPhysicalArmor;
        [SerializeField] private TextStat textStatArmorHead;
        [SerializeField] private TextStat textStatArmorTorso;
        [SerializeField] private TextStat textStatArmorBelt;
        [SerializeField] private TextStat textStatArmorLegs;
        [SerializeField] private TextStat textStatArmorFeet;
        [SerializeField] private TextStat textStatMagicArmor;

        [Header("Chance")]
        [SerializeField] private TextStat textStatСhanceCriticalStrike;
        [SerializeField] private TextStat textStatСhanceCriticalShot;
        [SerializeField] private TextStat textStatСhanceAvoid;
        [Header("Damage")]
        [SerializeField] private TextStat textPrimaryHand;
        [SerializeField] private TextStat textSecondaryHand;
        [SerializeField] private TextStat textStatMeleeDamage;
        [SerializeField] private TextStat textStatShotDamage;
        [SerializeField] private TextStat textStatMagicDamage;

        private List<TextStat> textsStats;
        public void Init(HumanoidEntity currentEntity ) {
            Stats stats = currentEntity.stats;

            textsStats = new List<TextStat>();

            //track
            textStatLevel?.SetTrack(stats.Level);
            textStatLevelExperience?.SetTrack(stats.LevelExperience);
            textStatSkillPoints?.SetTrack(stats.SkillPoints);

            textStatHealthPoints?.SetTrack(stats.HealthPoints);
            textStatManaPoints?.SetTrack(stats.ManaPoints);

            textStatResistFire?.SetTrack(stats.ResistFire);
            textStatResistIce?.SetTrack(stats.ResistIce);
            textStatResistPoison?.SetTrack(stats.ResistPoison);

            textStatStrength?.SetTrack(stats.Strength, currentEntity.IncreaseStrength, currentEntity.DecreaseStrength);
            textStatAgility?.SetTrack(stats.Agility, currentEntity.IncreaseAgility, currentEntity.DecreaseAgility);
            textStatIntelligence?.SetTrack(stats.Intelligence, currentEntity.IncreaseIntelligence, currentEntity.DecreaseIntelligence);
            textStatLuck?.SetTrack(stats.Luck, currentEntity.IncreaseLuck, currentEntity.DecreaseLuck);

            textStatPhysicalArmor?.SetTrack(stats.PhysicalArmor);
            textStatArmorHead?.SetTrack(currentEntity.equipment.armorHead);
            textStatArmorTorso?.SetTrack(currentEntity.equipment.armorTorso);
            textStatArmorBelt?.SetTrack(currentEntity.equipment.armorBelt);
            textStatArmorLegs?.SetTrack(currentEntity.equipment.armorLegs);
            textStatArmorFeet?.SetTrack(currentEntity.equipment.armorFeet);

            textStatMagicArmor?.SetTrack(stats.MagicArmor);

            textPrimaryHand?.SetTrack(currentEntity.equipment.lHandDamage);
            textSecondaryHand?.SetTrack(currentEntity.equipment.rHandDamage);

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

            textsStats.Add(textStatResistFire);
            textsStats.Add(textStatResistIce);
            textsStats.Add(textStatResistPoison);

            textsStats.Add(textStatStrength);
            textsStats.Add(textStatAgility);
            textsStats.Add(textStatIntelligence);
            textsStats.Add(textStatLuck);

            textsStats.Add(textStatPhysicalArmor);
            textsStats.Add(textStatArmorHead);
            textsStats.Add(textStatArmorTorso);
            textsStats.Add(textStatArmorBelt);
            textsStats.Add(textStatArmorLegs);
            textsStats.Add(textStatArmorFeet);
            textsStats.Add(textStatMagicArmor);

            textsStats.Add(textPrimaryHand);
            textsStats.Add(textSecondaryHand);

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