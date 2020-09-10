using UnityEngine;
using DDF.UI.Inventory.Items;

namespace DDF.Editor {
	using UnityEditor;
    [CustomEditor(typeof(Item))]

    public class ItemDrawer : Editor {
        uint cashCurrentStackCount;
        int cashMaxStackCount;

        bool firstTime = true;

        Item item;

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            
            item = target as Item;

            uint cashCurrent = item.itemStackCount;
            int cashMax = item.itemStackSize;

            if (firstTime) {

                cashCurrentStackCount = cashCurrent;
                cashMaxStackCount = cashMax;


                firstTime = false;
            }


            #region Stack
            //maxi
            if (cashMaxStackCount != cashMax) {//проверка на 0

                if (cashMax == 0) {
                    if (cashMaxStackCount < 0) {
                        cashMax--;
                    } else {
                        cashMax++;
					}
				}
                cashMaxStackCount = cashMax;
            }

            cashCurrentStackCount = cashCurrent;


            if (cashCurrentStackCount > cashMaxStackCount && cashMaxStackCount != -1) {
                cashCurrentStackCount = (uint)cashMaxStackCount;
            }

			#endregion


			item.itemStackCount = cashCurrentStackCount;
            item.itemStackSize = cashMaxStackCount;

        }
    }
}