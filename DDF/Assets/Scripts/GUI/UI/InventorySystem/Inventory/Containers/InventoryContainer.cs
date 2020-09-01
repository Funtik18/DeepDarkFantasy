using DDF.Character.Effects;
using DDF.Events;
using DDF.Help;
using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DDF.UI.Inventory {
    /// <summary>
    /// TODO: обновлять, сортировать currentItems
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(InventoryGrid))]
    [DisallowMultipleComponent]
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
        private List<InventoryModel> currentModels;

        #region Settup

        public void Init() {
            currentModels = new List<InventoryModel>();

            inventory = GetComponentInParent<Inventory>();

            grid = GetComponent<InventoryGrid>();
            grid.inventory = inventory;
            if (inventory.isGUI)
                grid.rootDragParents = DragParentsGUI._instance; 
            else
                grid.rootDragParents = DragParentsUI._instance;

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
                inventory.AddItem(item, true);
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
        public Item AddItem( Item item, bool enableModel) {
            if(AddItemXY(item, enableModel) == 0) {
                inventory.isFull = true;
            }
            return item;
        }

        #region ItemWork
        private void AddCurrentItem( Item item ) {
            MenuOptions._instance.ItemTagSetup(item, inventory.inventorytype);
            currentItems.Add(item);
        }
        private void RemoveCurrentItem( Item item ) {
            currentItems.Remove(item);
        }

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

            RemoveCurrentItem(item);

            ReloadHightLight();
        }
        public void DeleteModel( Item item ) {
            InventoryModel model = FindModelByItem(item);
            currentModels.Remove(model);
            Help.HelpFunctions.TransformSeer.DestroyObject(model.gameObject);
        }

        public void ShowContainer() {
            for(int i = 0; i < currentModels.Count; i++) {
                currentModels[i].ShowModel();
            }
        }
        public void HideContainer() {
            for (int i = 0; i < currentModels.Count; i++) {
                currentModels[i].HideModel();
            }
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
            overSeer.whereNow = inventory;

            if (MenuOptions._instance.IsHide && !overSeer.isDrag) ToolTipShow();

            if (overSeer.isDrag) {//передвигает от фром
                if(overSeer.whereNow != null) {
                    Inventory whereNow = overSeer.whereNow;
                    InventoryContainer whereNowcontainer = whereNow.container;
                    if (view.SolidHightlight) {
                        if (inventory.inventorytype == InventoryTypes.Equipment) {
                            Item item = overSeer.rootModel.referenceItem;
                            
                            List<StorageTypes> storageTypes = whereNow.storageTypes;
                            if(storageTypes.Count == 0) {
                                whereNowcontainer.SelectAllSlots(whereNow.view.highlightColor);
                                return;
							} else {
                                for (int i = 0; i < storageTypes.Count; i++) {
                                    if (item.Equals(storageTypes.ToString())) {
                                        whereNowcontainer.SelectAllSlots(whereNow.view.highlightColor);
                                        return;
                                    }
                                }
                                whereNowcontainer.SelectAllSlots(whereNow.view.invalidColor);
                            }
                        }
                    } else {
                        whereNowcontainer.HoverLanding();//и подсвечивает тот контейнер на который указывает
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
        public void OnPointerLeftClick( PointerEventData eventData, InventorySlot slot ) {
            if (slot.isEmpty()) return;

            int clickCount = eventData.clickCount;
            if (clickCount == 1) {

			} 
            else if (clickCount == 2) {
                Item item = slot.Item;
                MenuOptions._instance.DetermineAction(item.primaryTag)?.Invoke(item, inventory);
            } else if (clickCount > 2) {

			}
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

            overSeer.from = inventory;

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

            Inventory whereNow = overSeer.whereNow;
            if (whereNow.isDisposer) {//удаление из инвентаря
                ItemPlaceOnSlot(overSeer.from.container, overSeer.whereNow.container, overSeer.rootModel.referenceItem, overSeer.buffer.GetComponent<InventoryModel>());
                overSeer.whereNow.DeleteItem(overSeer.rootModel.referenceItem);
                return;
            }
            if (whereNow.inventorytype == InventoryTypes.Equipment) {
				if (!whereNow.IsEmpty) {
                    ItemBackToRootSlot(overSeer.from.inventorytype == InventoryTypes.Equipment);
                    return;
                }

                InventoryModel model = overSeer.rootModel;
                Item item = model.referenceItem;
                List<StorageTypes> storageTypes = whereNow.storageTypes;
                if (storageTypes.Count == 0) { 
                    ItemPlaceOnSlotRestriction(overSeer.from.container, overSeer.whereNow.container, item, model);
                    return;
                } else {
                    for (int i = 0; i < storageTypes.Count; i++) {
                        if (item.Equals(storageTypes[i].ToString())) {
                            ItemPlaceOnSlotRestriction(overSeer.from.container, overSeer.whereNow.container, item, model);
                            return;
                        }
                    }
                    ItemBackToRootSlot(overSeer.from.inventorytype == InventoryTypes.Equipment);
                    return;
                }
            } else {
                //+
                if (actionSelection == -1 ) {//нельзя
                    ItemBackToRootSlot();
                }
                //+
                if (actionSelection == 1) {//можно
                    ItemPlaceOnSlot(overSeer.from.container, overSeer.whereNow.container, overSeer.rootModel.referenceItem, overSeer.buffer.GetComponent<InventoryModel>());
                }

                if (actionSelection == 2) {//обмен

                    ItemBackToRootSlot();
                }
            }
        }

        /// <summary>
        /// TODO: проредить
        /// </summary>
        /// <param name="isRestrictions"></param>
        private void ItemBackToRootSlot(bool isRestrictions  = false) {
            InventoryContainer from = overSeer.from.container;
            InventoryModel model = overSeer.rootModel;
            Item item = model.referenceItem;
            if (isRestrictions) {
                List<InventorySlot> slots = from.slotsList;

                for (int i = 0; i < slots.Count; i++) {
                    slots[i].AssignItem(item);
                }

                RectTransform rect = slots[0].GetComponent<RectTransform>();
                model.transform.SetParent(from.grid.dragParent);
                model.transform.position = rect.TransformPoint(rect.rect.center);

                if (isRestrictions) {
                    grid.RecalculateCellPosition(overSeer.buffer, from.size);
                }

                overSeer.isDrag = false;

            } else {
                from.AddItemOnPosition(item, overSeer.rootSlot);
            }
            from.AddCurrentItem(item);

            overSeer.isDrag = false;
            from.ReloadHightLight();
        }
        private void ItemPlaceOnSlot( InventoryContainer from, InventoryContainer to, Item item, InventoryModel model) {
            to.AddItemOnPosition(item, overSeer.lastSlot);
            to.AddCurrentItem(item);

            overSeer.from.container.currentModels.Remove(model);
            model.transform.SetParent(to.grid.dragParent);
            to.currentModels.Add(model);

            overSeer.isDrag = false;

            ReloadHightLight();
        }
        private void ItemPlaceOnSlotRestriction( InventoryContainer from, InventoryContainer to, Item item, InventoryModel model, bool recalculatePos = true) {
            List<InventorySlot> slots = to.slotsList;

            for (int i = 0; i < slots.Count; i++) {
                slots[i].AssignItem(item);
            }
            to.AddCurrentItem(item);

            from.currentModels.Remove(model);

            RectTransform rect = slots[0].GetComponent<RectTransform>();
            model.transform.SetParent(to.grid.dragParent);
            model.transform.position = rect.TransformPoint(rect.rect.center);

            to.currentModels.Add(model);

            if (recalculatePos) {
                if(to.inventory.inventorytype == InventoryTypes.Equipment)
                    grid.RecalculateCellPosition(overSeer.buffer, to.size);
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

        private int AddItemXY( Item item, bool enableModel) {

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
                                InventoryModel newModel = grid.CreateModelByItem(item);
                                currentModels.Add(newModel);

                                if (enableModel) newModel.ShowModel();
                                else newModel.HideModel();

                                overSeer.buffer = newModel.transform as RectTransform;

                                AddItemOnPosition(item, neighbors[i], false);
                                grid.RecalculateCellProportion(overSeer.buffer, item.GetSize());
                            }
                        }
                        neighbors.Clear();

                        AddCurrentItem(item);
                        return 1;
                    }
                }
            }
            return 0;
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

        private InventoryModel FindModelByItem( Item item ) {
            InventoryModel model = null;
            for (int i = 0; i < currentModels.Count; i++) {
                if(currentModels[i].reference == item.GetId()) {
                    model = currentModels[i];
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


        /// <summary>
        /// Подсвечивает слоты с предметами.
        /// </summary>
        private void SelectAllNotEmptySlots() {
            for (int i = 0; i < slotsList.Count; i++) {
                if(!slotsList[i].isEmpty())
                    SelectSlot(slotsList[i], view.baseColor);
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

            MenuOptions._instance.SetCurrentItem(item, inventory);
            MenuOptions._instance.OpenMenu();
        }
        private void MenuOptionsHide() => MenuOptions._instance.CloseMenu();

        #endregion
    }
}
