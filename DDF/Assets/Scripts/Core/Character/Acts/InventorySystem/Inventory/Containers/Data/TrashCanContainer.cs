using DDF.Environment;
using DDF.UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DDF.UI.Inventory {
    public class TrashCanContainer : StorageContainer {

		public override void Init(Inventory owner) {
			
			base.Init(owner);
			//если корзина для мусора, то пусть инициализирует точку для выброса и подписывается на функцию.
			ThrowPoint.Init();
			inventory.onItemDisposed = ThrowItem;
		}

		/// <summary>
		/// Выброс предмета в физ мир.
		/// </summary>
		private void ThrowItem(Item item, Inventory inventory) {
			if (!ThrowPoint._instance) return;
			if (item.item3DModel == null) {
				Debug.LogError(item.itemName + " not have item3DModel");
				return;
			}
			Item3DModel throwingItem = Instantiate(item.item3DModel).GetComponent<Item3DModel>();
			throwingItem.transform.position = ThrowPoint._instance.transform.position;
			throwingItem.itemRigidbody.isKinematic = false;
		}

		#region Overrides
		protected override void OnPointerEnter(PointerEventData eventData, InventorySlot slot) {
			overSeer.lastSlot = slot;
			overSeer.whereNow = inventory;
		}
		protected override void OnPointerExit(PointerEventData eventData, InventorySlot slot) {
			overSeer.lastSlot = slot;
			overSeer.whereNow = null;
		}

		protected override void OnPointerDown(PointerEventData eventData, InventorySlot slot) {
			if (overSeer.isDrag) ItemBackToRootSlot();
			overSeer.rootSlot = slot;//запомнили слот откуда взяли
		}
		protected override void OnPointerLeftClick(PointerEventData eventData, InventorySlot slot) { }
		protected override void OnPointerRightClick(PointerEventData eventData, InventorySlot slot) {
			if (overSeer.isDrag) return;
		}

		protected override void OnDrop(PointerEventData eventData) {
            if (!overSeer.isDrag) return;

            Item cashItem = overSeer.rootModel.referenceItem;
            ItemPlaceOnSlot(overSeer.from.container, overSeer.whereNow.container, cashItem, overSeer.buffer.GetComponent<Item2DModel>());
            overSeer.whereNow.DeleteItem(cashItem);

            inventory.onItemDisposed?.Invoke(cashItem, overSeer.from.container.inventory);
        }
		#endregion
	}
}
