using UnityEngine;

namespace DDF.Character.Stats {
    public class Stats {

		#region Stats
		[HideInInspector ]public StatInt Level;
        [HideInInspector] public StatRegularInt LevelExperience;
        [HideInInspector] public StatInt SkillPoints;

        [HideInInspector] public StatRegularFloat HealthPoints;
        [HideInInspector] public StatRegularFloat ManaPoints;

        [HideInInspector] public StatInt Strength;
        [HideInInspector] public StatInt Agility;
        [HideInInspector] public StatInt Intelligence;

        [HideInInspector] public StatRegularInt PhysicalArmor;
        [HideInInspector] public StatRegularInt MagicArmor;

        [HideInInspector] public StatRegularFloat MeleeDamage;
        [HideInInspector] public StatRegularFloat ShotDamage;
        [HideInInspector] public StatRegularFloat MagicDamage;
        [HideInInspector] public StatFloat ChanceAvoid;
        [HideInInspector] public StatRegularFloat Speed;
        [HideInInspector] public StatFloat СhanceCriticalShot;
        [HideInInspector] public StatFloat СhanceCriticalStrike;
        #endregion
        /// <summary>
        /// Инициализация статов.
        /// </summary>
        public virtual void Init() {
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

            Speed = new StatRegularFloat("Скорость", 0, 0);
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