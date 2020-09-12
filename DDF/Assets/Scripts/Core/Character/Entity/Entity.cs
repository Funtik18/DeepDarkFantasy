using DDF.Atributes;
using DDF.Character.Effects;
using DDF.Character.Perks;
using DDF.UI.Inventory;
using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character {
    public class Entity : MonoBehaviour {
        public Stats.Stats stats;
        protected Perks.Perks perks;
        protected Abilities.Abilities abilities;
        protected Skills.Skills skills;
        protected Effects.Effects effects;

        [HideInInspector] public Inventory inventory;
        [HideInInspector] public Equipment equipment;

        /// <summary>
        /// Текущие перки.
        /// </summary>
        public List<Perk> currentPerks;
        /// <summary>
        /// Текущие-действующие эффекты.
        /// </summary>
        public List<Effect> currentEffects;

        protected virtual void Awake()
        {
            currentEffects = new List<Effect>();
            currentPerks = new List<Perk>();

            stats = new Stats.Stats();
            perks = new Perks.Perks(this);
            abilities = new Abilities.Abilities(stats);
            skills = new Skills.Skills(stats);
            effects = new Effects.Effects(this);

            stats.Init();
            perks.Init();
            abilities.Init();
            skills.Init();
        }

        protected void InitStartStats()
        {
            CurrentLevel = 1;
            MaxLevelExperience = 1000;
            CurrentSkillPoints = 5;

            CurrentSpeed = MaxSpeed;
        }
        protected void InitStartEffects()
        {
            effects.AddEffect("EffectRegenerateHealth");
        }
        protected void InitStartPerks()
        {
            perks.AddPerk("Talented");
            perks.AddPerk("Cupboard");
            //perks.AddPerk("Prompt");
            //perks.AddPerk("Savant");
        }

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] [ReadOnly] private bool isDead = false;
        public bool IsDead
        {
            get
            {
                return isDead;
            }
            set
            {
                isDead = value;
                if (isDead) stats.HealthPoints.currentInamount = 0;
                onChangeDead?.Invoke();
            }
        }
        public Action onChangeDead;

        /// <summary>
        /// Если true персонаж не может владеть магией
        /// </summary>
        [SerializeField] [ReadOnly] private bool isCastrat = false;
        public bool ISCastrat
        {
            get
            {
                return isCastrat;
            }
            set
            {
                isCastrat = value;
                if (isCastrat) MaxManaPoints = 0;
                onChangeCastrat?.Invoke();
            }
        }
        public Action onChangeCastrat;

        #region Уровень
        /// <summary>
        /// Текущий Уровень.
        /// </summary>
        public int CurrentLevel
        {
            get
            {
                return stats.Level.amount;
            }
            set
            {
                stats.Level.amount = value;
                if (stats.Level.amount < stats.levelMin) stats.Level.amount = stats.levelMin;
                onChangeLevel?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeLevel;
        #endregion
        #region Опыт
        /// <summary>
        /// Базовое значение для Опыта.
        /// </summary>
        private int baseLevelExperience = 0;
        /// <summary>
        /// Максимально возможное значение для Опыта.
        /// </summary>
        public int MaxLevelExperience
        {
            get
            {
                return stats.LevelExperience.amount;
            }
            set
            {
                stats.LevelExperience.amount = value;
                /*if (maxLevelExperience <= 0) maxLevelExperience = 0;
                if (maxLevelExperience < CurrentLevelExperience) CurrentLevelExperience = maxLevelExperience;
                onChangeLevelExperience?.Invoke();*/
            }
        }
        /// <summary>
        /// Текущее значение Опыта.
        /// </summary>
        public int CurrentLevelExperience
        {
            get
            {
                return stats.LevelExperience.currentInamount;
            }
            set
            {
                stats.LevelExperience.currentInamount = value;
                if (stats.LevelExperience.currentInamount >= MaxLevelExperience)
                {
                    IncreaseLevel();
                    CurrentLevelExperience = CurrentLevelExperience - stats.LevelExperience.amount;
                }
                if (stats.LevelExperience.currentInamount <= 0) stats.LevelExperience.currentInamount = 0;
                onChangeLevelExperience?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeLevelExperience;
        #endregion
        #region Очки навыков
        /// <summary>
        /// Базовое значение для Очков Навыков.
        /// </summary>
        private int baseSkillPoints = 10;
        /// <summary>
        /// Максимально возможное значение для Очков Навыков.
        /// </summary>
        public int MaxSkillPoints
        {
            get
            {
                return stats.SkillPoints.amount;
            }
            set
            {
                stats.SkillPoints.amount = value;
                if (stats.SkillPoints.amount <= 0) stats.SkillPoints.amount = 0;
                onChangeSkillPoints?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Очков Навыков.
        /// </summary>
        public int CurrentSkillPoints
        {
            get
            {
                return stats.SkillPoints.currentInamount;
            }
            set
            {
                stats.SkillPoints.currentInamount = value;
                if (stats.SkillPoints.currentInamount <= 0) stats.SkillPoints.currentInamount = 0;
                onChangeSkillPoints?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeSkillPoints;
        #endregion


        #region Здоровье
        /// <summary>
        /// Базовое значение для Здоровья.
        /// </summary>
        private float baseHealthPoints = 15;
        /// <summary>
        /// Максимально возможное значение для Здоровья.
        /// </summary>
        public float MaxHealthPoints
        {
            get
            {
                return stats.HealthPoints.amount;
            }
            set
            {
                stats.HealthPoints.amount = value;
                if (stats.HealthPoints.amount <= 0) stats.HealthPoints.amount = 0;
                if (stats.HealthPoints.amount <= CurrentHealthPoints) CurrentHealthPoints = stats.HealthPoints.amount;
                onChangeHealthPoints?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Здоровья.
        /// </summary>
        public float CurrentHealthPoints
        {
            get
            {
                return stats.HealthPoints.currentInamount;
            }
            set
            {
                stats.HealthPoints.currentInamount = value;
                if (stats.HealthPoints.currentInamount >= MaxHealthPoints) stats.HealthPoints.currentInamount = MaxHealthPoints;
                if (stats.HealthPoints.currentInamount <= 0) IsDead = true;
                onChangeHealthPoints?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeHealthPoints;
        #endregion
        #region Мана
        /// <summary>
        /// Базовое значене для Маны.
        /// </summary>
        private float baseManaPoints = 5;
        /// <summary>
        /// Максимально возможное значение для Маны.
        /// </summary>
        public float MaxManaPoints
        {
            get
            {
                return stats.ManaPoints.amount;
            }
            set
            {
                stats.ManaPoints.amount = value;
                if (stats.ManaPoints.amount <= 0) stats.ManaPoints.amount = 0;
                if (isCastrat) stats.ManaPoints.amount = 0;
                if (stats.ManaPoints.amount < CurrentManaPoints) CurrentManaPoints = stats.ManaPoints.amount;
                onChangeManaPoints?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Маны.
        /// </summary>
        public float CurrentManaPoints
        {
            get
            {
                return stats.ManaPoints.currentInamount;
            }
            set
            {
                stats.ManaPoints.currentInamount = value;
                if (stats.ManaPoints.currentInamount >= MaxManaPoints) stats.ManaPoints.currentInamount = MaxManaPoints;
                if (stats.ManaPoints.currentInamount <= 0) stats.ManaPoints.currentInamount = 0;
                onChangeManaPoints?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeManaPoints;
        #endregion
        #region Физическая броня
        /// <summary>
        /// Базовое значение Физической Брони.
        /// </summary>
        private float basePhysicalArmor = 0;
        /// <summary>
        /// Текущее значение Физической Брони.
        /// </summary>
        public float CurrentPhysicalArmor {
            get {
                return stats.PhysicalArmor.amount;
            }
            set {
                stats.PhysicalArmor.amount = value;
                if (stats.PhysicalArmor.amount <= 0) stats.PhysicalArmor.amount = 0;
                onChangePhysicalArmor?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangePhysicalArmor;
        #endregion
        #region Магическая броня
        /// <summary>
        /// Базовое значаение Магической Брони.
        /// </summary>
        private int baseMagicArmor = 5;
        /// <summary>
        /// Максимально возможное значение для Магической Брони.
        /// </summary>
        public int MaxMagicArmor
        {
            get
            {
                return stats.MagicArmor.amount;
            }
            set
            {
                stats.MagicArmor.amount = value;
                if (stats.MagicArmor.amount <= 0) stats.MagicArmor.amount = 0;
                if (stats.MagicArmor.amount < CurrentMagicArmor) CurrentMagicArmor = stats.MagicArmor.amount;
                onChangeMagicArmor?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Магической Брони.
        /// </summary>
        public int CurrentMagicArmor
        {
            get
            {
                return stats.MagicArmor.currentInamount;
            }
            set
            {
                stats.MagicArmor.currentInamount = value;
                if (stats.MagicArmor.currentInamount >= MaxMagicArmor) stats.MagicArmor.currentInamount = MaxMagicArmor;
                if (stats.MagicArmor.currentInamount <= 0) stats.MagicArmor.currentInamount = 0;
                onChangeMagicArmor?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeMagicArmor;
		#endregion

		#region Сопротивления
		#region Сопротивление к огню
		/// <summary>
		/// Базовое значение для Сопротивления к Огню.
		/// </summary>
		private float baseResistFire = 0;
        /// <summary>
        /// Максимально возможное значение для Сопротивления к Огню.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private float maxResistFire = 100;
        /// <summary>
        /// Максимально возможное значение для Сопротивления к Огню.
        /// </summary>
        public float MaxResistFire {
            get {
                return maxResistFire;
            }
        }
        /// <summary>
        /// Текущее значение Сопротивления к Огню.
        /// </summary>
        public float CurrentResistFire {
            get {
                return stats.ResistFire.amount;
            }
            set {
                stats.ResistFire.amount = value;
                if (stats.ResistFire.amount >= MaxResistFire) stats.ResistFire.amount = MaxResistFire;
                if (stats.ResistFire.amount <= 0) stats.ResistFire.amount = 0;
                onChangeResistFire?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeResistFire;
        #endregion
        #region Сопротивление к холоду
        /// <summary>
        /// Базовое значение для Сопротивления к Холоду.
        /// </summary>
        private float baseResistIce = 0;
        /// <summary>
        /// Максимально возможное значение для Сопротивления к Холоду.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private float maxResistIce = 100;
        /// <summary>
        /// Максимально возможное значение для Сопротивления к Холоду.
        /// </summary>
        public float MaxResistIce {
            get {
                return maxResistIce;
            }
        }
        /// <summary>
        /// Текущее значение Сопротивления к Холоду.
        /// </summary>
        public float CurrentResistIce {
            get {
                return stats.ResistIce.amount;
            }
            set {
                stats.ResistIce.amount = value;
                if (stats.ResistIce.amount >= MaxResistIce) stats.ResistIce.amount = MaxResistIce;
                if (stats.ResistIce.amount <= 0) stats.ResistIce.amount = 0;
                onChangeResistIce?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeResistIce;
        #endregion
        #region Сопротивление к яду
        /// <summary>
        /// Базовое значение для Сопротивления к Яду.
        /// </summary>
        private float baseResistPoison = 0;
        /// <summary>
        /// Максимально возможное значение для Сопротивления к Яду.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private float maxResistPoison = 100;
        /// <summary>
        /// Максимально возможное значение для Сопротивления к Яду.
        /// </summary>
        public float MaxResistPoison {
            get {
                return maxResistPoison;
            }
        }
        /// <summary>
        /// Текущее значение Сопротивления к Яду.
        /// </summary>
        public float CurrentResistPoison {
            get {
                return stats.ResistPoison.amount;
            }
            set {
                stats.ResistPoison.amount = value;
                if (stats.ResistPoison.amount >= MaxResistPoison) stats.ResistPoison.amount = MaxResistPoison;
                if (stats.ResistPoison.amount <= 0) stats.ResistPoison.amount = 0;
                onChangeResistPoison?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeResistPoison;
        #endregion
        #endregion

        #region Сила
        /// <summary>
        /// Текущая Сила.
        /// </summary>
        public int CurrentStrength
        {
            get
            {
                return stats.Strength.amount;
            }
            set
            {
                stats.Strength.amount = value;
                if (stats.Strength.amount < stats.statMin) stats.Strength.amount = stats.statMin;
                onChangeStrength?.Invoke();
            }
        }

        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeStrength;
        #endregion
        #region Ловкость
        /// <summary>
        /// Текущая Ловкость.
        /// </summary>
        public int CurrentAgility
        {
            get
            {
                return stats.Agility.amount;
            }
            set
            {
                stats.Agility.amount = value;
                if (stats.Agility.amount < stats.statMin) stats.Agility.amount = stats.statMin;
                onChangeAgility?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeAgility;
        #endregion
        #region Интелект
        /// <summary>
        /// Текущий Интелект.
        /// </summary>
        public int CurrentIntelligence
        {
            get
            {
                return stats.Intelligence.amount;
            }
            set
            {
                stats.Intelligence.amount = value;
                if (stats.Intelligence.amount < stats.statMin) stats.Intelligence.amount = stats.statMin;
                onChangeIntelligance?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeIntelligance;
        #endregion
        #region Удача
        /// <summary>
        /// Текущая Удача.
        /// </summary>
        public int CurrentLuck
        {
            get
            {
                return stats.Luck.amount;
            }
            set
            {
                stats.Luck.amount = value;
                if (stats.Luck.amount < stats.statMin) stats.Luck.amount = stats.statMin;
                onChangeLuck?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeLuck;
        #endregion

        #region Damage
        #region MeleeDamage
        /// <summary>
        /// Базовое значение для Урона в ближнем бою.
        /// </summary>
        private float baseMeleeDamage = 10;

        /// <summary>
        /// Максимально возможное значение Урона в ближнем бою.
        /// </summary>
        public float MaxMeleeDamage
        {
            get
            {
                return stats.MeleeDamage.amount;
            }
            set
            {
                stats.MeleeDamage.amount = value;
                if (stats.MeleeDamage.amount <= 0) stats.MeleeDamage.amount = 0;
                if (stats.MeleeDamage.amount < MinMeleeDamage) MinMeleeDamage = stats.MeleeDamage.amount;
                onChangeMeleeDamage?.Invoke();
            }
        }
        /// <summary>
        /// Минимальное значение Урона в ближнем бою.
        /// </summary>
        public float MinMeleeDamage
        {
            get
            {
                return stats.MeleeDamage.currentInamount;
            }
            set
            {
                stats.MeleeDamage.currentInamount = value;
                if (stats.MeleeDamage.currentInamount >= MaxMeleeDamage) stats.MeleeDamage.currentInamount = MaxMeleeDamage;
                if (stats.MeleeDamage.currentInamount <= 0) stats.MeleeDamage.currentInamount = 0;
                onChangeMeleeDamage?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeMeleeDamage;
        #endregion
        #region ShotDamage
        /// <summary>
        /// Базовое значение для Урона в дальнем бою.
        /// </summary>
        private float baseShotDamage = 10;
        /// <summary>
        /// Максимально возможное значение Урона в дальнем бою.
        /// </summary>
        public float MaxShotDamage
        {
            get
            {
                return stats.ShotDamage.amount;
            }
            set
            {
                stats.ShotDamage.amount = value;
                if (stats.ShotDamage.amount <= 0) stats.ShotDamage.amount = 0;
                if (stats.ShotDamage.amount < MinShotDamage) MinShotDamage = stats.ShotDamage.amount;
                onChangeShotDamage?.Invoke();
            }
        }
        /// <summary>
        /// Минимальное значение Урона в дальнем бою.
        /// </summary>
        public float MinShotDamage
        {
            get
            {
                return stats.ShotDamage.currentInamount;
            }
            set
            {
                stats.ShotDamage.currentInamount = value;
                if (stats.ShotDamage.currentInamount >= MaxShotDamage) stats.ShotDamage.currentInamount = MaxShotDamage;
                if (stats.ShotDamage.currentInamount <= 0) stats.ShotDamage.currentInamount = 0;
                onChangeShotDamage?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeShotDamage;
        #endregion
        #region MagicDamage
        /// <summary>
        /// Базовое значение для Магического Урона.
        /// </summary>
        private float baseMagicDamage = 0;
        /// <summary>
        /// Максимально возможное значение Магического Урона.
        /// </summary>
        public float MaxMagicDamage
        {
            get
            {
                return stats.MagicDamage.amount;
            }
            set
            {
                stats.MagicDamage.amount = value;
                if (stats.MagicDamage.amount <= 0) stats.MagicDamage.amount = 0;
                if (stats.MagicDamage.amount < MinMagicDamage) MinMagicDamage = stats.MagicDamage.amount;
                onChangeMagicDamage?.Invoke();
            }
        }
        /// <summary>
        /// Минимальное значение Магического Урона.
        /// </summary>
        public float MinMagicDamage
        {
            get
            {
                return stats.MagicDamage.currentInamount;
            }
            set
            {
                stats.MagicDamage.currentInamount = value;
                if (stats.MagicDamage.currentInamount >= MaxMagicDamage) stats.MagicDamage.currentInamount = MaxMagicDamage;
                if (stats.MagicDamage.currentInamount <= 0) stats.MagicDamage.currentInamount = 0;
                onChangeMagicDamage?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeMagicDamage;
        #endregion
        #endregion
        #region Шанс критического удара
        /// <summary>
        /// Базовое значение для Критического Удара.
        /// </summary>
        private float baseСhanceCriticalStrike = 0;
        /// <summary>
        /// Максимально возможное значение для Критического Удара.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private float maxСhanceCriticalStrike = 100;
        /// <summary>
        /// Максимально возможное значение для Критического Удара.
        /// </summary>
        public float MaxСhanceCriticalStrike
        {
            get
            {
                return maxСhanceCriticalStrike;
            }
        }
        /// <summary>
        /// Текущее значение Критического Удара.
        /// </summary>
        public float CurrentСhanceCriticalStrike
        {
            get
            {
                return stats.СhanceCriticalStrike.amount;
            }
            set
            {
                stats.СhanceCriticalStrike.amount = value;
                if (stats.СhanceCriticalStrike.amount >= MaxСhanceCriticalStrike) stats.СhanceCriticalStrike.amount = MaxСhanceCriticalStrike;
                if (stats.СhanceCriticalStrike.amount <= 0) stats.СhanceCriticalStrike.amount = 0;
                onChangeСhanceCriticalStrike?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeСhanceCriticalStrike;
        #endregion
        #region Шанс критического выстрела
        /// <summary>
        /// Базовое значение для Критического Выстрела.
        /// </summary>
        private float baseСhanceCriticalShot = 0;
        /// <summary>
        /// Максимально возможное значение для Критического Выстрела.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private float maxСhanceCriticalShot = 100;
        /// <summary>
        /// Максимально возможное значение для Критического Выстрела.
        /// </summary>
        public float MaxСhanceCriticalShot
        {
            get
            {
                return maxСhanceCriticalShot;
            }
        }
        /// <summary>
        /// Текущее значение Критического Выстрела.
        /// </summary>
        public float CurrentСhanceCriticalShot
        {
            get
            {
                return stats.СhanceCriticalShot.amount;
            }
            set
            {
                stats.СhanceCriticalShot.amount = value;
                if (stats.СhanceCriticalShot.amount >= MaxСhanceCriticalShot) stats.СhanceCriticalShot.amount = MaxСhanceCriticalShot;
                if (stats.СhanceCriticalShot.amount <= 0) stats.СhanceCriticalShot.amount = 0;
                onChangeСhanceCriticalShot?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeСhanceCriticalShot;
        #endregion

        #region Шанс Уклонения
        /// <summary>
        /// Базовое значение для Уклонения.
        /// </summary>
        private float baseChanceAvoid = 0;
        /// <summary>
        /// Максимально возможное значение для Уклонения.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private float maxChanceAvoid = 100;
        /// <summary>
        /// Максимально возможное значение для Уклонения
        /// </summary>
        public float MaxChanceAvoid
        {
            get
            {
                return maxChanceAvoid;
            }
        }
        /// <summary>
        /// Текущее значение Уклонения.
        /// </summary>
        public float CurrentChanceAvoid
        {
            get
            {
                return stats.ChanceAvoid.amount;
            }
            set
            {
                stats.ChanceAvoid.amount = value;
                if (stats.ChanceAvoid.amount >= MaxChanceAvoid) stats.ChanceAvoid.amount = MaxChanceAvoid;
                if (stats.ChanceAvoid.amount <= 0) stats.ChanceAvoid.amount = 0;
                onChangeChanceAvoid?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeChanceAvoid;
        #endregion

        #region Скорость
        /// <summary>
        /// Базовое значение для Скорости.
        /// </summary>
        private float baseSpeed = 5;
        /// <summary>
        /// Максимально возможное значение для Скорости.
        /// </summary>
        public float MaxSpeed
        {
            get
            {
                return stats.Speed.amount;
            }
            set
            {
                stats.Speed.amount = value;
                if (stats.Speed.amount <= 0) stats.Speed.amount = 0;
                if (stats.Speed.amount < CurrentSpeed) CurrentSpeed = stats.Speed.amount;
                onChangeSpeed?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Скорости.
        /// </summary>
        public float CurrentSpeed
        {
            get
            {
                return stats.Speed.currentInamount;
            }
            set
            {
                stats.Speed.currentInamount = value;
                if (stats.Speed.currentInamount >= MaxSpeed) stats.Speed.currentInamount = MaxSpeed;
                if (stats.Speed.currentInamount <= 0) stats.Speed.currentInamount = 0;
                onChangeSpeed?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeSpeed;
        #endregion

        #region Вес
        /// <summary>
        /// Базовое значение для Веса.
        /// </summary>
        private int baseWeight = 25;
        /// <summary>
        /// Максимально возможное значение для Веса.
        /// </summary>
        public int MaxWeight {
            get {
                return stats.Weight.amount;
            }
            set {
                stats.Weight.amount = value;
                if (stats.Weight.amount <= 0) stats.Weight.amount = 0;
                if (stats.Weight.amount < CurrentWeight) CurrentWeight = stats.Weight.amount;
                onChangeWeight?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Веса.
        /// </summary>
        public int CurrentWeight {
            get {
                return stats.Weight.currentInamount;
            }
            set {
                stats.Weight.currentInamount = value;
                if (stats.Weight.currentInamount >= MaxWeight) stats.Weight.currentInamount = MaxWeight;
                onChangeWeight?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeWeight;
        #endregion

        #region Functions
        /// <summary>
        /// Убить объект.
        /// </summary>
        public void Kill()
        {
            TakeDamage(CurrentHealthPoints);
            UpdateStats();
        }
        /// <summary>
        /// Возрадить объект.
        /// </summary>
        public void ReBorn()
        {
            if (!IsDead) return;
            IsDead = false;
            CurrentHealthPoints = 1;
            UpdateStats();
        }

        /// <summary>
        /// Отобрать умение владеть магией.
        /// </summary>
        public void Castrat()
        {
            ISCastrat = true;
            UpdateStats();
        }
        /// <summary>
        /// Вернуть владение магией.
        /// </summary>
        public void ReCastrat()
        {
            ISCastrat = false;
            UpdateStats();
        }

        /// <summary>
        /// Повысить уровень.
        /// </summary>
        public void IncreaseLevel()
        {
            CurrentLevel++;
            CurrentSkillPoints = CurrentSkillPoints + MaxSkillPoints;
            UpdateStats();
        }
        /// <summary>
        /// Понизить уровень.
        /// </summary>
        public void DecreaseLevel()
        {
            CurrentLevel--;
            UpdateStats();
        }

        /// <summary>
        /// Повысить опыт на count.
        /// </summary>
        /// <param name="count"></param>
        public virtual void IncreaseLevelExperience(int count)
        {
            CurrentLevelExperience += count;
            UpdateStats();
        }
        /// <summary>
        /// Понизить опыт на count.
        /// </summary>
        /// <param name="count"></param>
        public virtual void DecreaseLevelExperience(int count)
        {
            CurrentLevelExperience -= count;
            UpdateStats();
        }

        /// <summary>
        /// Повысить текущее количество очков навыков.
        /// </summary>
        public void IncreaseSkillPoints()
        {
            CurrentSkillPoints++;
        }
        /// <summary>
        /// Понизить текущее количество очков навыков.
        /// </summary>
        public void DecreaseSkillPoints()
        {
            CurrentSkillPoints--;
        }
        /// <summary>
        /// Есть ли ещё очки навыков.
        /// </summary>
        /// <returns></returns>
        public bool IsSkillPointsExist()
        {
            if (CurrentSkillPoints > 0) return true;
            return false;
        }

        #region HP
        /// <summary>
        /// Отнимает от текущего здоровья dmg.
        /// </summary>
        /// <param name="dmg"></param>
        public virtual void TakeDamage(float dmg)
        {
            CurrentHealthPoints -= dmg;
        }
        /// <summary>
        /// Востанавливает определёное количество здоровья.
        /// </summary>
        /// <param name="heal"></param>
        public virtual void RestoreHealth(float heal)
        {
            if (IsDead) return;
            CurrentHealthPoints += heal;
        }
        #endregion
        #region MP
        /// <summary>
        /// Отнимает от текущей маны count.
        /// </summary>
        /// <param name="count"></param>
        public virtual void SpendMana(float count)
        {
            CurrentManaPoints -= count;
        }
        /// <summary>
        /// Востанавливает определённое поличество маны
        /// </summary>
        /// <param name="mana"></param>
        public virtual void RestoreMana(float mana)
        {
            CurrentManaPoints += mana;

        }
        #endregion
        #region Strength
        /// <summary>
        /// Увеличение Силы на 1.
        /// </summary>
        public void IncreaseStrength()
        {
            if (!IsSkillPointsExist()) return;
            CurrentStrength++;
            DecreaseSkillPoints();
            UpdateStats();
        }
        /// <summary>
        /// Уменьшение Силы на 1.
        /// </summary>
        public void DecreaseStrength()
        {
            if (CurrentStrength == stats.statMin) return;
            if (!stats.Strength.IsCanDecreace) return;
            CurrentStrength--;
            IncreaseSkillPoints();
            UpdateStats();
        }
        #endregion
        #region Agility
        /// <summary>
        /// Увеличение Интелекта на 1.
        /// </summary>
        public void IncreaseAgility()
        {
            if (!IsSkillPointsExist()) return;
            CurrentAgility++;
            DecreaseSkillPoints();
            UpdateStats();
        }
        /// <summary>
        /// Уменьшение Ловкости на 1.
        /// </summary>
        public void DecreaseAgility()
        {
            if (CurrentAgility == stats.statMin) return;
            if (!stats.Agility.IsCanDecreace) return;
            CurrentAgility--;
            IncreaseSkillPoints();
            UpdateStats();
        }
        #endregion
        #region Intelligence
        /// <summary>
        /// Увеличение Интелекта на 1.
        /// </summary>
        public void IncreaseIntelligence()
        {
            if (!IsSkillPointsExist()) return;
            CurrentIntelligence++;
            DecreaseSkillPoints();
            UpdateStats();
        }
        /// <summary>
        /// Уменьшение Интелекта на 1.
        /// </summary>
        public void DecreaseIntelligence()
        {
            if (CurrentIntelligence == stats.statMin) return;
            if (!stats.Intelligence.IsCanDecreace) return;
            CurrentIntelligence--;
            IncreaseSkillPoints();
            UpdateStats();
        }
        #endregion
        #region Luck
        /// <summary>
        /// Увеличение Удачи на 1.
        /// </summary>
        public void IncreaseLuck()
        {
            if (!IsSkillPointsExist()) return;
            CurrentLuck++;
            DecreaseSkillPoints();
            UpdateStats();
        }
        /// <summary>
        /// Уменьшение Удачи на 1.
        /// </summary>
        public void DecreaseLuck()
        {
            if (CurrentStrength == stats.statMin) return;
            if (!stats.Luck.IsCanDecreace) return;
            CurrentLuck--;
            IncreaseSkillPoints();
            UpdateStats();
        }
        #endregion
        #region Damage
        public float GetMeleeDamage()
        {
            float dmg = UnityEngine.Random.Range(MinMeleeDamage, MaxMeleeDamage);
            return dmg;
        }
        public float GetShotDamage()
        {
            float dmg = UnityEngine.Random.Range(MinShotDamage, MaxShotDamage);
            return dmg;
        }
        public float GetMagicDamage()
        {
            float dmg = UnityEngine.Random.Range(MinMagicDamage, MaxMagicDamage);
            return dmg;
        }
        #endregion

        #endregion

        #region Actions
        public virtual bool Take( Item item, Inventory inventory ) {
            if(InventoryOverSeerGUI.GetInstance().mainInventory.AddItem(item, false) == false) {
                return false;
			}
            inventory?.DeleteItem(item);
            return true;
        }
        public virtual bool Equip( Item item, Inventory from ) {
            Item equipedItem = InventoryOverSeerGUI.GetInstance().mainEquipment.Equip(item, from);
            if (equipedItem == null) return false;
            from?.DeleteItem(item);
            return true;
        }
        public virtual void TakeOff( Item item, Inventory inventory ) {
            Item dropedItem = equipment.TakeOff(item, inventory);
            if (dropedItem == null) return;
            this.inventory.AddItem(dropedItem);
        }

        public virtual void Drink( ConsumableItem item, Inventory inventory ) {
            for (int i = 0; i < item.effects.Count; i++) {
                effects.AddEffect(Instantiate(item.effects[i]));
            }
            inventory.DeleteItem(item);
        }
        #endregion


        /// <summary>
        /// Пересчитывает-перерисовывает статы.
        /// </summary>
        protected virtual void UpdateStats() {
            MakeFormules();
        }
        protected virtual void UpdatePerks()
        {
            for (int i = 0; i < currentPerks.Count; i++)
            {
                currentPerks[i].Calculate();
            }
        }

        /// <summary>
        /// 1 считаем формулы.2 записываем всё в дату
        /// </summary>
        protected virtual void MakeFormules()
        {
            //formules
            //MaxSkillPoints = CurrentIntelligence;

            MaxHealthPoints = baseHealthPoints + CurrentStrength * 2;
            MaxManaPoints = baseManaPoints + CurrentIntelligence * 2;

            MaxMagicArmor = baseMagicArmor+CurrentIntelligence * 2;


            MaxMeleeDamage = baseMeleeDamage + CurrentStrength * 2;
            MinMeleeDamage = 0;

            MaxShotDamage = baseShotDamage + CurrentAgility * 2;
            MinShotDamage = 0;

            MaxMagicDamage = baseMagicDamage + CurrentIntelligence * 2;
            MinMagicDamage = 0;

            CurrentWeight = baseWeight + CurrentStrength * 25;

            CurrentСhanceCriticalStrike = (float)Math.Round(baseСhanceCriticalStrike+((float)CurrentStrength + (float)CurrentLuck )/2+ UnityEngine.Random.Range(0,10),3);
            CurrentСhanceCriticalShot = (float)Math.Round(baseСhanceCriticalShot + (((float)CurrentAgility + (float)CurrentLuck)) / 2 + UnityEngine.Random.Range(0, 10), 3);
            CurrentChanceAvoid = (float)Math.Round(baseChanceAvoid + (((float)CurrentAgility + (float)CurrentLuck)) / 2f + UnityEngine.Random.Range(0, 10), 3);

            MaxSpeed = baseSpeed + CurrentAgility / 2;
        }
    }
}