using UnityEngine;
using DDF.Inventory.Items;

namespace DDF.Editor {
	using UnityEditor;
    [CustomEditor(typeof(Item))]

    public class ItemDrawer : Editor {

        int MAX = 10;
        int MIN = -1;

        int cashWidthSize;
        int cashHeightSize;

        int cashCurrentStackCount;
        int cashMaxStackCount;

        bool firstTime = true;

        Item item;

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            
            item = target as Item;

            int cashX = item.itemSize.x;
            int cashY = item.itemSize.y;

            int cashCurrent = item.itemStackCount.x;
            int cashMax = item.itemStackCount.y;

            if (firstTime) {
                cashWidthSize = cashX;
                cashHeightSize = cashY;

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
                cashCurrentStackCount = cashMaxStackCount;
            }

			#endregion



			item.itemStackCount.x = cashCurrentStackCount;
            item.itemStackCount.y = cashMaxStackCount;

        }
    }
}