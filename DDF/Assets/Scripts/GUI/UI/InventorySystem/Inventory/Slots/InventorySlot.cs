using DDF.UI.Events;
using DDF.UI.Inventory.Items;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DDF.UI.Inventory {

    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [AddComponentMenu("Inventory/Slot", 3)]
    [Serializable]
    public class InventorySlot : MonoBehaviour, IPointerUI, IDragUI, IDropUI {

        [SerializeField]
        private Item item;

        [HideInInspector]
        public Item Item {
            get { return item; }
            protected set {
                if (item != value) {
                    item = value;
                }
            }
        }

        [Header("Sub-GameObject References")]

        [SerializeField]
        private Image highlight;
        public Color HighlightColor {
            set {
                if (highlight != null) {
                    highlight.color = value;
                    //disable the highlight if the color is fully transparent
                    if (value.a <= 0.001f) highlight.enabled = false;
                    else highlight.enabled = true;
                }
            }
            get { return highlight.color; }
        }
        /// <summary>
        /// Позиция в инвентаре.
        /// </summary>
        [HideInInspector]
        public Vector2Int position = Vector2Int.zero;

		#region Functional
		public virtual void AssignItem( Item newItem ) {

            Item = newItem;
        }
        public virtual void FreeItem() {
            Item = null;
        }
        public virtual bool isEmpty(){
            if (Item == null){
                return true;
			}
            return false;
        }
		#endregion

		#region Triggers, Events
		[Serializable]
        public class PointerTrigger : UnityEvent<PointerEventData, InventorySlot> { }
        [HideInInspector]
        public PointerTrigger OnHover = new PointerTrigger();
        [HideInInspector]
        public PointerTrigger OnDown = new PointerTrigger();
        [HideInInspector]
        public PointerTrigger OnLeftClick = new PointerTrigger();
        [HideInInspector]
        public PointerTrigger OnMiddleClick = new PointerTrigger();
        [HideInInspector]
        public PointerTrigger OnRightClick = new PointerTrigger();
        [HideInInspector]
        public PointerTrigger OnUp = new PointerTrigger();
        [HideInInspector]
        public PointerTrigger OnEndHover = new PointerTrigger();

        [Serializable]
        public class DragTrigger : UnityEvent<PointerEventData> { }
        [HideInInspector]
        public DragTrigger OnBeginDragEvent = new DragTrigger();
        [HideInInspector]
        public DragTrigger OnDragEvent = new DragTrigger();
        [HideInInspector]
        public DragTrigger OnEndDragEvent = new DragTrigger();

        [HideInInspector]
        public DragTrigger OnDropEvent = new DragTrigger();
		#endregion

		#region Interfaces
		#region Pointer
		public void OnPointerEnter( PointerEventData eventData ) {

            OnHover.Invoke(eventData, this);

            /*if (Input.touches.Length > 0) {
                HoverStartTime = Time.unscaledTime;
                TouchHover = true;
            }*/
        }
        public void OnPointerDown( PointerEventData eventData ) {
            OnDown.Invoke(eventData, this);
        }
        public void OnPointerClick( PointerEventData eventData ) {
            //if (InvalidateClick) return;
            //if (TouchHover && Time.unscaledTime - HoverStartTime > MinHoverTime) return;
            if(eventData.button == PointerEventData.InputButton.Left) {
                OnLeftClick.Invoke(eventData, this);
            }
            if (eventData.button == PointerEventData.InputButton.Middle) {
                OnMiddleClick.Invoke(eventData, this);
            }
            if (eventData.button == PointerEventData.InputButton.Right) {
                OnRightClick.Invoke(eventData, this);
            }
        }
		public void OnPointerUp( PointerEventData eventData ) {
            OnUp.Invoke(eventData, this);
        }
		public void OnPointerExit( PointerEventData eventData ) {
            OnEndHover.Invoke(eventData, this);
            //InvalidateClick = false;
            //TouchHover = false;

        }
		#endregion
		#region Drag
		public void OnBeginDrag( PointerEventData eventData ) {
            if (Item != null) {
                OnBeginDragEvent.Invoke(eventData);
                //InvalidateClick = true;
            }
        }
		public void OnDrag( PointerEventData eventData ) {
            OnDragEvent.Invoke(eventData);
        }
        public void OnEndDrag( PointerEventData eventData ) {
            OnEndDragEvent.Invoke(eventData);
            //InvalidateClick = false;
        }
        public void OnDrop( PointerEventData eventData ) {
            OnDropEvent.Invoke(eventData);
            //InvalidateClick = false;
        }
		#endregion
		#endregion
	}
}
