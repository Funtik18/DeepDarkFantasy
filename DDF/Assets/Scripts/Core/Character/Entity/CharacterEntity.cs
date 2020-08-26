﻿using UnityEngine;
using DDF.UI.Bar;
using System.Collections.Generic;
using DDF.UI.Inventory;
using DDF.Character.Effects;

namespace DDF.Character.Stats {
    /// <summary>
    /// Весь UI этой сущности.
    /// </summary>
    public class CharacterEntity : Entity {

        public static CharacterEntity _instance;


        [SerializeField]
        private List<TextsStats> textsStats;

        [SerializeField]
        private TextsEffects textsEffects;

        [SerializeField]
        private HealthBar HPBar;
        [SerializeField]
        private ManaBar MPBar;
        [SerializeField]
        private LevelBar LevelBar;


        #region Setup
        protected override void Awake() {
            _instance = this;
            base.Awake();
			foreach (var item in textsStats) {
                item.Init(this);
            }
            textsEffects.Init(currentEffects);
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
            InventoryOverSeerGUI._instance.CloseGUI();
            mainInventory = InventoryOverSeerGUI._instance.mainInventory;
            mainEquipment = InventoryOverSeerGUI._instance.mainEquipment;
        }
		
		
        protected override void UpdateStats() {
            base.UpdateStats();
            UpdateUI();
        }
        private void UpdateUI() {
            LevelBar.SetMaxValue(MaxLevelExperience);
            LevelBar.UpdateBar(CurrentLevelExperience);//надо будет менять на загружаемое значение

            HPBar.SetMaxValue(MaxHealthPoints);
            HPBar.UpdateBar(CurrentHealthPoints);//надо будет менять на загружаемое значение

            MPBar.SetMaxValue(MaxManaPoints);
            MPBar.UpdateBar(CurrentManaPoints);//надо будет менять на загружаемое значение

            UpdateTXT();
        }
        private void UpdateTXT() {
            foreach (var item in textsStats) {
                item.UpdateAllTXT();
            }
            textsEffects.UpdateAllTXT();
        }
        #endregion




        /*//Дополнительные пойнты для здоровья
        private float baseAHP = 0;
        private float maxAHP;
        public float MaxAHP {
            get {
                return maxAHP;
            }
            set {
                maxAHP = value;
                if (maxAHP <= 0) maxAHP = 0;
            }
        }
        [SerializeField]
        [ReadOnly]
        private float currentAHP;
        public float CurrentAHP {
            get {
                return currentAHP;
            }
            set {
                currentAHP = value;
                if (currentAHP >= MaxAHP) currentAHP = MaxAHP;
                if (currentAHP <= 0) currentAHP = 0;
            }
        }*/

        #region Stats
        #region Exp
        public override void IncreaseLevelExperience( int count ) {
            base.IncreaseLevelExperience(count);
            LevelBar.UpdateBar(CurrentLevelExperience);
        }
        public override void DecreaseLevelExperience( int count ) {
            base.DecreaseLevelExperience(count);
            LevelBar.UpdateBar(CurrentLevelExperience);
        }
        #endregion
        #region HP
        public override void TakeDamage( float dmg ) {
            base.TakeDamage(dmg);
            HPBar.UpdateBar(CurrentHealthPoints);
        }
        public override void RestoreHealth( float heal ) {
            base.RestoreHealth(heal);
            HPBar.UpdateBar(CurrentHealthPoints);
        }
        #endregion
        #region MP
        public override void SpendMana( float count ) {
            base.SpendMana(count);
            MPBar.UpdateBar(CurrentManaPoints);
        }
        public override void RestoreMana( float mana ) {
            base.RestoreMana(mana);
            MPBar.UpdateBar(CurrentManaPoints);
        }
        #endregion
        #endregion
    }
}