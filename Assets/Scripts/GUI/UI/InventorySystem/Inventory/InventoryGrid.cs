using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.Inventory {

    public class InventoryGrid : MonoBehaviour {
        [HideInInspector]
        public InventoryView view;

        [Tooltip("Запрашивает слот с InventorySLot")]
        public GameObject slotPrefab;
        [Tooltip("Запрашивает модель с InventoryModel")]
        public GameObject modelPrefab;

        public int width = 10;
        public int height = 6;

        public Vector2 cellSize = new Vector2(64, 64);
        public Vector2 cellSpace;

        public Vector2 offset;

        private Camera mainCamera;
        private float screenToCameraDistance;

        private void Awake() {

            mainCamera = Camera.main;
            screenToCameraDistance = mainCamera.nearClipPlane;

            ConstructGrid();
        }

        /// <summary>
        /// Генерирует сетку.
        /// </summary>
        public void ConstructGrid() {
            DisposeGrid();

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {

                    GameObject obj = HelpFunctions.TransformSeer.CreateObjectInParen(transform, slotPrefab);
                    obj.name = "Slot(" + x + "," + y + ")";

                    RectTransform slotRect = obj.transform as RectTransform;


                    RecalculateCellSizeAndPosition(slotRect, cellSize, new Vector2(x, y));
                }
            }
        }
        /// <summary>
        /// Удаляет сетку.
        /// </summary>
        public void DisposeGrid() {
            HelpFunctions.TransformSeer.DestroyChildrenInParent(transform);
        }


        /// <summary>
        /// Создаёт объект для просмотра предмета, его картинку.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public RectTransform CreateModelByItem( Item item, bool solidItemSlot ) {

            GameObject obj = Instantiate(modelPrefab);
            obj.name = item.name;

            InventoryModel model = obj.GetComponent<InventoryModel>();

            model.Icon.sprite = item.icon;
            model.Icon.preserveAspect = true;

			if (solidItemSlot) {
                model.Hightlight.gameObject.SetActive(true);
                model.Hightlight.color = view.baseColor;
			} else {
                model.Hightlight.gameObject.SetActive(false);
            }


            model.referenceItem = item;
            
            RectTransform rect = obj.transform as RectTransform;

            rect.SetParent(view.dragParent);


            rect.sizeDelta = cellSize;
            rect.localScale = Vector3.one;
            rect.rotation = Quaternion.Euler(item.iconOrientation.x, item.iconOrientation.y, item.iconOrientation.z);

            return rect;
        }


        public Vector3 RectSetPositionToWorld(Vector3 position2D ) {
            return mainCamera.ScreenToWorldPoint(GetPositionNearClip(position2D));
        }

        public Vector2 RecalculateToolTipPosition(RectTransform rect) {
            int i = 3;
            RectTransform toolTip = ToolTip.ToolTip._instance.GetComponent<RectTransform>();

            Vector2 newPosition = Vector2.zero;

            if (i == 1) {//
                newPosition = rect.TransformPoint(rect.rect.center + new Vector2(-cellSize.x / 2, cellSize.y / 2) + new Vector2(-toolTip.sizeDelta.x, toolTip.sizeDelta.y));
            }
            if (i == 2) {//
                newPosition = rect.TransformPoint(rect.rect.center + new Vector2(cellSize.x / 2, cellSize.y / 2) + new Vector2(0, toolTip.sizeDelta.y));
            }

            if (i == 3) {

                newPosition = rect.TransformPoint(rect.rect.center + new Vector2(cellSize.x / 2, -cellSize.y / 2));
            }
            if (i == 4) {
                newPosition = rect.TransformPoint(rect.rect.center + new Vector2(-cellSize.x / 2, -cellSize.y / 2) + new Vector2(-toolTip.sizeDelta.x, 0));
            }

            return newPosition;
        }

        private Vector3 GetPositionNearClip(Vector3 localPos ) {
            Vector3 positionNearClipPlane = new Vector3(localPos.x, localPos.y, screenToCameraDistance);
            return positionNearClipPlane;
        }



        #region Recalculate
        public void RecalculateCellSizeAndPosition( RectTransform cellRect, Vector2 cellSize, Vector2 cellPos) {
            cellRect.sizeDelta = cellSize;
            Vector2 newPos = new Vector2(( cellSize.x * cellPos.x ) + cellSpace.x * cellPos.x, -( cellSize.y * cellPos.y ) + cellSpace.y * cellPos.y);
            newPos += offset;

            cellRect.anchoredPosition = newPos;
        }
        public void RecalculateCellProportion( RectTransform cellRect, Vector2 size ) {

            Vector2 cellOffset = new Vector2(cellSpace.x * (float)Math.Floor(size.x / 2), -cellSpace.y * (float)Math.Floor(size.y / 2));
            Vector2 newSize = cellRect.sizeDelta * size + cellOffset;

            cellRect.sizeDelta = newSize;

            RecalculateCellPosition(cellRect, size);
        }
        public void RecalculateCellPosition( RectTransform cellRect, Vector2 size ) {
            cellRect.anchoredPosition += new Vector2((float)Math.Floor(cellSize.x / 2) * ( size.x - 1 ), -( (float)Math.Floor(cellSize.y / 2) * ( size.y - 1 ) ));
        }

        public void ResetCell( RectTransform cellRect, Vector2 cellPos ) {
            RecalculateCellSizeAndPosition(cellRect, cellSize, cellPos);
        }
		#endregion
	}
}
