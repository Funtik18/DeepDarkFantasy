using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DDF.UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DDF.UI.Inventory {
    public class EquipmentContainer : StorageContainer {

		#region Overrides

		public override Item AddItem(Item item, bool enableModel) {
            AddItemXY(item, size, enableModel);
            return item;
        }

        protected override void OnPointerEnter(PointerEventData eventData, InventorySlot slot) {
            base.OnPointerEnter(eventData, slot);
            if (overSeer.isDrag) {//передвигает от фром
                Inventory whereNow = overSeer.whereNow;
                StorageContainer container = (whereNow.container as StorageContainer);
                if (whereNow != null) {
                    if (view.SolidHightlight) {
                        Item item = overSeer.rootModel.referenceItem;
                        if (item is WeaponItem) {
                            if (!whereNow.IsEmpty) {
                                container.HightlightAllSlots(whereNow.view.invalidColor);
                                return;
                            }
                        }
                        List<StorageTypes> storageTypes = whereNow.storageTypes;
                        if (storageTypes.Count == 0) {
                            container.HightlightAllSlots(whereNow.view.highlightColor);
                            return;
                        } else {
                            for (int i = 0; i < storageTypes.Count; i++) {
                                if (item.CompareType(storageTypes[i].ToString())) {
                                    container.HightlightAllSlots(whereNow.view.highlightColor);
                                    return;
                                }
                            }
                            container.HightlightAllSlots(whereNow.view.invalidColor);
                        }
                    }
                }
            }
        }

		protected override void OnDrop(PointerEventData eventData) {
            if (!overSeer.isDrag) return;
            Inventory whereNow = overSeer.whereNow;
            Inventory from = overSeer.from;

            if (!whereNow.IsEmpty) {
                from.container.ItemBackToRootSlot();
                return;
            }

            Item2DModel model = overSeer.rootModel;
            Item item = model.referenceItem;
            List<StorageTypes> storageTypes = whereNow.storageTypes;
            if (storageTypes.Count == 0) {
                ItemPlaceOnSlotSolid(from.container, whereNow.container, item, model);
                return;
            } else {
                for (int i = 0; i < storageTypes.Count; i++) {
                    if (item.CompareType(storageTypes[i].ToString())) {
                        ItemPlaceOnSlotSolid(from.container, whereNow.container, item, model);
                        return;
                    }
                }
                from.container.ItemBackToRootSlot();
                return;
            }
        }
		public override void ItemBackToRootSlot() {
            Container from = overSeer.from.container;
            Item2DModel model = overSeer.rootModel;
            Item item = model.referenceItem;

            List<InventorySlot> slots = from.slotsList;

            for (int i = 0; i < slots.Count; i++) {
                slots[i].AssignItem(item);
            }

            RectTransform rect = slots[0].GetComponent<RectTransform>();
            model.transform.SetParent(from.grid.dragParent);
            model.transform.position = rect.TransformPoint(rect.rect.center);
            
            grid.RecalculateCellPosition(overSeer.buffer, from.size);

            overSeer.isDrag = false;

            from.AddCurrentItem(item);

            overSeer.isDrag = false;
            ( overSeer.from.container as StorageContainer ).ReloadHightLight();
        }
        /// <summary>
        /// Добавляет айтем на все слоты в контейнер.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="item"></param>
        /// <param name="model"></param>
        /// <param name="recalculatePos"></param>
        private void ItemPlaceOnSlotSolid(Container from, Container to, Item item, Item2DModel model, bool recalculatePos = true) {
            List<InventorySlot> slots = to.slotsList;

            for (int i = 0; i < slots.Count; i++) {
                slots[i].AssignItem(item);
            }
            to.AddCurrentItem(item);

            from.RemoveModelItem(model);

            RectTransform rect = slots[0].GetComponent<RectTransform>();
            model.transform.SetParent(to.grid.dragParent);
            model.transform.position = rect.TransformPoint(rect.rect.center);

            to.AddModelItem(model);

            if (recalculatePos) {
                grid.RecalculateCellPosition(overSeer.buffer, to.size);
                // if (to.inventory.inventorytype == InventoryTypes.Equipment)
                // else
                //grid.RecalculateCellPosition(overSeer.buffer, item.GetSize());
            }

            overSeer.isDrag = false;

            ReloadHightLight();
        }
		#endregion
	}
}