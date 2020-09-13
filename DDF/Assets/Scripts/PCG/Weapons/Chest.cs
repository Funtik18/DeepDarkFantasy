using DDF.Environment;
using System.Collections;
using UnityEngine;

namespace DDF.PCG.WEAPON
{
    public class Chest : MonoBehaviour
    {
        public ChestInteraction chest;

        public int ItemType;
        [Range(0,3)]
        public int ItemCount;
        void Awake()
        {
            chest.fix = () => {
                for (int i = 0; i < ItemCount; i++) {
                    chest.insInventory.AddItem(WeaponGenerator.GetInstance().Generator(ItemType));
                }
            };
        }
    }
}