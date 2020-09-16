using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DDF.UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DDF.UI.Inventory {
    public class EquipmentContainer : StorageContainer {

		#region Overrides
		public override Item AddItem(Item item, bool enableModel, bool interactModel = true) {
            AddItemXY(item, size, enableModel, interactModel);
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


        protected override void OnBeginDrag(PointerEventData eventData) {
            base.OnBeginDrag(eventData);

        }
        protected override void OnDrop(PointerEventData eventData) {
            if (!overSeer.isDrag) return;
            Inventory whereNow = overSeer.whereNow;
            Inventory from = overSeer.from;
            Item2DModel model = overSeer.rootModel;
            Item item = model.referenceItem;
            if (!whereNow.IsEmpty) {//если занято, нельзя дропнуть
                from.container.ItemBackToRootSlot();
                return;
            }

            List<StorageTypes> storageTypes = whereNow.storageTypes;
            if (storageTypes.Count == 0) {
                ItemPlaceOnSlotSolid(from.container, whereNow.container, item, model);
                return;
            } else {
                for (int i = 0; i < storageTypes.Count; i++) {
                    if (item.CompareType(storageTypes[i].ToString())) {//смогли найти место для дропа
                        ItemPlaceOnSlotSolid(from.container, whereNow.container, item, model);
                        inventory.onItemDrop?.Invoke(item, inventory);
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
            
            grid.RecalculateCellPosition(model.transform as RectTransform, from.size);

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

            /*if (recalculatePos) {
                grid.ResetCell(model.transform as RectTransform, slots[0].position);
                grid.RecalculateCellProportion(model.transform as RectTransform, size);

                grid.RecalculateCellPosition(model.transform as RectTransform, size);
            }*/

            grid.ResetCell(model.transform as RectTransform, slots[0].position);
            grid.RecalculateCellProportion(model.transform as RectTransform, to.size);

            RectTransform rect = slots[0].GetComponent<RectTransform>();
            model.transform.SetParent(to.grid.dragParent);
            model.transform.position = rect.TransformPoint(rect.rect.center);
            grid.RecalculateCellPosition(model.transform as RectTransform, to.size);

            to.AddModelItem(model);

            

            overSeer.isDrag = false;

            ReloadHightLight();
        }
		#endregion
	}
}