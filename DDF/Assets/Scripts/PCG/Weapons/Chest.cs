using DDF.Environment;
using System.Collections;
using UnityEngine;

namespace DDF.PCG.WEAPON
{
    public class Chest : MonoBehaviour
    {
        public ChestInteraction chest;
        public WeaponGenerator wg;

        public int ItemType;
        [Range(0,3)]
        public int ItemCount;
        void Awake()
        {
            for (int i = 0; i < ItemCount; i++)
            {
                chest.startItems.Add(wg.Generator(ItemType));
            }
        }
    }
}