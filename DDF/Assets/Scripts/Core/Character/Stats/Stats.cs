using DDF.Atributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Character.Stats {
    public class Stats : MonoBehaviour {

        /// <summary>
        /// 
        /// </summary>
        [Header("Статы")]
        //[SerializeField] [ReadOnly]
        public bool isDead = false;
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
        public bool isCastrat = false;
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

        //статы, лучше не использовать вне текущего класса.
        protected Stat HealthPoints;
        protected Stat ManaPoints;
        protected Stat Strength;
        protected Stat Agility;
        protected Stat Intelligence;
        protected Stat PhysicalArmor;
        protected Stat MagicArmor;

        protected Queue<Stat> stats;
        protected Queue<UnityAction> increaseActions;
        protected Queue<UnityAction> decreaseActions;
        protected virtual void Awake() {
            //инициализация статов
            HealthPoints = new StatRegularFloat("Жизненые силы", 0, 0);
            ManaPoints = new StatRegularFloat("Магическая энергия", 0, 0);

            Strength = new StatInt("Сила", 1);
            Agility = new StatInt("Ловкость", 1);
            Intelligence = new StatInt("Интелект", 1);

            PhysicalArmor = new StatRegularInt("Физическая броня", 0, 0);
            MagicArmor = new StatRegularInt("Магическая броня", 0, 0);


            stats = new Queue<Stat>();
            increaseActions = new Queue<UnityAction>();
            decreaseActions = new Queue<UnityAction>();

            //запись ссылок статов и некоторых функций для передачи
            stats.Enqueue(HealthPoints);
            increaseActions.Enqueue(null);
            decreaseActions.Enqueue(null);

            stats.Enqueue(ManaPoints);
            increaseActions.Enqueue(null);
            decreaseActions.Enqueue(null);

            stats.Enqueue(Strength);
            increaseActions.Enqueue(IncreaceStrength);
            decreaseActions.Enqueue(DecreaceStrength);

            stats.Enqueue(Agility);
            increaseActions.Enqueue(IncreaceAgility);
            decreaseActions.Enqueue(DecreaceAgility);

            stats.Enqueue(Intelligence);
            increaseActions.Enqueue(IncreaceIntelligence);
            decreaseActions.Enqueue(DecreaceIntelligence);

            stats.Enqueue(PhysicalArmor);
            increaseActions.Enqueue(null);
            decreaseActions.Enqueue(null);

            stats.Enqueue(MagicArmor);
            increaseActions.Enqueue(null);
            decreaseActions.Enqueue(null);
        }
        protected virtual void Start() {
            //удаление не нужного
            stats.Clear();
            increaseActions.Clear();
            decreaseActions.Clear();
        }

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
                if (maxSkillPoints < CurrentSkillPoints) CurrentSkillPoints = maxSkillPoints;
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
                if (currentSkillPoints >= MaxSkillPoints) currentSkillPoints = MaxSkillPoints;
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


        /// <summary>
        /// Минимально возможное значаение стата.
        /// </summary>
        private int statMin = 1;

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
		private float basemeleeDamage = 5;
        public float meleeDamage;
		#endregion

		private float baseavoid = 5;
        private float avoid;

        private float basespeed = 5;
        public float speed;

        //Functions
        public void Kill() {
            IsDead = true;
            UpdateStats();
		}
        public void ReBorn() {
            IsDead = false;
            RestoreHealth(1);
            UpdateStats();
        }
        public void Castrat() {
            ISCastrat = true;
            UpdateStats();
        }
        public void ReCastrat() {
            ISCastrat = false;
            UpdateStats();
        }


        #region Stats
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

        /// <summary>
        /// Увеличение Силы на 1.
        /// </summary>
        public void IncreaceStrength() {
            CurrentStrength++;
            UpdateStats();
        }
        /// <summary>
        /// Уменьшение Силы на 1.
        /// </summary>
        public void DecreaceStrength() {
            CurrentStrength--;
            UpdateStats();
        }

        /// <summary>
        /// Увеличение Интелекта на 1.
        /// </summary>
        public void IncreaceAgility() {
            CurrentAgility++;
            UpdateStats();
        }
        /// <summary>
        /// Уменьшение Ловкости на 1.
        /// </summary>
        public void DecreaceAgility() {
            CurrentAgility--;
            UpdateStats();
        }

        /// <summary>
        /// Увеличение Интелекта на 1.
        /// </summary>
        public void IncreaceIntelligence() {
            CurrentIntelligence++;
            UpdateStats();
        }
        /// <summary>
        /// Уменьшение Интелекта на 1.
        /// </summary>
        public void DecreaceIntelligence() {
            CurrentIntelligence--;
            UpdateStats();
        }
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
			MaxSkillPoints = baseSkillPoints + CurrentIntelligence * 2;

			MaxHealthPoints = baseHealthPoints + CurrentStrength * 2;
			MaxManaPoints = baseManaPoints + CurrentIntelligence * 2;

			MaxMagicArmor = CurrentIntelligence * 2;

			meleeDamage = basemeleeDamage + CurrentStrength * 2;
			avoid = baseavoid + CurrentAgility / 2;
			speed = basespeed + CurrentAgility / 2;

			UpdateData();
		}

        /// <summary>
        /// Записывает все значения в дату.
        /// </summary>
        protected void UpdateData() {
			//cash
			StatRegularFloat maxHealth = ( (StatRegularFloat)HealthPoints );
			StatRegularFloat maxMana = ( (StatRegularFloat)ManaPoints );

			StatInt currentStrength = ( (StatInt)Strength );
			StatInt currentAgility = ( (StatInt)Agility );
			StatInt currentIntelligence = ( (StatInt)Intelligence );

			StatRegularInt currentPhysicalArmor = ( (StatRegularInt)PhysicalArmor );
			StatRegularInt currentMagicArmor = ( (StatRegularInt)MagicArmor );


			//UpdateData
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
		}
	}
}