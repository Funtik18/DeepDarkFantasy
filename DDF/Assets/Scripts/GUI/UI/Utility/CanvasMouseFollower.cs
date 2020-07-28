using System.Collections;
using System.Collections.Generic;
using Toolbox.Common;
using UnityEngine;


namespace DDFInventory.Utility {

    /// <summary>
    /// Вспомогательный компонент, который позволяет игровому объекту следовать за мышью на канвасе.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Inventory/Utility/Mouse Follower", 0)]
    public class CanvasMouseFollower : MonoBehaviour {

        [Tooltip("The canvas that will be used when determining where to position this GameObject.")]
        public Canvas canvas;


        void Update() {
            transform.position = GetPointerPosOnCanvas(canvas, PointerUtility.GetPosition());
        }

        /// <summary>
        /// Global static helper method for finding the position of the mouse on a canvas.
        /// </summary>
        public static Vector3 GetPointerPosOnCanvas( Canvas canvas, Vector2 pointerPos ) {
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera) {
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pointerPos, canvas.worldCamera, out pos);
                return canvas.transform.TransformPoint(pos);
            } else if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
                return PointerUtility.GetPosition();
            } else {
                Vector3 globalMousePos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, PointerUtility.GetPosition(), canvas.worldCamera, out globalMousePos)) {
                    return globalMousePos;
                }
            };

            return Vector2.zero;
        }
    }
}


