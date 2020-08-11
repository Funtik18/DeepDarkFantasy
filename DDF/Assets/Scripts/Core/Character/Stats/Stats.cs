using DDF.Atributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Character.Stats {
    public class Stats : MonoBehaviour {
        [Header("Статы")]

        public bool dead = false;
        public bool castrat = false;//не может владеть магией если true

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

            //запись ссылок статов для передачи
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
        private int baseSkillPoints = 10;
        [SerializeField]
        [ReadOnly]
        private int maxSkillPoints;
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
        [SerializeField]
        [ReadOnly]
        private int currentSkillPoints;
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
        private float baseHealthPoints = 10;
        [SerializeField] [ReadOnly]
        private float maxHealthPoints;
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
        }// максимально возможное количесвто хп
        [SerializeField] [ReadOnly]
        private float currentHealthPoints;
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
        }//теукщее
        /// <summary>
        /// Событие, если значение изменилось.
        /// </summary>
        public Action onChangeHealthPoints;
        #endregion
        #region Мана
        private float baseManaPoints = 5;
        [SerializeField] [ReadOnly]
        private float maxManaPoints;
        public float MaxManaPoints {
            get {
                return maxManaPoints;
            }
            set {
                maxManaPoints = value;
                if (maxManaPoints <= 0) maxManaPoints = 0;
                if (castrat) maxHealthPoints = 0;
                if (maxManaPoints < CurrentManaPoints) CurrentManaPoints = maxHealthPoints;
                onChangeManaPoints?.Invoke();
            }
        }
        [SerializeField] [ReadOnly]
        private float currentManaPoints;
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
        private int basePhysicalArmor = 0;
        [SerializeField]
        [ReadOnly]
        private int maxPhysicalArmor;
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
        [SerializeField]
        [ReadOnly]
        private int currentPhysicalArmor;
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
        private int baseMagicArmor = 0;
        [SerializeField]
        [ReadOnly]
        private int maxMagicArmor;
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
        [SerializeField]
        [ReadOnly]
        private int currentMagicArmor;
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


        private int statMin = 1;

        #region Сила
        [SerializeField] [ReadOnly]
        private int currentStrength;
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
        [SerializeField] [ReadOnly]
        private int currentAgility;
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
        [SerializeField] [ReadOnly]
        public int currentIntelligence;
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

        #region Stats
        #region HP
        public virtual void TakeDamage( float dmg ) {
            CurrentHealthPoints -= dmg;
        }
        public virtual void RestoreHealth( float heal ) {
            CurrentHealthPoints += heal;
        }
        #endregion
        #region MP
        public virtual void SpendMana( float count ) {
            CurrentManaPoints -= count;
        }
        public virtual void RestoreMana( float mana ) {
            CurrentManaPoints += mana;

        }
        #endregion

        public void IncreaceStrength() {
            CurrentStrength++;
            UpdateStats();
        }
        public void DecreaceStrength() {
            CurrentStrength--;
            UpdateStats();
        }

        public void IncreaceAgility() {
            CurrentAgility++;
            UpdateStats();
        }
        public void DecreaceAgility() {
            CurrentAgility--;
            UpdateStats();
        }

        public void IncreaceIntelligence() {
            CurrentIntelligence++;
            UpdateStats();
        }
        public void DecreaceIntelligence() {
            CurrentIntelligence--;
            UpdateStats();
        }
		#endregion


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