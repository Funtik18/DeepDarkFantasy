using System.Collections;
using System.Collections.Generic;
using DDF.UI.Inventory;
using UnityEngine;

namespace DDF {
	/// <summary>
	/// Класс ссылка, этот класс играет роль наблюдателя между контейнерами в ui.
	/// Сильно помогает при работе инвентарей.
	/// </summary>
    public class InventoryOverSeer : MonoBehaviour {
		protected static InventoryOverSeer _instance { get; private set; }

		[HideInInspector] public List<Inventory> containers;

		[HideInInspector] public RectTransform buffer;

		[HideInInspector] public InventorySlot lastSlot;
		[HideInInspector] public InventorySlot rootSlot;

		[HideInInspector] public Item2DModel lastModel;
		[HideInInspector] public Item2DModel rootModel;

		[HideInInspector] public bool isDrag = false;

		[HideInInspector] public Inventory from;//откуда взяли
		[HideInInspector] public Inventory whereNow;//где сейчас находимся

		public static InventoryOverSeer GetInstance() {
			if (_instance == null) {
				_instance = FindObjectOfType<InventoryOverSeer>();
			}
			return _instance;
		}

		protected virtual void Awake() {
			containers = new List<Inventory>();
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
		/// <summary>
		/// Запоминает новый контейнер.
		/// </summary>
		public void RegistrationContainer( Inventory container ) {
			containers.Add(container);
		}
	}
}