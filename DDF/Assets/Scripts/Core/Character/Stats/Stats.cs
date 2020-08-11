using DDF.Atributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    public class Stats : MonoBehaviour {
        [Header("Статы")]

        #region Здоровье
        [SerializeField] private Stat HP;
        private float baseHP = 10;
        private float maxHP;
        public float MaxHP {
            get {
                return maxHP;
            }
            set {
                maxHP = value;
                if (maxHP <= 0) maxHP = 0;
                if (maxHP < CurrentHP) CurrentHP = maxHP;
                onChangeHP?.Invoke();
            }
        }// максимально возможное количесвто хп
        [SerializeField]
        [ReadOnly]
        private float currentHP;
        public float CurrentHP {
            get {
                return currentHP;
            }
            set {
                currentHP = value;
                if (currentHP >= MaxHP) currentHP = MaxHP;
                if (currentHP <= 0) currentHP = 0;
                onChangeHP?.Invoke();
            }
        }//теукщее

        public Action onChangeHP;
        #endregion

        #region Мана
        [SerializeField] private Stat MP;
        private float baseMP = 5;
        private float maxMP;
        public float MaxMP {
            get {
                return maxMP;
            }
            set {
                maxMP = value;
                if (maxMP <= 0) maxMP = 0;
                if (maxMP < CurrentMP) CurrentMP = maxHP;
                onChangeMP?.Invoke();
            }
        }
        [SerializeField]
        [ReadOnly]
        private float currentMP;
        public float CurrentMP {
            get {
                return currentMP;
            }
            set {
                currentMP = value;
                if (currentMP >= maxMP) currentMP = maxMP;
                if (currentMP <= 0) currentMP = 0;
                onChangeMP?.Invoke();
            }
        }

        public Action onChangeMP;
        #endregion

        private int statMin = 1;

        #region Сила
        [SerializeField] private Stat strength;
        [ReadOnly] public int currentStrength;
        public int Strength {
            get {
                return currentStrength;
            }
            set {
                currentStrength = value;
                if (currentStrength < statMin) currentStrength = statMin;
                onChangeStrength?.Invoke();
            }
        }

        public Action onChangeStrength;
        #endregion
        #region Ловкость
        [SerializeField] private Stat agility;
        [ReadOnly] public int currentAgility;
        public int Agility {
            get {
                return currentAgility;
            }
            set {
                currentAgility = value;
                if (currentAgility < statMin) currentAgility = statMin;
                onChangeAgility?.Invoke();
            }
        }

        public Action onChangeAgility;
        #endregion
        #region Интелект
        [SerializeField] private Stat intelligence;
        [ReadOnly] public int currentIntelligence;
        public int Intelligence {
            get {
                return currentIntelligence;
            }
            set {
                currentIntelligence = value;
                if (currentIntelligence < statMin) currentIntelligence = statMin;
                onChangeIntelligance?.Invoke();
            }
        }

        public Action onChangeIntelligance;
        #endregion

        private float basemeleeDamage = 5;
        public float meleeDamage;

        private float baseavoid = 5;
        private float avoid;

        private float basespeed = 5;
        public float speed;


        #region Stats
        #region HP
        public virtual void TakeDamage( float dmg ) {
            CurrentHP -= dmg;
        }
        public virtual void RestoreHealth( float heal ) {
            CurrentHP += heal;
        }
        #endregion
        #region MP
        public virtual void SpendMana( float count ) {
            CurrentMP -= count;
        }
        public virtual void RestoreMana( float mana ) {
            CurrentMP += mana;
        }
        #endregion

        public void IncreaceStrength() {
            Strength++;
            UpdateStats();
        }
        public void DecreaceStrength() {
            Strength--;
            UpdateStats();
        }

        public void IncreaceAgility() {
            Agility++;
            UpdateStats();
        }
        public void DecreaceAgility() {
            Agility--;
            UpdateStats();
        }

        public void IncreaceIntelligence() {
            Intelligence++;
            UpdateStats();
        }
        public void DecreaceIntelligence() {
            Intelligence--;
            UpdateStats();
        }
		#endregion


		protected virtual void UpdateStats() {
            MakeFormules();
            SetData();
        }
        protected virtual void MakeFormules() {
            MaxHP = baseHP + Strength * 2;
            MaxMP = baseMP + Intelligence * 2;
            meleeDamage = basemeleeDamage + Strength * 2;
            avoid = baseavoid + Agility / 2;
            speed = basespeed + Agility / 2;
        }


        protected void GetData() {
            currentHP = MaxHP = HP.amount;
            currentMP = MaxMP = MP.amount;
            currentStrength = (int)strength.amount;
            currentAgility = (int)agility.amount;
            currentIntelligence = (int)intelligence.amount;
        }
        protected void SetData() {
            HP.amount = maxHP;
            MP.amount = maxMP;
            strength.amount = currentStrength;
            agility.amount = currentAgility;
            intelligence.amount = currentIntelligence;
        }
        protected void SaveData() {

        }
        protected void LoadData() {

        }

        private void GetCopyStats() {
            HP = HP.GetStatCopy();
            MP = MP.GetStatCopy();
            strength = strength.GetStatCopy();
            agility = agility.GetStatCopy();
            intelligence = intelligence.GetStatCopy();
        }
    }
}