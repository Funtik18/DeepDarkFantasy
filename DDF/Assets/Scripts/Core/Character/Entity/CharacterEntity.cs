﻿using UnityEngine;
using DDF.UI.Bar;
using System.Collections.Generic;
using DDF.Inputs;
using DDF.UI.GUI;
using DDF.UI.Inventory;

/// <summary>
/// Note: если onChange руглярка то нужно апдейтить дату по новой, тк current
/// </summary>
namespace DDF.Character.Stats {
    public class CharacterEntity : Entity {

        [SerializeField]
        private CanvasGroup GUINavigator;

        [SerializeField]
        private NavigationBar navigation;

        [SerializeField]
        private List<TextsStats> textsStats;

        [SerializeField]
        private HealthBar HPBar;
        [SerializeField]
        private ManaBar MPBar;
        [SerializeField]
        private LevelBar LevelBar;


        #region Setup
        protected override void Awake() {
            base.Awake();
			foreach (var item in textsStats) {
                item.Init(statsRef);
            }
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
            CloseGUI();
        }
		private void Update() {
			if (Input.GetButtonDown(InputManager.ButtonInventoryPage)) {
                if (GameProcess.State == GameState.stream) {
                    OpenGUI(navigation.startPage);
                }else if (GameProcess.State == GameState.pause) {
                    CloseGUI();
                }
            }
		}
		private void UpdateTXT() {
            foreach (var item in textsStats) {
                item.UpdateAllTXT();
            }
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
		#endregion

        private void OpenGUI(int pageId) {
            GameProcess.Pause();
            ChoosePage(pageId);
            Help.HelpFunctions.CanvasGroupSeer.EnableGameObject(GUINavigator, true);
            InventoryOverSeerGUI._instance.Show();
        }
        private void CloseGUI() {
            InventoryOverSeerGUI._instance.Hide();
            Help.HelpFunctions.CanvasGroupSeer.DisableGameObject(GUINavigator);
            GameProcess.Resume();
        }

        private void ChoosePage(int pageId) {
            navigation.SetCurrentPage(pageId);
        }


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