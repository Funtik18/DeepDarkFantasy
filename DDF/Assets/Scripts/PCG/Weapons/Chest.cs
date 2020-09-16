using DDF.Environment;
using System.Collections;
using UnityEngine;

namespace DDF.PCG.WEAPON
{
    public class Chest : MonoBehaviour
    {
        public ChestInteraction chest;

        [Range(0, 3)]
        public int ItemType;
        
        public int ItemCount;
        void Awake()
        {
            chest.onCreated = InitItems;
        }

        private void InitItems() {
            for (int i = 0; i < ItemCount; i++) {
                chest.insInventory.AddItem(WeaponGenerator._instance.Generator(ItemType));
            }
        }
    }
}