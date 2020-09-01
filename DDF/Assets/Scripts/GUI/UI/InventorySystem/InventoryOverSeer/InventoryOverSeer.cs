using System.Collections;
using System.Collections.Generic;
using DDF.Character.Stats;
using UnityEngine;

namespace DDF.UI.Inventory {
    public class InventoryOverSeer : MonoBehaviour {
		protected static InventoryOverSeer _instance { get; private set; }

		[HideInInspector] public List<Inventory> containers;


		[HideInInspector] public RectTransform buffer;

		[HideInInspector] public InventorySlot lastSlot;
		[HideInInspector] public InventorySlot rootSlot;

		[HideInInspector] public InventoryModel lastModel;
		[HideInInspector] public InventoryModel rootModel;

		[HideInInspector] public bool isDrag = false;

		[HideInInspector] public Inventory from;//откуда взяли
		[HideInInspector] public Inventory whereNow;//где сейчас находимся

		protected virtual void Awake() {
			containers = new List<Inventory>();
		}
		public static InventoryOverSeer Getinstance() {
			if(_instance == null) {
				_instance = FindObjectOfType<InventoryOverSeer>();
			}
			return _instance;
		}



		/// <summary>
		/// Показывает все контейнеры и их содержимое.
		/// </summary>
		public void Show() {
			for (int i = 0; i < containers.Count; i++) {
				containers[i].ShowInventory();
			}
		}
		/// <summary>
		/// Прячет все контейнеры и их содержимое.
		/// </summary>
		public void Hide() {
			for(int i = 0; i < containers.Count; i++) {
				containers[i].HideInventory();
			}
		}

		public void OrderRefresh() {
			//ToolTip._instance.transform.SetAsLastSibling();
			//MenuOptions._instance.transform.SetAsLastSibling();
		}

		public void RegistrationContainer( Inventory container ) {
			containers.Add(container);
		}
	}
}