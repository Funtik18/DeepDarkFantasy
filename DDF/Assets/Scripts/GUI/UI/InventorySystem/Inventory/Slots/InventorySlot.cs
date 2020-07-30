using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DDF.Inventory {

    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [AddComponentMenu("DDF/Inventory/Slot", 0)]
    [Serializable]
    public class InventorySlot : MonoBehaviour, IPointerUI, IDragUI, IDropUI {

        //public Transform[] squares;

        [SerializeField]
        private Item item;

        [HideInInspector]
        public Item Item {
            get { return item; }
            protected set {
                if (item != value) {
                    item = value;
                    if (item == null) StackCount = 0;
					else {
                        //StackCount = (int)item.StackCount;
                    }
                }
            }
        }
        private int stackSize = 0;
        public int StackCount {
            protected set {
                if (stackSize != value) {
                    stackSize = value;
                }
            }

            get {
                if (stackSize <= 0) return 0;
                else return stackSize;
            }
        }

        [Header("Inventory Behaviour")]

        /*[SerializeField]
        [Tooltip("The default icon to use when no item is displayed in this slot.")]
        private Sprite defaultIcon;
        public Sprite DefaultIcon {
            get { return defaultIcon; }
            set {
                defaultIcon = value;
                if (Item == null) SetDefaultIcon();
            }
        }
        public Color defaultIconColor = Color.white;
        */

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


        /* [SerializeField]
         private Image iconImage;
         [HideInInspector]
         public Sprite Icon {
             protected set {
                 iconImage.sprite = value;
                 if (value == null) iconImage.enabled = false;
                 else iconImage.enabled = true;
             }
             get {
                 return iconImage.sprite;
             }
         }
         */

        


        public bool autoEquip = false;

        [HideInInspector]
        public Vector2Int position = Vector2Int.zero;


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

		/// <summary>
		/// Helper for setting default icon of the slot.
		/// </summary>
		/*public void SetDefaultIcon() {
            //iconMesh.Rotation = Vector3.zero;
            //IconMesh.material = null;
            //Icon3D = null;
            Icon = defaultIcon;

            iconImage.color = defaultIconColor;
        }
        */
        #region Triggers, Events
        [Serializable]
        public class PointerTrigger : UnityEvent<PointerEventData, InventorySlot> { }
        [HideInInspector]
        public PointerTrigger OnHover = new PointerTrigger();
        [HideInInspector]
        public PointerTrigger OnDown = new PointerTrigger();
        [HideInInspector]
        public PointerTrigger OnClick = new PointerTrigger();
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

            OnClick.Invoke(eventData, this);
            //if (this.Item != null) Item.OnClick.Invoke(eventData, this);
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
