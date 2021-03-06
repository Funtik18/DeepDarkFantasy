﻿using System;
using DDF.Atributes;
using UnityEngine;

namespace DDF.Character.Stats {
    [Serializable]
    public class Stats {

		#region Stats
		public StatInt Level;
        public StatRegularInt LevelExperience;
        public StatRegularInt SkillPoints;

        public StatRegularFloat HealthPoints;
        public StatRegularFloat ManaPoints;

        public StatFloat ResistFire;
        public StatFloat ResistIce;
        public StatFloat ResistPoison;

        public StatInt Strength;
        public StatInt Agility;
        public StatInt Intelligence;
        public StatInt Luck;

        public StatFloat PhysicalArmor;
        public StatRegularInt MagicArmor;

        public StatRegularFloat MeleeDamage;
        public StatRegularFloat ShotDamage;
        public StatRegularFloat MagicDamage;
        public StatFloat ChanceAvoid;
        public StatRegularFloat Speed;
        public StatFloat СhanceCriticalShot;
        public StatFloat СhanceCriticalStrike;

        public StatRegularInt Weight;
        #endregion

        [ReadOnly]
        /// <summary>
        /// Минимально возможный уровень.
        /// </summary>
        public int levelMin = 1;
        [ReadOnly]
        /// <summary>
        /// Минимально возможное значаение стата.
        /// </summary>
        public int statMin = 1;


        /// <summary>
        /// Инициализация статов.
        /// </summary>
        public virtual void Init() {
            Level = new StatInt("Текущий уровень", 1);
            LevelExperience = new StatRegularInt("Опыт", 0, 0);
            SkillPoints = new StatRegularInt("Доступные очки", 0, 0);

            HealthPoints = new StatRegularFloat("Жизненые силы", 0, 0);
            ManaPoints = new StatRegularFloat("Магическая энергия", 0, 0);

            ResistFire = new StatFloat("Сопротивление к огню", 0);
            ResistIce = new StatFloat("Сопротивление к холоду", 0);
            ResistPoison = new StatFloat("Сопротивление к яду", 0);

            Strength = new StatInt("Сила", 1);
            Agility = new StatInt("Ловкость", 1);
            Intelligence = new StatInt("Интелект", 1);
            Luck = new StatInt("Удача", 1);

            PhysicalArmor = new StatFloat("Физическая броня", 0);
            MagicArmor = new StatRegularInt("Магическая броня", 0, 0);

            MeleeDamage = new StatRegularFloat("Урон в ближнем бою", 0, 0, "-");
            ShotDamage = new StatRegularFloat("Урон в дальнем бою", 0, 0, "-");
            MagicDamage = new StatRegularFloat("Магический урон", 0, 0, "-");
            СhanceCriticalShot = new StatFloat("Шанс крит выстрела", 0, "%");
            СhanceCriticalStrike = new StatFloat("Шанс крит удара", 0, "%");
            ChanceAvoid = new StatFloat("Шанс уклонения", 0, "%");

            Speed = new StatRegularFloat("Скорость", 0, 0);

            Weight = new StatRegularInt("", 0, 0);
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