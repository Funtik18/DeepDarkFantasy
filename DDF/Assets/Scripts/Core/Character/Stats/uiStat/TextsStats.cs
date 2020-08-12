﻿using System;
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
        [SerializeField] private TextStat textStatPhysicalArmor;
        [SerializeField] private TextStat textStatMagicArmor;

        private List<TextStat> textsStats;
        private Dictionary<string, Tuple<Stat, UnityAction, UnityAction>> stats;
        public void Init( Dictionary<string, Tuple<Stat, UnityAction, UnityAction>> curstats ) {
            textsStats = new List<TextStat>();
            stats = curstats;

            //track
            textStatLevel.SetTrack(GetStatByString("Level"));
            textStatLevelExperience.SetTrack(GetStatByString("LevelExperience"));
            textStatSkillPoints.SetTrack(GetStatByString("SkillPoints"));

            textStatHealthPoints.SetTrack(GetStatByString("HealthPoints"));
            textStatManaPoints.SetTrack(GetStatByString("ManaPoints"));

            textStatStrength.SetTrack(GetStatByString("Strength"));
            textStatAgility.SetTrack(GetStatByString("Agility"));
            textStatIntelligence.SetTrack(GetStatByString("Intelligence"));

            textStatPhysicalArmor.SetTrack(GetStatByString("PhysicalArmor"));
            textStatMagicArmor.SetTrack(GetStatByString("MagicArmor"));

            //to list
            textsStats.Add(textStatLevel);
            textsStats.Add(textStatLevelExperience);
            textsStats.Add(textStatSkillPoints);

            textsStats.Add(textStatHealthPoints);
            textsStats.Add(textStatManaPoints);

            textsStats.Add(textStatStrength);
            textsStats.Add(textStatAgility);
            textsStats.Add(textStatIntelligence);

            textsStats.Add(textStatPhysicalArmor);
            textsStats.Add(textStatMagicArmor);
        }
        public void UpdateAllTXT() {
            for (int i = 0; i < textsStats.Count; i++) {
                textsStats[i].UpdateText();
            }
        }

        private Tuple<Stat, UnityAction, UnityAction> GetStatByString( string id ) {
            Tuple<Stat, UnityAction, UnityAction> tuple;
            stats.TryGetValue(id, out tuple);
            return tuple;
        }
    }
}