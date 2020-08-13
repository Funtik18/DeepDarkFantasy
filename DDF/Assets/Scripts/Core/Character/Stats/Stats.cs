using DDF.Atributes;
using DDF.Character.Perks;
using DDF.Randomizer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Character.Stats {
    public class Stats : MonoBehaviour {

        public Perks.Perks perks;

        //статы, лучше не использовать вне текущего класса.

        protected Stat Level;
        protected Stat LevelExperience;
        protected Stat SkillPoints;

        protected Stat HealthPoints;
        protected Stat ManaPoints;

        protected Stat Strength;
        protected Stat Agility;
        protected Stat Intelligence;

        protected Stat PhysicalArmor;
        protected Stat MagicArmor;

        protected Stat MeleeDamage;
        protected Stat ShotDamage;
        protected Stat MagicDamage;
        protected Stat ChanceAvoid;
        protected Stat Speed;
        protected Stat СhanceCriticalShot;
        protected Stat СhanceCriticalStrike;

        protected Dictionary<string, Tuple<Stat, UnityAction, UnityAction>> stats;

        protected virtual void Awake() {
            //инициализация статов
            Level = new StatInt("Текущий уровень", 1);
            LevelExperience = new StatRegularInt("Опыт", 0, 0);
            SkillPoints = new StatInt("Доступные очки", 0);

            HealthPoints = new StatRegularFloat("Жизненые силы", 0, 0);
            ManaPoints = new StatRegularFloat("Магическая энергия", 0, 0);

            Strength = new StatInt("Сила", 1);
            Agility = new StatInt("Ловкость", 1);
            Intelligence = new StatInt("Интелект", 1);

            PhysicalArmor = new StatRegularInt("Физическая броня", 0, 0);
            MagicArmor = new StatRegularInt("Магическая броня", 0, 0);

            MeleeDamage = new StatRegularFloat("Урон в ближнем бою", 0, 0, "-");
            ShotDamage = new StatRegularFloat("Урон в дальнем бою", 0, 0, "-");
            MagicDamage = new StatRegularFloat("Магический урон", 0, 0, "-");
            СhanceCriticalShot = new StatFloat("Шанс крит выстрела", 0, "%");
            СhanceCriticalStrike = new StatFloat("Шанс крит удара", 0, "%");
            ChanceAvoid = new StatFloat("Шанс уклонения", 0, "%");

            #region запись ссылок статов и некоторых функций для передачи
            stats = new Dictionary<string, Tuple<Stat, UnityAction, UnityAction>>();

            stats.Add("Level", new Tuple<Stat, UnityAction, UnityAction>(Level, null, null));
            stats.Add("LevelExperience", new Tuple<Stat, UnityAction, UnityAction>(LevelExperience, null, null));
            stats.Add("SkillPoints", new Tuple<Stat, UnityAction, UnityAction>(SkillPoints, null, null));
            
            stats.Add("HealthPoints", new Tuple<Stat, UnityAction, UnityAction>(HealthPoints, null, null));
            stats.Add("ManaPoints", new Tuple<Stat, UnityAction, UnityAction>(ManaPoints, null, null));
            
            stats.Add("Strength", new Tuple<Stat, UnityAction, UnityAction>(Strength, IncreaseStrength, DecreaseStrength));
            stats.Add("Agility", new Tuple<Stat, UnityAction, UnityAction>(Agility, IncreaseAgility, DecreaseAgility));
            stats.Add("Intelligence", new Tuple<Stat, UnityAction, UnityAction>(Intelligence, IncreaseIntelligence, DecreaseIntelligence));
            
            stats.Add("PhysicalArmor", new Tuple<Stat, UnityAction, UnityAction>(PhysicalArmor, null, null));
            stats.Add("MagicArmor", new Tuple<Stat, UnityAction, UnityAction>(MagicArmor, null, null));

            stats.Add("MeleeDamage", new Tuple<Stat, UnityAction, UnityAction>(MeleeDamage, null, null));
            stats.Add("ShotDamage", new Tuple<Stat, UnityAction, UnityAction>(ShotDamage, null, null));
            stats.Add("MagicDamage", new Tuple<Stat, UnityAction, UnityAction>(MagicDamage, null, null));
            stats.Add("СhanceCriticalStrike", new Tuple<Stat, UnityAction, UnityAction>(СhanceCriticalStrike, null, null));
            stats.Add("СhanceCriticalShot", new Tuple<Stat, UnityAction, UnityAction>(СhanceCriticalShot, null, null));
            stats.Add("ChanceAvoid", new Tuple<Stat, UnityAction, UnityAction>(ChanceAvoid, null, null));
            #endregion


            //init stats
            CurrentLevel = 1;
            MaxLevelExperience = 1000;
            CurrentSkillPoints = 5;

            CurrentStrength = (Strength as StatInt).amount;
            CurrentAgility = ( Agility as StatInt ).amount;
            CurrentIntelligence = ( Intelligence as StatInt ).amount;
        }
        protected virtual void Start() {
            //удаление не нужного
            //stats.Clear();
        }

        /// <summary>
        /// Минимально возможный уровень.
        /// </summary>
        private int levelMin = 1;
        /// <summary>
        /// Минимально возможное значаение стата.
        /// </summary>
        private int statMin = 1;

        /// <summary>
        /// 
        /// </summary>
        [Header("Смерть")]
        [SerializeField] [ReadOnly]
        private bool isDead = false;
        public bool IsDead {
            get {
                return isDead;
            }
            set {
                isDead = value;
                if (isDead) currentHealthPoints = 0;
                onChangeDead?.Invoke();
            }
        }
        public Action onChangeDead;


        /// <summary>
        /// Если true персонаж не может владеть магией
        /// </summary>
        [SerializeField] [ReadOnly]
        private bool isCastrat = false;
        public bool ISCastrat {
            get {
                return isCastrat;
            }
            set {
                isCastrat = value;
                if (isCastrat) MaxManaPoints = 0;
                onChangeCastrat?.Invoke();
            }
        }
        public Action onChangeCastrat;

        #region Уровень
        [Header("Уровень")]
        /// <summary>
        /// Текущий Уровень.
        /// </summary>
        [SerializeField] [ReadOnly]
        private int currentLevel;
        /// <summary>
        /// Текущий Уровень.
        /// </summary>
        public int CurrentLevel {
            get {
                return currentLevel;
            }
            set {
                currentLevel = value;
                if (currentLevel < levelMin) currentLevel = levelMin;
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
        [SerializeField] [ReadOnly]
        private int maxLevelExperience;
        /// <summary>
        /// Максимально возможное значение для Опыта.
        /// </summary>
        public int MaxLevelExperience {
            get {
                return maxLevelExperience;
            }
            set {
                maxLevelExperience = value;
                /*if (maxLevelExperience <= 0) maxLevelExperience = 0;
                if (maxLevelExperience < CurrentLevelExperience) CurrentLevelExperience = maxLevelExperience;
                onChangeLevelExperience?.Invoke();*/
            }
        }
        /// <summary>
        /// Текущее значение Опыта.
        /// </summary>
        [SerializeField] [ReadOnly]
        private int currentLevelExperience;
        /// <summary>
        /// Текущее значение Опыта.
        /// </summary>
        public int CurrentLevelExperience {
            get {
                return currentLevelExperience;
            }
            set {
                currentLevelExperience = value;
                if (currentLevelExperience >= MaxLevelExperience) {
                    IncreaseLevel();
                    CurrentLevelExperience = CurrentLevelExperience - maxLevelExperience;
                }
                if (currentLevelExperience <= 0) currentLevelExperience = 0;
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
        [SerializeField] [ReadOnly]
        private int maxSkillPoints;
        /// <summary>
        /// Максимально возможное значение для Очков Навыков.
        /// </summary>
        public int MaxSkillPoints {
            get {
                return maxSkillPoints;
            }
            set {
                maxSkillPoints = value;
                if (maxSkillPoints <= 0) maxSkillPoints = 0;
                onChangeSkillPoints?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Очков Навыков.
        /// </summary>
        [SerializeField] [ReadOnly]
        private int currentSkillPoints;
        /// <summary>
        /// Текущее значение Очков Навыков.
        /// </summary>
        public int CurrentSkillPoints {
            get {
                return currentSkillPoints;
            }
            set {
                currentSkillPoints = value;
                if (currentSkillPoints <= 0) currentSkillPoints = 0;
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
        private float baseHealthPoints = 10;
        [Header("Жизнь")]
        /// <summary>
        /// Максимально возможное значение для Здоровья.
        /// </summary>
        [SerializeField] [ReadOnly]
        private float maxHealthPoints;
        /// <summary>
        /// Максимально возможное значение для Здоровья.
        /// </summary>
        public float MaxHealthPoints {
            get {
                return maxHealthPoints;
            }
            set {
                maxHealthPoints = value;
                if (maxHealthPoints <= 0) maxHealthPoints = 0;
                if (maxHealthPoints < CurrentHealthPoints) CurrentHealthPoints = maxHealthPoints;
                onChangeHealthPoints?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Здоровья.
        /// </summary>
        [SerializeField] [ReadOnly]
        private float currentHealthPoints;
        /// <summary>
        /// Текущее значение Здоровья.
        /// </summary>
        public float CurrentHealthPoints {
            get {
                return currentHealthPoints;
            }
            set {
                currentHealthPoints = value;
                if (currentHealthPoints >= MaxHealthPoints) currentHealthPoints = MaxHealthPoints;
                if (currentHealthPoints <= 0) currentHealthPoints = 0;
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
        [SerializeField] [ReadOnly]
        private float maxManaPoints;
        /// <summary>
        /// Максимально возможное значение для Маны.
        /// </summary>
        public float MaxManaPoints {
            get {
                return maxManaPoints;
            }
            set {
                maxManaPoints = value;
                if (maxManaPoints <= 0) maxManaPoints = 0;
                if (isCastrat) maxManaPoints = 0;
                if (maxManaPoints < CurrentManaPoints) CurrentManaPoints = maxHealthPoints;
                onChangeManaPoints?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Маны.
        /// </summary>
        [SerializeField] [ReadOnly]
        private float currentManaPoints;
        /// <summary>
        /// Текущее значение Маны.
        /// </summary>
        public float CurrentManaPoints {
            get {
                return currentManaPoints;
            }
            set {
                currentManaPoints = value;
                if (currentManaPoints >= maxManaPoints) currentManaPoints = maxManaPoints;
                if (currentManaPoints <= 0) currentManaPoints = 0;
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
        private int basePhysicalArmor = 0;
        /// <summary>
        /// Максимально возможное значение для Физической брони.
        /// </summary>
        [Header("Броня")]
        [SerializeField] [ReadOnly]
        private int maxPhysicalArmor;
        /// <summary>
        /// Максимально возможное значение для Физической брони.
        /// </summary>
        public int MaxPhysicalArmor {
            get {
                return maxPhysicalArmor;
            }
            set {
                maxPhysicalArmor = value;
                if (maxPhysicalArmor <= 0) maxPhysicalArmor = 0;
                if (maxPhysicalArmor < CurrentPhysicalArmor) CurrentPhysicalArmor = maxPhysicalArmor;
                onChangePhysicalArmor?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Физической Брони.
        /// </summary>
        [SerializeField] [ReadOnly]
        private int currentPhysicalArmor;
        /// <summary>
        /// Текущее значение Физической Брони.
        /// </summary>
        public int CurrentPhysicalArmor {
            get {
                return currentPhysicalArmor;
            }
            set {
                currentPhysicalArmor = value;
                if (currentPhysicalArmor >= maxPhysicalArmor) currentPhysicalArmor = maxPhysicalArmor;
                if (currentPhysicalArmor <= 0) currentPhysicalArmor = 0;
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
        private int baseMagicArmor = 0;
        /// <summary>
        /// Максимально возможное значение для Магической Брони.
        /// </summary>
        [SerializeField] [ReadOnly]
        private int maxMagicArmor;
        /// <summary>
        /// Максимально возможное значение для Магической Брони.
        /// </summary>
        public int MaxMagicArmor {
            get {
                return maxMagicArmor;
            }
            set {
                maxMagicArmor = value;
                if (maxMagicArmor <= 0) maxMagicArmor = 0;
                if (maxMagicArmor < CurrentMagicArmor) CurrentMagicArmor = maxMagicArmor;
                onChangeMagicArmor?.Invoke();
            }
        }
        /// <summary>
        /// Текущее значение Магической Брони.
        /// </summary>
        [SerializeField] [ReadOnly]
        private int currentMagicArmor;
        /// <summary>
        /// Текущее значение Магической Брони.
        /// </summary>
        public int CurrentMagicArmor {
            get {
                return currentMagicArmor;
            }
            set {
                currentMagicArmor = value;
                if (currentMagicArmor >= maxMagicArmor) currentMagicArmor = maxMagicArmor;
                if (currentMagicArmor <= 0) currentMagicArmor = 0;
                onChangeMagicArmor?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeMagicArmor;
        #endregion

        
        [Header("Статы")]
        #region Сила
        /// <summary>
        /// Текущая Сила.
        /// </summary>
        [SerializeField] [ReadOnly]
        private int currentStrength;
        /// <summary>
        /// Текущая Сила.
        /// </summary>
        public int CurrentStrength {
            get {
                return currentStrength;
            }
            set {
                currentStrength = value;
                if (currentStrength < statMin) currentStrength = statMin;
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
        [SerializeField] [ReadOnly]
        private int currentAgility;
        /// <summary>
        /// Текущая Ловкость.
        /// </summary>
        public int CurrentAgility {
            get {
                return currentAgility;
            }
            set {
                currentAgility = value;
                if (currentAgility < statMin) currentAgility = statMin;
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
        [SerializeField] [ReadOnly]
        private int currentIntelligence;
        /// <summary>
        /// Текущий Интелект.
        /// </summary>
        public int CurrentIntelligence {
            get {
                return currentIntelligence;
            }
            set {
                currentIntelligence = value;
                if (currentIntelligence < statMin) currentIntelligence = statMin;
                onChangeIntelligance?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeIntelligance;
		#endregion

		#region Damage
		#region MeleeDamage
		/// <summary>
		/// Базовое значение для Урона в ближнем бою.
		/// </summary>
		private float baseMeleeDamage = 10;
        [Header("Урон")]
        /// <summary>
        /// Максимально возможное значение Урона в ближнем бою.
        /// </summary>
        [SerializeField] [ReadOnly]
        private float maxMeleeDamage;
        /// <summary>
        /// Максимально возможное значение Урона в ближнем бою.
        /// </summary>
        public float MaxMeleeDamage {
            get {
                return maxMeleeDamage;
            }
            set {
                maxMeleeDamage = value;
                if (maxMeleeDamage <= 0) maxMeleeDamage = 0;
                if (maxMeleeDamage < MinMeleeDamage) MinMeleeDamage = maxMeleeDamage;
                onChangeMeleeDamage?.Invoke();
            }
        }
        /// <summary>
        /// Минимальное значение Урона в ближнем бою.
        /// </summary>
        [SerializeField] [ReadOnly]
        private float minMeleeDamage;
        /// <summary>
        /// Минимальное значение Урона в ближнем бою.
        /// </summary>
        public float MinMeleeDamage {
            get {
                return minMeleeDamage;
            }
            set {
                minMeleeDamage = value;
                if (minMeleeDamage >= MaxMeleeDamage) minMeleeDamage = MaxMeleeDamage;
                if (minMeleeDamage <= 0) minMeleeDamage = 0;
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
        [SerializeField] [ReadOnly]
        private float maxShotDamage;
        /// <summary>
        /// Максимально возможное значение Урона в дальнем бою.
        /// </summary>
        public float MaxShotDamage {
            get {
                return maxShotDamage;
            }
            set {
                maxShotDamage = value;
                if (maxShotDamage <= 0) maxShotDamage = 0;
                if (maxShotDamage < MinShotDamage) MinShotDamage = maxShotDamage;
                onChangeShotDamage?.Invoke();
            }
        }
        /// <summary>
        /// Минимальное значение Урона в дальнем бою.
        /// </summary>
        [SerializeField] [ReadOnly]
        private float minShotDamage;
        /// <summary>
        /// Минимальное значение Урона в дальнем бою.
        /// </summary>
        public float MinShotDamage {
            get {
                return minShotDamage;
            }
            set {
                minShotDamage = value;
                if (minShotDamage >= MaxShotDamage) minShotDamage = MaxShotDamage;
                if (minShotDamage <= 0) minShotDamage = 0;
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
        [SerializeField] [ReadOnly]
        private float maxMagicDamage;
        /// <summary>
        /// Максимально возможное значение Магического Урона.
        /// </summary>
        public float MaxMagicDamage {
            get {
                return maxMagicDamage;
            }
            set {
                maxMagicDamage = value;
                if (maxMagicDamage <= 0) maxMagicDamage = 0;
                if (maxMagicDamage < MinMagicDamage) MinMagicDamage = maxMagicDamage;
                onChangeMagicDamage?.Invoke();
            }
        }
        /// <summary>
        /// Минимальное значение Магического Урона.
        /// </summary>
        [SerializeField] [ReadOnly]
        private float minMagicDamage;
        /// <summary>
        /// Минимальное значение Магического Урона.
        /// </summary>
        public float MinMagicDamage {
            get {
                return minMagicDamage;
            }
            set {
                minMagicDamage = value;
                if (minMagicDamage >= MaxMagicDamage) minMagicDamage = MaxMagicDamage;
                if (minMagicDamage <= 0) minMagicDamage = 0;
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
        public float MaxСhanceCriticalStrike {
            get {
                return maxСhanceCriticalStrike;
            }
        }
        /// <summary>
        /// Текущее значение Критического Удара.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private float currentСhanceCriticalStrike;
        /// <summary>
        /// Текущее значение Критического Удара.
        /// </summary>
        public float CurrentСhanceCriticalStrike {
            get {
                return currentСhanceCriticalStrike;
            }
            set {
                currentСhanceCriticalStrike = value;
                if (currentСhanceCriticalStrike >= MaxСhanceCriticalStrike) currentСhanceCriticalStrike = MaxСhanceCriticalStrike;
                if (currentСhanceCriticalStrike <= 0) currentСhanceCriticalStrike = 0;
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
        public float MaxСhanceCriticalShot {
            get {
                return maxСhanceCriticalShot;
            }
        }
        /// <summary>
        /// Текущее значение Критического Выстрела.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private float currentСhanceCriticalShot;
        /// <summary>
        /// Текущее значение Критического Выстрела.
        /// </summary>
        public float CurrentСhanceCriticalShot {
            get {
                return currentСhanceCriticalShot;
            }
            set {
                currentСhanceCriticalShot = value;
                if (currentСhanceCriticalShot >= MaxСhanceCriticalShot) currentСhanceCriticalShot = MaxСhanceCriticalShot;
                if (currentСhanceCriticalShot <= 0) currentСhanceCriticalShot = 0;
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
        [SerializeField] [ReadOnly]
        private float maxChanceAvoid = 100;
        /// <summary>
        /// Максимально возможное значение для Уклонения
        /// </summary>
        public float MaxChanceAvoid {
            get {
                return maxChanceAvoid;
            }
        }
        /// <summary>
        /// Текущее значение Уклонения.
        /// </summary>
        [SerializeField] [ReadOnly]
        private float currentChanceAvoid;
        /// <summary>
        /// Текущее значение Уклонения.
        /// </summary>
        public float CurrentChanceAvoid {
            get {
                return currentChanceAvoid;
            }
            set {
                currentChanceAvoid = value;
                if (currentChanceAvoid >= MaxChanceAvoid) currentChanceAvoid = MaxChanceAvoid;
                if (currentChanceAvoid <= 0) currentChanceAvoid = 0;
                onChangeChanceAvoid?.Invoke();
            }
        }
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeChanceAvoid;
		#endregion






		private float basespeed = 5;
        public float speed;

        #region Functions
        /// <summary>
        /// Убить объект.
        /// </summary>
        public void Kill() {
            IsDead = true;
            UpdateStats();
		}
        /// <summary>
        /// Возрадить объект.
        /// </summary>
        public void ReBorn() {
            if (!IsDead) return;
            IsDead = false;
            CurrentHealthPoints = 1;
            UpdateStats();
        }

        /// <summary>
        /// Отобрать умение владеть магией.
        /// </summary>
        public void Castrat() {
            ISCastrat = true;
            UpdateStats();
        }
        /// <summary>
        /// Вернуть владение магией.
        /// </summary>
        public void ReCastrat() {
            ISCastrat = false;
            UpdateStats();
        }
        
        /// <summary>
        /// Повысить уровень.
        /// </summary>
        public void IncreaseLevel() {
            CurrentLevel++;
            CurrentSkillPoints = CurrentSkillPoints + MaxSkillPoints;
            UpdateStats();
        }
        /// <summary>
        /// Понизить уровень.
        /// </summary>
        public void DecreaseLevel() {
            CurrentLevel--;
            UpdateStats();
        }

        /// <summary>
        /// Повысить опыт на count.
        /// </summary>
        /// <param name="count"></param>
        public virtual void IncreaseLevelExperience(int count) {
            CurrentLevelExperience += count;
            UpdateStats();
        }
        /// <summary>
        /// Понизить опыт на count.
        /// </summary>
        /// <param name="count"></param>
        public virtual void DecreaseLevelExperience( int count ) {
            CurrentLevelExperience -= count;
            UpdateStats();
        }

        /// <summary>
        /// Повысить текущее количество очков навыков.
        /// </summary>
        public void IncreaseSkillPoints() {
            CurrentSkillPoints++;
        }
        /// <summary>
        /// Понизить текущее количество очков навыков.
        /// </summary>
        public void DecreaseSkillPoints() {
            CurrentSkillPoints--;
        }
        /// <summary>
        /// Есть ли ещё очки навыков.
        /// </summary>
        /// <returns></returns>
        public bool IsSkillPointsExist() {
            if (CurrentSkillPoints > 0) return true;
            return false;
		}

		#region HP
		/// <summary>
		/// Отнимает от текущего здоровья dmg.
		/// </summary>
		/// <param name="dmg"></param>
		public virtual void TakeDamage( float dmg ) {
            CurrentHealthPoints -= dmg;
        }
        /// <summary>
        /// Востанавливает определёное количество здоровья.
        /// </summary>
        /// <param name="heal"></param>
        public virtual void RestoreHealth( float heal ) {
            if (IsDead) return;
            CurrentHealthPoints += heal;
        }
		#endregion
		#region MP
		/// <summary>
		/// Отнимает от текущей маны count.
		/// </summary>
		/// <param name="count"></param>
		public virtual void SpendMana( float count ) {
            CurrentManaPoints -= count;
        }
        /// <summary>
        /// Востанавливает определённое поличество маны
        /// </summary>
        /// <param name="mana"></param>
        public virtual void RestoreMana( float mana ) {
            CurrentManaPoints += mana;

        }
		#endregion
		#region Strength
		/// <summary>
		/// Увеличение Силы на 1.
		/// </summary>
		public void IncreaseStrength() {
            if (!IsSkillPointsExist()) return;
            CurrentStrength++;
            DecreaseSkillPoints();
            UpdateStats();
        }
        /// <summary>
        /// Уменьшение Силы на 1.
        /// </summary>
        public void DecreaseStrength() {
            if (CurrentStrength == statMin) return;
            CurrentStrength--;
            IncreaseSkillPoints();
            UpdateStats();
        }
		#endregion
		#region Agility
		/// <summary>
		/// Увеличение Интелекта на 1.
		/// </summary>
		public void IncreaseAgility() {
            if (!IsSkillPointsExist()) return;
            CurrentAgility++;
            DecreaseSkillPoints();
            UpdateStats();
        }
        /// <summary>
        /// Уменьшение Ловкости на 1.
        /// </summary>
        public void DecreaseAgility() {
            if (CurrentAgility == statMin) return;
            CurrentAgility--;
            IncreaseSkillPoints();
            UpdateStats();
        }
		#endregion
		#region Intelligence
		/// <summary>
		/// Увеличение Интелекта на 1.
		/// </summary>
		public void IncreaseIntelligence() {
            if (!IsSkillPointsExist()) return;
            CurrentIntelligence++;
            DecreaseSkillPoints();
            UpdateStats();
        }
        /// <summary>
        /// Уменьшение Интелекта на 1.
        /// </summary>
        public void DecreaseIntelligence() {
            if (CurrentIntelligence == statMin) return;
            CurrentIntelligence--;
            IncreaseSkillPoints();
            UpdateStats();
        }
		#endregion
		#region Damage
		public float GetMeleeDamage() {
            DDFRandom random = new DDFRandom();
            float dmg = random.RandomBetween(MinMeleeDamage, MaxMeleeDamage);
            return dmg;
        }
        public float GetShotDamage() {
            DDFRandom random = new DDFRandom();
            float dmg = random.RandomBetween(MinShotDamage, MaxShotDamage);
            return dmg;
        }
        public float GetMagicDamage() {
            DDFRandom random = new DDFRandom();
            float dmg = random.RandomBetween(MinMagicDamage, MaxMagicDamage);
            return dmg;
        }
		#endregion
		#endregion

		/// <summary>
		/// Пересчитывает-перерисовывает статы.
		/// </summary>
		protected virtual void UpdateStats() {
            MakeFormules();
        }

        /// <summary>
        /// 1 считаем формулы.2 записываем всё в дату
        /// </summary>
        protected virtual void MakeFormules() {
			//formules
			MaxSkillPoints = CurrentIntelligence;

			MaxHealthPoints = baseHealthPoints + CurrentStrength * 2;
			MaxManaPoints = baseManaPoints + CurrentIntelligence * 2;

			MaxMagicArmor = CurrentIntelligence * 2;


            MaxMeleeDamage = baseMeleeDamage + CurrentStrength * 2;
            MinMeleeDamage = 0;

            MaxShotDamage = baseShotDamage + CurrentAgility * 2;
            MinShotDamage = 0;

            MaxMagicDamage = baseMagicDamage + CurrentIntelligence * 2;
            MinMagicDamage = 0;

            CurrentСhanceCriticalStrike = (float)Math.Round(baseСhanceCriticalStrike + ( (float)CurrentStrength ) / 2, 3);
            CurrentСhanceCriticalShot= (float)Math.Round(baseСhanceCriticalShot + ( (float)CurrentAgility ) / 2, 3);
            CurrentChanceAvoid = (float)Math.Round(baseChanceAvoid + ( (float)CurrentAgility ) / 1.5f, 3);

			UpdateData();
		}

        /// <summary>
        /// Записывает все значения в дату.
        /// </summary>
        protected void UpdateData() {
            //cash
            StatInt currentLevel = ( (StatInt)Level );
            StatRegularInt currentLevelExperience = ( (StatRegularInt)LevelExperience );
            StatInt currentSkillPoints = ( (StatInt)SkillPoints );

            StatRegularFloat maxHealth = ( (StatRegularFloat)HealthPoints );
			StatRegularFloat maxMana = ( (StatRegularFloat)ManaPoints );

			StatInt currentStrength = ( (StatInt)Strength );
			StatInt currentAgility = ( (StatInt)Agility );
			StatInt currentIntelligence = ( (StatInt)Intelligence );

			StatRegularInt currentPhysicalArmor = ( (StatRegularInt)PhysicalArmor );
			StatRegularInt currentMagicArmor = ( (StatRegularInt)MagicArmor );

            StatRegularFloat currentMeleeDamage = ( (StatRegularFloat)MeleeDamage );
            StatRegularFloat currentShotDamage = ( (StatRegularFloat)ShotDamage );
            StatRegularFloat currentMagicDamage = ( (StatRegularFloat)MagicDamage );
            StatFloat currentСhanceCriticalStrike = ( (StatFloat)СhanceCriticalStrike );
            StatFloat currentСhanceCriticalShot = ( (StatFloat)СhanceCriticalShot );
            StatFloat currentChanceAvoid = ( (StatFloat)ChanceAvoid );

            //UpdateData
            currentLevel.amount = CurrentLevel;

            currentLevelExperience.amount = MaxLevelExperience;
            currentLevelExperience.currentInamount= CurrentLevelExperience;

            currentSkillPoints.amount = CurrentSkillPoints;

            maxHealth.amount = MaxHealthPoints;
			maxHealth.currentInamount = CurrentHealthPoints;

			maxMana.amount = MaxManaPoints;
			maxMana.currentInamount = CurrentManaPoints;

			currentStrength.amount = CurrentStrength;
			currentAgility.amount = CurrentAgility;
			currentIntelligence.amount = CurrentIntelligence;

			currentPhysicalArmor.amount = MaxPhysicalArmor;
			currentPhysicalArmor.currentInamount = CurrentPhysicalArmor;

			currentMagicArmor.amount = MaxMagicArmor;
			currentMagicArmor.currentInamount = CurrentMagicArmor;

            currentMeleeDamage.amount = MaxMeleeDamage;
            currentMeleeDamage.currentInamount = MinMeleeDamage;

            currentShotDamage.amount = MaxShotDamage;
            currentShotDamage.currentInamount = MinShotDamage;

            currentMagicDamage.amount = MaxMagicDamage;
            currentMagicDamage.currentInamount = MinMagicDamage;

            currentСhanceCriticalStrike.amount = CurrentСhanceCriticalStrike;

            currentСhanceCriticalShot.amount = CurrentСhanceCriticalShot;

            currentChanceAvoid.amount = CurrentChanceAvoid;
        }

        public void SaveStats() {
            StatsJSON file = new StatsJSON(this);
            file.SaveFile();
		}
        public void LoadStats() {
            StatsJSON file = new StatsJSON(this);
        }
    }
}