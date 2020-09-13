using System.Collections.Generic;
using DDF.UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DDF.UI.Inventory {
	public class StorageContainer : Container {
		protected int actionSelection = -1;

		#region Overrides
		protected override void OnPointerEnter(PointerEventData eventData, InventorySlot slot) {
			base.OnPointerEnter(eventData, slot);
			if (overSeer.isDrag) {//передвигает от фром
				Inventory whereNow = overSeer.whereNow;
				if (whereNow != null) {
					if (!view.SolidHightlight) {
						( whereNow.container as StorageContainer).HoverLanding();//и подсвечивает тот контейнер на который указывает
					}
				}
			} else {
				if (view.SolidHightlight) {
					for (int i = 0; i < slotsList.Count; i++) {
						HoverHightlightCursor(slotsList[i], view.disableColor);
					}
				}
				if (view.HoverHightlight) {
					HoverHightlightCursor(overSeer.lastSlot, view.hoverColor);
				}
				HoverItem();
			}
		}
		protected override void OnPointerExit(PointerEventData eventData, InventorySlot slot) {
			base.OnPointerExit(eventData, slot);
			ReloadHightLight();
		}

		protected override void OnDrop(PointerEventData eventData) {
			if (!overSeer.isDrag) return;
			if (actionSelection == -1) {//нельзя
				ItemBackToRootSlot();
			}
			if (actionSelection == 1) {//можно
				ItemPlaceOnSlot(overSeer.from.container, overSeer.whereNow.container, overSeer.rootModel.referenceItem, overSeer.buffer.GetComponent<Item2DModel>());
			}
			if (actionSelection == 2) {//обмен
				ItemBackToRootSlot();
			}
		}

		public override void DeleteItem(Item item) {
			base.DeleteItem(item);
			ReloadHightLight();
		}
		public override void ItemBackToRootSlot() {
			Container from = overSeer.from.container;
			Item2DModel model = overSeer.rootModel;
			Item item = model.referenceItem;

			from.AddItemOnPosition(item, overSeer.rootSlot);

			from.AddCurrentItem(item);

			overSeer.isDrag = false;
			( overSeer.from.container as StorageContainer).ReloadHightLight();
		}
		protected void SelectLandingSlots(InventorySlot slot, Vector2 size) {
			List<InventorySlot> slots = TakeSlotsBySize(slot, size);
			List<Item> unionItems = TakeUnionSlots(slots);

			if (unionItems.Count == 0) {
				if (slots.Count == size.x * size.y) {
					HightlightSlots(slots, view.highlightColor);

					actionSelection = 1;
				} else {
					HightlightSlots(slots, view.invalidColor);

					actionSelection = -1;
				}
			} else if (unionItems.Count == 1) {
				List<InventorySlot> diffrentItemSlots = FindItemSlots(unionItems[0]);

				HightlightSlots(diffrentItemSlots, view.replaceColor);
				HightlightSlots(slots, view.replaceColor);

				actionSelection = 2;
			} else {
				for (int i = 0; i < unionItems.Count; i++) {
					List<InventorySlot> diffrentItemSlots = FindItemSlots(unionItems[i]);

					HightlightSlots(diffrentItemSlots, view.invalidColor);
				}
				HightlightSlots(slots, view.invalidColor);

				actionSelection = -1;
			}

			unionItems.Clear();
			slots.Clear();
		}
		protected override void ItemPlaceOnSlot(Container from, Container to, Item item, Item2DModel model) {
			base.ItemPlaceOnSlot(from, to, item, model);
			ReloadHightLight();
		}
		#endregion

		#region Hightlight
		public void ReloadHightLight() {
			DeselectAllSlots();
			HoverNotEmptySlots();
		}

		/// <summary>
		/// Подсветка посадки предмета.
		/// </summary>
		protected void HoverLanding() {
			SelectLandingSlots(overSeer.lastSlot, overSeer.buffer.GetComponent<Item2DModel>().referenceItem.GetSize());
		}
		/// <summary>
		/// Подсветка курсора.
		/// </summary>
		protected void HoverHightlightCursor(InventorySlot slot, Color highlight) {
			HightlightSlot(slot, highlight);
		}
		/// <summary>
		/// Наведение на предметы.
		/// </summary>
		protected void HoverItem() {
			if (!overSeer.lastSlot.isEmpty()) {
				HightlightItemSlots(overSeer.lastSlot);
			}
		}

		/// <summary>
		/// Подсвечивает слоты с предметами.
		/// </summary>
		protected void HoverNotEmptySlots() {
			HightlightSlots(SelectAllNotEmptySlots(), view.baseColor);
		}

		/// <summary>
		/// Подсвечивает все слоты.
		/// </summary>
		/// <param name="color"></param>
		public void HightlightAllSlots(Color color) {
			HightlightSlots(slotsList, color);
		}
		/// <summary>
		/// Востанавливает "нормальную подсветку"
		/// </summary>
		protected void DeselectAllSlots() {
			HightlightSlots(slotsList, view.normalColor);
		}


		/// <summary>
		/// Подсветка всех слотов у айтема.
		/// </summary>
		/// <param name="slot"></param>
		protected void HightlightItemSlots(InventorySlot slot) {

			List<InventorySlot> slots = FindItemSlots(slot.Item);

			if (slots.Count > 0) {
				HightlightSlots(slots, view.hoverColor);
			}
			slots.Clear();
		}


		/// <summary>
		/// Подсветка слотов.
		/// </summary>
		/// <param name="slots"></param>
		/// <param name="color"></param>
		protected void HightlightSlots(List<InventorySlot> slots, Color color) {
			for (int i = 0; i < slots.Count; i++) {
				HightlightSlot(slots[i], color);
			}
		}
		/// <summary>
		/// Подсветка слота.
		/// </summary>
		/// <param name="slot"></param>
		/// <param name="color"></param>
		protected void HightlightSlot(InventorySlot slot, Color color) {
			slot.HighlightColor = color;
		}
		#endregion


		#region UIInteraction
		private void ToolTipShow() {

			if (overSeer.lastSlot.isEmpty()) return;
			RectTransform rectPos;

			Item item = overSeer.lastSlot.Item;
			List<InventorySlot> slots = TakeSlotsByItem(item);
			rectPos = slots[slots.Count - 1].GetComponent<RectTransform>();

			slots.Clear();

			inventory.toolTip.SetItem(item);
			inventory.toolTip.SetPosition(grid.RecalculatePositionToCornRect(rectPos, inventory.toolTip.rect));
			inventory.toolTip.ShowToolTip();
		}
		private void ToolTipHide() {
			inventory.toolTip.HideToolTip();
		}

		private void MenuOptionsShow() {
			if (overSeer.lastSlot.isEmpty()) return;
			RectTransform rectPos;

			Item item = overSeer.lastSlot.Item;
			List<InventorySlot> slots = TakeSlotsByItem(item);
			rectPos = slots[slots.Count - 1].GetComponent<RectTransform>();

			slots.Clear();

			MenuOptions._instance.SetPosition(grid.RecalculatePositionToCornRect(rectPos, MenuOptions._instance.rect));

			MenuOptions._instance.SetCurrentItem(item, inventory);
			MenuOptions._instance.OpenMenu();
		}
		private void MenuOptionsHide() => MenuOptions._instance.CloseMenu();

		#endregion
	}
}