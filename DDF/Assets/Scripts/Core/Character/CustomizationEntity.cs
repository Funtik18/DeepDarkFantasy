using DDF.UI.Bar;
using DDF.Character.Stats;

using UnityEngine;
using System.Collections.Generic;
using DDF.Character.Perks;

namespace DDF.Character.Stats {
    public class CustomizationEntity : Entity {
        [SerializeField]
        private List<TextsStats> textsStats;
        [SerializeField]
        private TextsPerks textsPerks;
        #region Setup
        protected override void Awake() {
            base.Awake();
			for (int i = 0; i < textsStats.Count; i++) {
                textsStats[i].Init(statsRef);
            }
            textsPerks.Init(perks.GetAllPerks());
        }
        protected override void Start() {
            base.Start();

            onChangeSkillPoints = () => UpdateTXT();


            onChangeHealthPoints = delegate { UpdateData(); UpdateTXT(); };
            onChangeManaPoints = delegate { UpdateData(); UpdateTXT(); };
            onChangeStrength = () => UpdateTXT();
            onChangeAgility = () => UpdateTXT();
            onChangeIntelligance = () => UpdateTXT();

            onChangePhysicalArmor = delegate { UpdateData(); UpdateTXT(); };
            onChangeMagicArmor = delegate { UpdateData(); UpdateTXT(); };


            base.UpdateStats();
            CurrentHealthPoints = MaxHealthPoints;
            CurrentManaPoints = MaxManaPoints;

            UpdateUI();
        }
        private void UpdateTXT() {
            for (int i = 0; i < textsStats.Count; i++) {
                textsStats[i].UpdateAllTXT();
            }
        }
        protected override void UpdateStats() {
            base.UpdateStats();
            UpdateUI();
        }
        private void UpdateUI() {
            //LevelBar.SetMaxValue(MaxLevelExperience);
            //LevelBar.UpdateBar(CurrentLevelExperience);//надо будет менять на загружаемое значение

            //HPBar.SetMaxValue(MaxHealthPoints);
            //HPBar.UpdateBar(CurrentHealthPoints);//надо будет менять на загружаемое значение

           // MPBar.SetMaxValue(MaxManaPoints);
            //MPBar.UpdateBar(CurrentManaPoints);//надо будет менять на загружаемое значение

            UpdateTXT();
        }
        #endregion
    }
}