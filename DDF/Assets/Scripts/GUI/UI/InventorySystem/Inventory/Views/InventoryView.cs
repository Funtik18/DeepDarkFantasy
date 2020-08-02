using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DDF.UI.Inventory {
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [AddComponentMenu("Inventory/View", 4)]
    [Serializable]
    public class InventoryView : MonoBehaviour {

        public InventoryContainer container;


        [Header("Drag & Drop Toggles")]
        public bool DisableDragging;
        public bool DisableDropping;
        public bool DisableWorldDropping;

        public bool HoverHightlight = false;
        public bool SolidItemSlot = false;


        [Header("Slot Colors")]
        public Color normalColor = Color.clear;
        public Color highlightColor = new Color32(127, 223, 127, 255);//green
        public Color replaceColor = new Color32(223, 223, 63, 255);//yellow
        public Color invalidColor = new Color32(223, 127, 127, 255);//red

        public Color baseColor = new Color32(159, 159, 223, 255);//blue
        public Color hoverColor = new Color32(191, 191, 223, 255);//blue2
        public Color disableColor = new Color32(223, 223, 223, 255);//gray
    }
}