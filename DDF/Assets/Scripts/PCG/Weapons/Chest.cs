using DDF.Environment;
using System.Collections;
using UnityEngine;

namespace DDF.PCG.WEAPON
{
    public class Chest : MonoBehaviour
    {
        public ChestInteraction chest;
        public WeaponGenerator wg;
        void Start()
        {
            //yield return new WaitForSeconds(3f);
            chest.startItems.Add(wg.Generator(3));
            chest.startItems.Add(wg.Generator(3));
            chest.startItems.Add(wg.Generator(3));
            chest.startItems.Add(wg.Generator(3));
        }
    }
}