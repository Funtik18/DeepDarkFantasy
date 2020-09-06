using UnityEngine;
using DDF.UI.Bar;
using System.Collections.Generic;
using DDF.UI.Inventory;
using DDF.Character.Effects;
using DDF.Character.Perks;
using DDF.Character.Stats;

namespace DDF.Character {
    /// <summary>
    /// Весь UI этой сущности.
    /// </summary>
    public class CharacterEntity : Entity {

        public static CharacterEntity _instance;


        [SerializeField]
        private List<TextsStats> textsStats;

        [SerializeField]
        private TextsPerks textsPerks;

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

            textsPerks.Init(currentPerks);

            textsEffects.Init(currentEffects);
            effects.effectOnStart += textsEffects.CreateEffect;
            effects.effectOnUpdate += textsEffects.UpdateEffectTXT;
            effects.effectOnDelete = textsEffects.RemoveEffect + effects.effectOnDelete;
        }

		protected void Start() {

            base.InitStartPerks();
            base.UpdatePerks();

            base.InitStartStats();
            base.UpdateStats();

            base.InitStartEffects();

            CurrentHealthPoints = MaxHealthPoints;
            CurrentManaPoints = MaxManaPoints;

            onChangePhysicalArmor = () => UpdateTXT();
            onChangeMeleeDamage = () => UpdateTXT();

            onChangeStrength = () => UpdateTXT();
            onChangeAgility = () => UpdateTXT();
            onChangeIntelligance = () => UpdateTXT();

            
            UpdateUI();
            InventoryOverSeerGUI.Getinstance().CloseGUI();
            mainInventory = InventoryOverSeerGUI.Getinstance().mainInventory;
            mainEquipment = InventoryOverSeerGUI.Getinstance().mainEquipment;
            mainEquipment.currentEntity = this;
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
            textsPerks.UpdateAllTXT();
        }
        #endregion
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