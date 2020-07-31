using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DDF.UI.Tools {

    public class DraggableWindow : MonoBehaviour, IDragUI, IPointerUI {

        private Vector2 pointerOffset;//для того чтобы не перетаскивать с середины

        [Tooltip("Ограничение.")]
        public RectTransform container;
        [Tooltip("Объект который двигаем.")]
        public RectTransform root;


        private bool clampedToLeft;
        private bool clampedToRight;
        private bool clampedToTop;
        private bool clampedToBottom;

        public void Start() {
            if (container == null)
                container = transform.parent as RectTransform;
            if (root == null)
                root = transform as RectTransform;

            clampedToLeft = false;
            clampedToRight = false;
            clampedToTop = false;
            clampedToBottom = false;
        }
        public void OnBeginDrag( PointerEventData eventData ) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(root, eventData.position, eventData.pressEventCamera, out pointerOffset);
        }
        public void OnDrag( PointerEventData eventData ) {

            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(container, eventData.position, eventData.pressEventCamera, out localPointerPosition)) {

                root.localPosition = localPointerPosition - pointerOffset;//движение
                ClampToWindow();//определение перешёл ли границу экрана
                Vector2 clampedPosition = root.localPosition;
                OffsetBorders(ref clampedPosition);
                root.localPosition = clampedPosition;

            }

        }
        public void OnEndDrag( PointerEventData eventData ) {

        }


        void ClampToWindow() {
            Vector3[] canvasCorners = new Vector3[4];
            Vector3[] panelRectCorners = new Vector3[4];

            container.GetWorldCorners(canvasCorners);
            root.GetWorldCorners(panelRectCorners);

            if (panelRectCorners[2].x > canvasCorners[2].x) {//Debug.Log("Panel is to the right of canvas limits");
                if (!clampedToRight) {
                    clampedToRight = true;
                }
            } else if (clampedToRight) {
                clampedToRight = false;
            }

            if (panelRectCorners[0].x < canvasCorners[0].x) {//Debug.Log("Panel is to the left of canvas limits");
                if (!clampedToLeft) {
                    clampedToLeft = true;
                }
            } else if (clampedToLeft) {
                clampedToLeft = false;
            }

            if (panelRectCorners[2].y > canvasCorners[2].y) {//Debug.Log("Panel is to the top of canvas limits");
                if (!clampedToTop) {
                    clampedToTop = true;
                }
            } else if (clampedToTop) {
                clampedToTop = false;
            }

            if (panelRectCorners[0].y < canvasCorners[0].y) {//Debug.Log("Panel is to the bottom of canvas limits");
                if (!clampedToBottom) {
                    clampedToBottom = true;
                }
            } else if (clampedToBottom) {
                clampedToBottom = false;
            }
        }

        void OffsetBorders( ref Vector2 _clampedPosition ) {
            if (clampedToRight) {
                _clampedPosition.x = ( container.rect.width / 2 ) - ( root.rect.width * ( 1 - root.pivot.x ) );
            }

            if (clampedToLeft) {
                _clampedPosition.x = ( -container.rect.width / 2 ) + ( root.rect.width * root.pivot.x );
            }

            if (clampedToTop) {
                _clampedPosition.y = ( container.rect.height / 2 ) - ( root.rect.height * ( 1 - root.pivot.y ) );
            }

            if (clampedToBottom) {
                _clampedPosition.y = ( -container.rect.height / 2 ) + ( root.rect.height * root.pivot.y );
            }
        }

        public void OnPointerClick( PointerEventData eventData ) { }
        public void OnPointerDown( PointerEventData eventData ) { }
        public void OnPointerEnter( PointerEventData eventData ) { }
        public void OnPointerExit( PointerEventData eventData ) { }
        public void OnPointerUp( PointerEventData eventData ) { }

    }
}