using DDF.Atributes;
using DDF.Environment;
using DDF.UI.Inventory.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(CanvasGroup))]
	public class Inventory : MonoBehaviour {

		[ReadOnly] public string inventoryID;
		[ReadOnly] public string inventoryName;

		public bool isGUI = false;
		public bool isTooltipEnabled = true;
		public bool isMenuOptionsEnabled = true;


		[HideInInspector] public bool isFull;
		[HideInInspector] public bool IsEmpty {
			get {
				return currentItems.Count == 0;
			}
		}

		
		[HideInInspector] public InventoryOverSeer overSeer;
		public InventoryView view;
		public Container container;
		/// <summary>
		/// Фильтр. Если размер 0 то принимает любой предмет.
		/// </summary>
		public List<StorageTypes> storageTypes;
		public List<Item> currentItems;

		/// <summary>
		/// Событие. Происходит когда какой-то айтем добавился в какой-то контейнер.
		/// </summary>
		public UnityAction<Item, Inventory> onItemAdded;
		public UnityAction<Item, Inventory> onItemRemoved;
		public UnityAction<Item, Inventory> onItemDisposed;

		private CanvasGroup canvasGroup;

		public void CreateNewID() {
			inventoryID = System.Guid.NewGuid().ToString();
			inventoryName = container.GetType().Name;
		}
		protected virtual void Awake() {
			canvasGroup = GetComponent<CanvasGroup>();
			if (inventoryID == "")
				CreateNewID();
			if (isGUI)
				overSeer = InventoryOverSeerGUI.GetInstance();
			else
				overSeer = InventoryOverSeerUI.GetInstance();

			container.Init(this);
			overSeer.RegistrationContainer(this);
		}

		public virtual Item AddItem(Item item, bool enableModel = true) {
			if (item == null) { Debug.LogError("Error 404"); return null; }
			Item clone = item.GetItemCopy<Item>();
			return container.AddItem(clone, enableModel);
		}
		public void DeleteItem(Item item) {
			container.DeleteItem(item);
			container.DeleteModel(item);
		}

		public void ShowInventory() {
			Help.HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup, true);
			container.ShowContainer();
		}
		public void HideInventory() {
			container.HideContainer();
			Help.HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);
		}
	}
	public enum StorageTypes {
		HeadItem,
		TorsoItem,
		WaistItem,
		LegsItem,
		FeetItem,
		WristItem,
		JewerlyItem,

		OffHandItem,
		OneHandedItem,
		RangedItem,
		TwoHandedItem,

		FoodItem,
		PotionItem,
	}
}