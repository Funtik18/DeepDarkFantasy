﻿using DDF.Environment;
using UnityEngine;

namespace DDF.PCG.WEAPON
{
    public class Chest : MonoBehaviour
    {
        public ChestInteraction chest;
        public WeaponGenerator wg;
        private void Start()
        {
            chest.startItems.Add(wg.Generator(2));
            chest.startItems.Add(wg.Generator(2));
            chest.startItems.Add(wg.Generator(2));
            chest.startItems.Add(wg.Generator(2));
        }
    }
}