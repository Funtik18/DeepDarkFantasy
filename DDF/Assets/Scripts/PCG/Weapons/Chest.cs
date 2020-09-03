using DDF.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DDF.PCG.WEAPON
{
    public class Chest : MonoBehaviour
    {
        public ChestInteraction chest;
        public WeaponGenerator wg;
        private void Start()
        {
            chest.startItems.Add(wg.Generator(1));
        }
    }
}