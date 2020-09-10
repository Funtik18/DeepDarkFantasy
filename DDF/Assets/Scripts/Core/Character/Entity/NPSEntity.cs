using UnityEngine;
using DDF.UI.Bar;
using System.Collections.Generic;
using DDF.UI.Inventory;
using DDF.Character.Effects;
using DDF.Character.Perks;

namespace DDF.Character.Stats {
    
    public class NPSEntity : Entity
    {
        public int strength = 5;
        public int agility = 5;
        public float Speed = 2;
        private void Start() {
            InitStartStats();
            for(int i =0;i<strength;i++)
                IncreaseStrength();
            for(int i =0;i<agility;i++)
                IncreaseAgility();

            CurrentSpeed = Speed;
            UpdateStats();
            CurrentHealthPoints = MaxHealthPoints;
        }
    }

}