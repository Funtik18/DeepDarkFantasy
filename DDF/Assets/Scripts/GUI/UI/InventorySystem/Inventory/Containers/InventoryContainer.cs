﻿using DDF.Events;
using DDF.Help;
using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DDF.UI.Inventory {
    /// <summary>
    /// TODO: обновлять, сортировать currentItems
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(InventoryGrid))]
    [DisallowMultipleComponent]
    [AddComponentMenu("Inventory/Container", 2)]
    public class InventoryContainer : MonoBehaviour {

        private Inventory inventory;
        private InventoryOverSeer overSeer { get { return inventory.overSeer; } }
        private InventoryView view { get { return inventory.view; } }

        protected InventoryGrid grid;
        private int width, height;
        private Vector2Int size;

        [HideInInspector] public InventorySlot[,] slotsArray;
        [HideInInspector] public List<InventorySlot> slotsList;

        private List<Item> currentItems; 

        #region Settup

        public void Init() {

            inventory = GetComponentInParent<Inventory>();

            grid = GetComponent<InventoryGrid>();
            grid.inventory = inventory;

            grid.Init();

            width = grid.width;
            height = grid.height;
            size = new Vector2Int(width, height);

            slotsList = GetComponentsInChildren<InventorySlot>().ToList();
            slotsArray = new InventorySlot[width, height];

            ListToGridArray();

            foreach (var slot in slotsList) {
                SubscribeSlot(slot);
            }
            currentItems = inventory.currentItems;

            List<Item> copy = new List<Item>(currentItems);
            currentItems.Clear();
            foreach (var item in copy) {
                AddItem(item);
            }
            copy.Clear();

            overSeer.RegistrationContainer(inventory);
        }

        private void ListToGridArray() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    InventorySlot slot = slotsList[y * width + x];
                    slot.position = new Vector2Int(x, y);
                    slotsArray[x, y] = slot;
                }
            }
        }

        private void SubscribeSlot( InventorySlot slot ) {

            slot.OnHover.AddListener(OnPointerEnter);

            slot.OnDown.AddListener(OnPointerDown);

            slot.OnLeftClick.AddListener(OnPointerLeftClick);
            slot.OnMiddleClick.AddListener(OnPointerMiddleClick);
            slot.OnRightClick.AddListener(OnPointerRightClick);

            slot.OnUp.AddListener(OnPointerUp);
            slot.OnEndHover.AddListener(OnPointerExit);

            slot.OnBeginDragEvent.AddListener(OnBeginDrag);
            slot.OnDragEvent.AddListener(OnDrag);
            slot.OnEndDragEvent.AddListener(OnEndDrag);

            slot.OnDropEvent.AddListener(OnDrop);


        }
        private void UnSubscribeSlot( InventorySlot slot ) {
            slot.OnHover.RemoveAllListeners();
            slot.OnDown.RemoveAllListeners();
            slot.OnLeftClick.RemoveAllListeners();
            slot.OnMiddleClick.RemoveAllListeners();
            slot.OnRightClick.RemoveAllListeners();
            slot.OnUp.RemoveAllListeners();
            slot.OnEndHover.RemoveAllListeners();

            slot.OnBeginDragEvent.RemoveAllListeners();
            slot.OnDragEvent.RemoveAllListeners();
            slot.OnEndDragEvent.RemoveAllListeners();

            slot.OnDropEvent.RemoveAllListeners();
        }
        #endregion

        #region Functional
        
        /// <summary>
        /// Добавление предмета в инвентарь.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void AddItem( Item item ) {
            Item clone = item.GetItemCopy();

            for (int i = 0; i < currentItems.Count; i++) {
                if (currentItems[i].CompareItem(clone) == 1) {
                    //если true значит смог найти такой же предмет и положить туда количество
                    if (IncreaseItemCount(currentItems[i], clone.itemStackCount) == true) {
                        //обновляем модель
                        InventoryModel model = FindModelByItem(currentItems[i]);
                        model.RefreshModel();
                        return;
                    }
                }
            }

            if (AddItemXY(clone)) {
                ItemType type = clone.GetItemType();
                if (type is PouchType) {
                    PouchType pouchType = type as PouchType;
                    Inventory pouchPrefab = pouchType.inventory;
                    Inventory newpouch = HelpFunctions.TransformSeer.CreateObjectInParent(GetComponentInParent<Canvas>().transform, pouchPrefab.gameObject, pouchPrefab.InventoryName).GetComponent<Inventory>();
                    
                    newpouch.CreateNewID();
                    pouchType.inventoryReference = newpouch.inventoryID;
                }

                

            } else {
                Debug.LogError(item.name + " Can not assign this item");
            }
        }
        #region ItemWork
        private bool IncreaseItemCount(Item item, uint count) {
            if (item.itemStackSize == -1) {//объект "бесконечный", деньги
                item.itemStackCount += count;
                return true;
			} else {
                //int diff = item.itemStackSize - (int)item.itemStackCount;//сколько я ещё могу добавить в предмет
				//if (count <= diff) {
                  //  item.itemStackCount += count;
                    //return true;
                //} else {
                    //int diff2 = (int)count - diff;

                    //item.itemStackCount += (uint)diff;
                //}
            }
            return false;
        }
        private void DecreaseItemCount( Item item, uint count ) {

        }
        #endregion

        /// <summary>
        /// Удаление предмета из инвенторя.
        /// </summary>
        /// <param name="item"></param>
        public void DeleteItem( Item item ) {
            List<InventorySlot> slots = TakeSlotsByItem(item);

            for (int i = 0; i < slots.Count; i++) {
                slots[i].FreeItem();
            }
            slots.Clear();

            currentItems.Remove(item);
        }

        public void ShowContainer() {
            grid.ShowModels();
        }
        public void HideContainer() {
            grid.HideModels();
        }
        #endregion

        #region Events
        /// <summary>
        /// Наведение на предметы.
        /// </summary>
        public void HoverItem() {
            if (!overSeer.lastSlot.isEmpty()) {
                SelectItemSlots(overSeer.lastSlot);
            }
        }
        /// <summary>
        /// Подсветка посадки предмета.
        /// </summary>
        public void HoverLanding() {
            SelectLandingSlots(overSeer.lastSlot, overSeer.buffer.GetComponent<InventoryModel>().referenceItem.GetSize());
        }
        /// <summary>
        /// Подсветка курсора.
        /// </summary>
        public void HoverHightlightCursor(InventorySlot slot, Color highlight) {
            SelectSlot(slot, highlight);
        }
        private void ReloadHightLight() {
            DeselectAllSlots();
            SelectAllNotEmptySlots();
        }

        #region Slot Events
        public virtual void OnPointerEnter( PointerEventData eventData, InventorySlot slot ) {
            overSeer.lastSlot = slot;
            overSeer.whereNow = this;

            if (MenuOptions._instance.IsHide && !overSeer.isDrag) ToolTipShow();

            if (overSeer.isDrag) {//передвигает от фром
                if(overSeer.whereNow != null) {
					if (view.SolidHightlight) {
                        if (inventory.isRestrictions) {
                            Item item = overSeer.rootModel.referenceItem;
                            Inventory whereNow = overSeer.whereNow.inventory;
                            List<ItemType> storageTypes = whereNow.storageTypes;
                            if(storageTypes.Count == 0) {
                                overSeer.whereNow.SelectAllSlots(whereNow.view.highlightColor);
                                return;
							} else {
                                for (int i = 0; i < whereNow.storageTypes.Count; i++) {
                                    if (item.CompareItemType(whereNow.storageTypes[i]) == 1) {
                                        overSeer.whereNow.SelectAllSlots(whereNow.view.highlightColor);
                                        return;
                                    }
                                }
                                overSeer.whereNow.SelectAllSlots(whereNow.view.invalidColor);
                            }
                        }
                    } else {
                        overSeer.whereNow.HoverLanding();//и подсвечивает тот контейнер на который указывает
                    }
                }
            } else {
				if (view.SolidHightlight) {
					for(int i = 0; i < slotsList.Count;i++) {
                        HoverHightlightCursor(slotsList[i], view.disableColor);
                    }
				}
                if (view.HoverHightlight) {
                    HoverHightlightCursor(overSeer.lastSlot, view.hoverColor);
                }
                HoverItem();
            }
        }
        public void OnPointerDown( PointerEventData eventData, InventorySlot slot ) {
            if(overSeer.isDrag) ItemBackToRootSlot();
            if (!MenuOptions._instance.IsHide) return;

            overSeer.rootSlot = slot;//запомнили слот откуда взяли
        }

        float clicked = 0;
        float clicktime = 0;
        float clickdelay = 0.5f;
        public void OnPointerLeftClick( PointerEventData eventData, InventorySlot slot ) {
			#region DoubleClick
			clicked++;
            if (clicked == 1) clicktime = Time.time;

            if (clicked > 1 && Time.time - clicktime < clickdelay) {
                clicked = 0;
                clicktime = 0;
                Debug.Log("Double CLick: " + slot.gameObject.name);

            } else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
			#endregion

			MenuOptionsHide();
        }
        public void OnPointerMiddleClick( PointerEventData eventData, InventorySlot slot ) { }
        public void OnPointerRightClick( PointerEventData eventData, InventorySlot slot ) {
            if (overSeer.isDrag) return;
            if (slot.isEmpty()) { MenuOptionsHide(); return; }
            ToolTipHide();
            MenuOptionsShow();
        }

        public void OnPointerUp( PointerEventData eventData, InventorySlot slot ) { }
        public void OnPointerExit( PointerEventData eventData, InventorySlot slot ) {
            overSeer.lastSlot = slot;

            ToolTipHide();

            ReloadHightLight();

            overSeer.whereNow = null;
        }
        private void OnBeginDrag( PointerEventData eventData ) {
            if (!MenuOptions._instance.IsHide) return;
            if (overSeer.rootSlot.isEmpty()) return;

            overSeer.from = this;

            overSeer.isDrag = true;

            List<InventorySlot> slots = FindItemSlots(overSeer.rootSlot.Item);
            overSeer.rootSlot = slots[0];

            overSeer.rootModel = FindModelByItem(overSeer.rootSlot.Item);

            DeleteItem(overSeer.rootSlot.Item);

            overSeer.buffer = overSeer.rootModel.GetComponent<RectTransform>();

            #region Позиция в иерархии
            overSeer.buffer.SetAsLastSibling();
            grid.dragParent.SetAsLastSibling();
            #endregion
        }

		private void OnDrag( PointerEventData eventData ) {
            if (!MenuOptions._instance.IsHide) return;
            if (!overSeer.isDrag) return;

            Vector2 mousePos2D = Input.mousePosition;

            overSeer.buffer.position = mousePos2D;
                //overSeer.buffer.position = grid.RectSetPositionToWorld(mousePos2D);
        }
        private void OnEndDrag( PointerEventData eventData ) {//если дропнул на тот же слот откуда взял или дропнул не известно куда
            if (!MenuOptions._instance.IsHide) return;
            if (!overSeer.isDrag) return;

            if (overSeer.isDrag != false ) {
                ItemBackToRootSlot();

                print("ВЫБРОС " + overSeer.rootModel.referenceItem.itemName);
            }

            overSeer.from = null;
        }
        private void OnDrop( PointerEventData eventData ) {
            if (!MenuOptions._instance.IsHide) return;
            if (!overSeer.isDrag) return;

            Inventory whereNow = overSeer.whereNow.inventory;

            if (whereNow.isRestrictions) {
                Item item = overSeer.rootModel.referenceItem;
                Vector2 size = item.GetSize();
                List<ItemType> storageTypes = whereNow.storageTypes;
                if (storageTypes.Count == 0) { 
                    ItemPlaceOnSlotRestriction(whereNow.container);
                    return;
                } else {
                    for (int i = 0; i < storageTypes.Count; i++) {
                        if (item.CompareItemType(storageTypes[i]) == 1) {
                            ItemPlaceOnSlotRestriction(whereNow.container);
                            return;
                        }
                    }
                    ItemBackToRootSlot(overSeer.from.inventory.isRestrictions);
                    return;
                }
            } else {
                //+
                if (actionSelection == -1 ) {//нельзя
                    ItemBackToRootSlot();
                }
                //+
                if (actionSelection == 1) {//можно
                    ItemPlaceOnSlot();
                }

                if (actionSelection == 2) {//обмен

                    ItemBackToRootSlot();
                }
            }
        }

        private void ItemBackToRootSlot(bool isRestrictions  = false) {
            Item item = overSeer.rootModel.referenceItem;
            if (isRestrictions) {
                overSeer.from.ItemPlaceOnSlotRestriction(overSeer.from);
            } else {
                overSeer.from.AddItemOnPosition(item, overSeer.rootSlot);
            }
            overSeer.from.currentItems.Add(item);

            overSeer.isDrag = false;
            ReloadHightLight();
        }
        private void ItemPlaceOnSlot() {
            Item item = overSeer.rootModel.referenceItem;
            overSeer.whereNow.AddItemOnPosition(item, overSeer.lastSlot);
            overSeer.whereNow.currentItems.Add(item);

            overSeer.buffer.SetParent(overSeer.whereNow.grid.dragParent);

            overSeer.isDrag = false;

            ReloadHightLight();
        }
        private void ItemPlaceOnSlotRestriction(InventoryContainer container, bool recalculatePos = true) {
            Item item = overSeer.rootModel.referenceItem;
            List<InventorySlot> slots = container.slotsList;

            for (int i = 0; i < slots.Count; i++) {
                slots[i].AssignItem(item);
            }
            container.currentItems.Add(item);

            overSeer.buffer.SetParent(container.grid.dragParent);

            RectTransform rect = slots[0].GetComponent<RectTransform>();
            overSeer.buffer.position = rect.TransformPoint(rect.rect.center);

            if (recalculatePos) {
                if(container.inventory.isRestrictions)
                    grid.RecalculateCellPosition(overSeer.buffer, container.size);
                else
                    grid.RecalculateCellPosition(overSeer.buffer, item.GetSize());
            }

            overSeer.isDrag = false;

            ReloadHightLight();
        }

        /// <summary>
        /// Добавление предмета на определёную позицию в инвентаре.
        /// При перетаскивании
        /// </summary>
        /// <param name="item"></param>
        /// <param name="positionSlot"></param>
        private void AddItemOnPosition( Item item, InventorySlot positionSlot, bool recalculatePos = true ) {

            Vector2 size = item.GetSize();

            List<InventorySlot> slots = TakeSlotsBySize(positionSlot, size);

            if (slots.Count > 0) {
                for (int i = 0; i < slots.Count; i++) {
                    slots[i].AssignItem(item);
                }
            }
            slots.Clear();

            RectTransform rect = positionSlot.GetComponent<RectTransform>();
            overSeer.buffer.position = rect.TransformPoint(rect.rect.center);

            if (recalculatePos)
                grid.RecalculateCellPosition(overSeer.buffer, size);

            SelectAllNotEmptySlots();
        }

        private bool AddItemXY( Item item ) {

            Vector2 size = item.GetSize();

            for (int y = 0; y < height; y++) {

                for (int x = 0; x < width; x++) {

                    InventorySlot slot = slotsArray[x, y];

                    List<InventorySlot> neighbors = TakeSlotsForLanding(slot, size);

                    if (neighbors.Count > 0) {


                        for (int i = 0; i < neighbors.Count; i++) {//добавление

                            neighbors[i].HighlightColor = view.baseColor;
                            neighbors[i].AssignItem(item);

                            if (i == 0) {
                                overSeer.buffer = grid.CreateModelByItem(item);

                                AddItemOnPosition(item, neighbors[i], false);
                                grid.RecalculateCellProportion(overSeer.buffer, item.GetSize());
                            }
                        }
                        neighbors.Clear();

                        currentItems.Add(item);

                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
        #endregion

        #region Slots work
        private List<InventorySlot> TakeSlotsBySize( InventorySlot slot, Vector2 size ) {

            Vector2Int slotPosition = slot.position;

            List<InventorySlot> neighbors = new List<InventorySlot>();

            int neighborsCount = 0;

            /*for (int yy = 0; yy < size.y; yy++) {
                for (int xx = 0; xx < size.x; xx++) {
                    int x = slotPosition.x - xx;
                    int y = slotPosition.y - yy;
                    if (x >= width || y >= height || y < 0 || x < 0) {
                        continue;
                    }

                    InventorySlot neighbor = slotsArray[x, y];
                    neighborsCount++;
                    neighbors.Add(neighbor);
                }
            }*/

            //////////////////////////////////////////////////////////////////////////////////
            for (int yy = 0; yy < size.y; yy++) {
                for (int xx = 0; xx < size.x; xx++) {
                    int x = slotPosition.x + xx;
                    int y = slotPosition.y + yy;
                    if (x >= width || y >= height) {
                        continue;
                    }

                    InventorySlot neighbor = slotsArray[x, y];
                    neighborsCount++;
                    neighbors.Add(neighbor);
                }
            }
            return neighbors;
        }

        /// <summary>
        /// Возвращает объединение предметов по слотам.
        /// </summary>
        /// <param name="slots"></param>
        /// <returns></returns>
        private List<Item> TakeUnionSlots( List<InventorySlot> slots ) {

            List<Item> unionItems = new List<Item>();

            for (int i = 0; i < slots.Count; i++) {
                if (!slots[i].isEmpty()) {
                    if (!unionItems.Contains(slots[i].Item))
                        unionItems.Add(slots[i].Item);
                }
            }
            return unionItems;
        }

        /// <summary>
        /// Получаем слоты, соседи слота который передаём. Отбирает пустые слоты.
        /// </summary>
        private List<InventorySlot> TakeSlotsForLanding( InventorySlot slot, Vector2 size) {

            List<InventorySlot> neighbors = TakeSlotsBySize(slot, size);

            int emptyNeighbors = 0;

            for (int i = 0; i <neighbors.Count; i++) {
                if (neighbors[i].isEmpty()) {
                    emptyNeighbors++;
                }
            }
            if (emptyNeighbors != size.x * size.y) {
                neighbors.Clear();
            }

            if (neighbors.Count != size.x * size.y) {
                neighbors.Clear();
            }
            return neighbors;
        }

        protected List<InventorySlot> TakeSlotsByItem( Item item ) {
            string id = item.GetId();
            List<InventorySlot> slots = new List<InventorySlot>();

            for(int i = 0; i < slotsList.Count; i++) {
                if (slotsList[i].isEmpty()) continue;
                if(slotsList[i].Item.GetId() == id)
                    slots.Add(slotsList[i]);
            }
            return slots;
        }


        private InventoryModel FindModelByItem(Item item) {
            List<InventoryModel> models = grid.dragParent.GetComponentsInChildren<InventoryModel>().ToList();
            InventoryModel model = null;
            for (int i = 0; i < models.Count; i++) {
                if (models[i].reference == item.GetId()) {
                    model = models[i];
                    break;
                }
            }
            return model;
        }


        /// <summary>
        /// Находит при помощи предмета все его части.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private List<InventorySlot> FindItemSlots( Item item ) {
            List<InventorySlot> slots = new List<InventorySlot>();
            for (int i = 0; i < slotsList.Count; i++) {
                if (!slotsList[i].isEmpty()) {
                    if (slotsList[i].Item.GetId() == item.GetId()) {
                        slots.Add(slotsList[i]);
                    }
                }
            }

            return slots;
        }

        #region Select
        private void SelectSlot( InventorySlot slot, Color color) {
            slot.HighlightColor = color;
        }
        private void SelectSlots( List<InventorySlot> slots, Color color) {
            for (int i = 0; i < slots.Count; i++) {
                SelectSlot(slots[i], color);
            }
        }

        private void SelectAllSlots(Color color) {
            SelectSlots(slotsList, color);
        }
        /// <summary>
        /// Убирает подсветку с всех слотов.
        /// </summary>
        private void DeselectAllSlots() {
            SelectSlots(slotsList, view.normalColor);
        }

        /// <summary>
        /// Подсветка предмета при наведении на его слот.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="size"></param>
        private void SelectItemSlots( InventorySlot slot ) {

            List<InventorySlot> slots = FindItemSlots(slot.Item);

            if (slots.Count > 0) {
                SelectSlots(slots, view.hoverColor);
            }
            slots.Clear();
        }


        private int actionSelection = -1;
        private void SelectLandingSlots( InventorySlot slot, Vector2 size ) {

            List<InventorySlot> slots = TakeSlotsBySize(slot, size);
            List<Item> unionItems = TakeUnionSlots(slots);

            if (unionItems.Count == 0) {
                if (slots.Count == size.x * size.y) {
                    SelectSlots(slots, view.highlightColor);

                    actionSelection = 1;
                } else {
                    SelectSlots(slots, view.invalidColor);

                    actionSelection = -1;
                }
            } else if (unionItems.Count == 1) {
                List<InventorySlot> diffrentItemSlots = FindItemSlots(unionItems[0]);

                SelectSlots(diffrentItemSlots, view.replaceColor);
                SelectSlots(slots, view.replaceColor);

                actionSelection = 2;
            } else {
                for (int i = 0; i < unionItems.Count; i++) {
                    List<InventorySlot> diffrentItemSlots = FindItemSlots(unionItems[i]);

                    SelectSlots(diffrentItemSlots, view.invalidColor);
                }
                SelectSlots(slots, view.invalidColor);

                actionSelection = -1;
            }

            unionItems.Clear();
            slots.Clear();
        }
        private void SelectLandingModels( InventorySlot slot, Vector2 size ) {
            List<InventorySlot> slots = TakeSlotsBySize(slot, size);
            List<Item> unionItems = TakeUnionSlots(slots);

            if (unionItems.Count == 0) {
                if (slots.Count == size.x * size.y) {
                    SelectSlots(slots, view.hoverColor);

                    actionSelection = 1;
                } else {
                    SelectSlots(slots, view.invalidColor);

                    actionSelection = -1;
                }
            } else if (unionItems.Count == 1) {
                List<InventorySlot> diffrentItemSlots = FindItemSlots(unionItems[0]);

                SelectSlots(diffrentItemSlots, view.replaceColor);
                SelectSlots(slots, view.replaceColor);

                actionSelection = 2;
            } else {

                for (int i = 0; i < unionItems.Count; i++) {
                    InventoryModel model = FindModelByItem(unionItems[i]);
                    model.Hightlight.color = view.invalidColor;
                }
                /*for (int i = 0; i < unionItems.Count; i++) {
                    List<InventorySlot> diffrentItemSlots = FindItemSlots(unionItems[i]);

                    SelectSlots(diffrentItemSlots, view.invalidColor);
                }*/
                SelectSlots(slots, view.invalidColor);

                actionSelection = -1;
            }

            unionItems.Clear();
            slots.Clear();
        }

        

        /// <summary>
        /// Подсвечивает слоты с предметами.
        /// </summary>
        private void SelectAllNotEmptySlots() {
            for (int i = 0; i < slotsList.Count; i++) {
                if(!slotsList[i].isEmpty())
                    SelectSlot(slotsList[i], view.baseColor);
            }
        }
        private void SelectAllNotEmptyModels() {
            List<InventoryModel> models = grid.dragParent.GetComponentsInChildren<InventoryModel>().ToList();

            for (int i = 0; i < models.Count; i++) {
                models[i].Hightlight.color = view.baseColor;
            }
        }

        #endregion
        #endregion

        #region UIInteraction
        private void ToolTipShow() {

            if (overSeer.lastSlot.isEmpty()) return;
            RectTransform rectPos;

            Item item = overSeer.lastSlot.Item;
            List<InventorySlot> slots = TakeSlotsByItem(item);
            rectPos = slots[slots.Count - 1].GetComponent<RectTransform>();
            //ToolTip._instance.SetItem(item);

            slots.Clear();

            overSeer.OrderRefresh();

            ToolTip._instance.SetPosition(grid.RecalculatePositionToCornRect(rectPos, ToolTip._instance.rect));
            ToolTip._instance.ShowToolTip();
        }
        private void ToolTipHide() => ToolTip._instance.HideToolTip();


        private void MenuOptionsShow() {
            if (overSeer.lastSlot.isEmpty()) return;
            RectTransform rectPos;

            Item item = overSeer.lastSlot.Item;
            List<InventorySlot> slots = TakeSlotsByItem(item);
            rectPos = slots[slots.Count - 1].GetComponent<RectTransform>();

            slots.Clear();

            MenuOptions._instance.SetPosition(grid.RecalculatePositionToCornRect(rectPos, MenuOptions._instance.rect));

            overSeer.OrderRefresh();

            MenuOptions._instance.SetCurrentItem(item);
            MenuOptions._instance.OpenMenu();
        }
        private void MenuOptionsHide() => MenuOptions._instance.CloseMenu();

        #endregion
    }
}
