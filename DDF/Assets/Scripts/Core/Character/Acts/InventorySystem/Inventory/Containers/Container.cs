using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DDF.UI.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DDF.UI.Inventory {

    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(InventoryGrid))]
    [DisallowMultipleComponent]
    public class Container : MonoBehaviour {
        /// <summary>
        /// Ссылка на инвентар текущего контейнера.
        /// </summary>
        [HideInInspector]public Inventory inventory;
        protected InventoryOverSeer overSeer { get { return inventory.overSeer; } }
        protected InventoryView view { get { return inventory.view; } }

        protected List<Item2DModel> currentModels;

        /// <summary>
        /// Разметка контейнера.
        /// </summary>
        [HideInInspector] public InventoryGrid grid;
        private int width, height;
        /// <summary>
        /// Размер контейнера, по слотам.
        /// </summary>
        [HideInInspector] public Vector2Int size;

        /// <summary>
        /// Двумерный массив слотов InventorySlot.
        /// </summary>
        [HideInInspector] public InventorySlot[,] slotsArray;
        /// <summary>
        /// Список слотов InventorySlot.
        /// </summary>
        [HideInInspector] public List<InventorySlot> slotsList;

        #region Public methods
        #region Initialization
        public void Init() {
            inventory = GetComponentInParent<Inventory>();
            grid = GetComponent<InventoryGrid>();

            currentModels = new List<Item2DModel>();

            //TODO
            if (inventory.isGUI)
                grid.Init(inventory, DragParentsGUI._instance);
            else
                grid.Init(inventory, DragParentsUI._instance);

            #region Init size
            width = grid.width;
            height = grid.height;
            size = new Vector2Int(width, height);
            #endregion
            #region Init slots
            slotsList = GetComponentsInChildren<InventorySlot>().ToList();
            foreach (var slot in slotsList) {
                SubscribeSlot(slot);
            }
            slotsArray = new InventorySlot[width, height];
            ListToGridArray(slotsList, slotsArray);
            #endregion
            #region Init items
            //currentItems = inventory.currentItems;
            List<Item> copy = new List<Item>(inventory.currentItems);
            inventory.currentItems.Clear();
            foreach (var item in copy) {
                inventory.AddItem(item, true);
            }
            copy.Clear();
            #endregion
            overSeer.RegistrationContainer(inventory);
        }

        private void SubscribeSlot(InventorySlot slot) {
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
        private void UnSubscribeSlot(InventorySlot slot) {
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
        private void ListToGridArray<T>(List<T> list, T[,] array) where T : InventorySlot {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    T item = list[y * width + x];
                    item.position = new Vector2Int(x, y);
                    array[x, y] = item;
                }
            }
        }
        #endregion

        /// <summary>
        /// Добавление предмета в инвентарь.
        /// </summary>
        public virtual Item AddItem(Item item, bool enableModel) {
            AddItemXY(item, item.GetSize(), enableModel);
            return item;
        }
        /// <summary>
        /// Удаление предмета из инвентаря.
        /// </summary>
        /// <param name="item"></param>
        public virtual void DeleteItem(Item item) {
            List<InventorySlot> slots = TakeSlotsByItem(item);

            for (int i = 0; i < slots.Count; i++) {
                slots[i].FreeItem();
            }
            slots.Clear();

            RemoveCurrentItem(item);
        }
        public virtual void DeleteModel(Item item) {
            Item2DModel model = FindModelByItem(item);
            currentModels.Remove(model);
            Help.HelpFunctions.TransformSeer.DestroyObject(model.gameObject);
        }

        public void AddCurrentItem(Item item) {
            //MenuOptions._instance.ItemTagSetup(item, inventory.inventorytype);
            inventory.currentItems.Add(item);
            inventory.onItemAdded?.Invoke(item, inventory);
        }
        private void RemoveCurrentItem(Item item) {
            inventory.currentItems.Remove(item);
            inventory.onItemRemoved?.Invoke(item, inventory);
        }

        public void AddModelItem(Item2DModel model) {
            currentModels.Add(model);
        }
        public void RemoveModelItem(Item2DModel model) {
            currentModels.Remove(model);
        }

        public void ShowContainer() {
            for (int i = 0; i < currentModels.Count; i++) {
                currentModels[i].ShowModel();
            }
        }
        public void HideContainer() {
            for (int i = 0; i < currentModels.Count; i++) {
                currentModels[i].HideModel();
            }
        }

        public virtual void ItemBackToRootSlot() { }
        #endregion


        #region Slot events
        #region Pointer
        protected virtual void OnPointerEnter(PointerEventData eventData, InventorySlot slot) {
            overSeer.lastSlot = slot;
            overSeer.whereNow = inventory;
        }
        protected virtual void OnPointerDown(PointerEventData eventData, InventorySlot slot) {
            if (overSeer.isDrag) ItemBackToRootSlot();
            overSeer.rootSlot = slot;//запомнили слот откуда взяли
        }
        protected virtual void OnPointerLeftClick(PointerEventData eventData, InventorySlot slot) { }
        protected virtual void OnPointerMiddleClick(PointerEventData eventData, InventorySlot slot) { }
        protected virtual void OnPointerRightClick(PointerEventData eventData, InventorySlot slot) { }

        protected virtual void OnPointerUp(PointerEventData eventData, InventorySlot slot) { }
        protected virtual void OnPointerExit(PointerEventData eventData, InventorySlot slot) {
            overSeer.lastSlot = slot;
            overSeer.whereNow = null;
        }
		#endregion
		#region Drag
		protected virtual void OnBeginDrag(PointerEventData eventData) {

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
        protected virtual void OnDrag(PointerEventData eventData) {
            if (!overSeer.isDrag) return;

            Vector2 mousePos2D = Input.mousePosition;

            overSeer.buffer.position = mousePos2D;
        }
        protected virtual void OnEndDrag(PointerEventData eventData) {//если дропнул на тот же слот откуда взял или дропнул не известно куда
            if (!overSeer.isDrag) return;

            if (overSeer.isDrag != false) {
                ItemBackToRootSlot();

                Debug.LogError("ВЫБРОС " + overSeer.rootModel.referenceItem.itemName);
            }

            overSeer.from = null;
        }
        protected virtual void OnDrop(PointerEventData eventData) { }
        #endregion
        #endregion

        #region Private methods
        #region Add
        protected int AddItemXY(Item item, Vector2 size, bool enableModel) {

            for (int y = 0; y < height; y++) {

                for (int x = 0; x < width; x++) {

                    InventorySlot slot = slotsArray[x, y];

                    List<InventorySlot> neighbors = TakeSlotsForLanding(slot, size);

                    if (neighbors.Count > 0) {

                        for (int i = 0; i < neighbors.Count; i++) {//добавление

                            neighbors[i].HighlightColor = view.baseColor;
                            neighbors[i].AssignItem(item);

                            if (i == 0) {
                                Item2DModel newModel = grid.CreateModelByItem(item);
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
        /// <summary>
        /// Добавление предмета на определёную позицию в инвентаре.
        /// При перетаскивании
        /// </summary>
        public void AddItemOnPosition(Item item, InventorySlot positionSlot, bool recalculatePos = true) {

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

            //SelectAllNotEmptySlots();
        }
        #endregion

        /// <summary>
        /// Возвращает объединение предметов по слотам.
        /// </summary>
        /// <param name="slots"></param>
        /// <returns></returns>
        protected List<Item> TakeUnionSlots(List<InventorySlot> slots) {

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
        /// Дропает айтем на определёную позицию в определённом контейнере.
        /// </summary>
        /// <param name="from">откуда взяли</param>
        /// <param name="to">куда положили</param>
        /// <param name="item"></param>
        /// <param name="model"></param>
        protected virtual void ItemPlaceOnSlot(Container from, Container to, Item item, Item2DModel model) {
             to.AddItemOnPosition(item, overSeer.lastSlot);
             to.AddCurrentItem(item);

             overSeer.from.container.currentModels.Remove(model);
             model.transform.SetParent(to.grid.dragParent);
             to.currentModels.Add(model);

             overSeer.isDrag = false;
        }
        /// <summary>
        /// Возвращает айтем туда, откуда взяли.
        /// </summary>


        protected List<InventorySlot> TakeSlotsByItem(Item item) {
            string id = item.GetId();
            List<InventorySlot> slots = new List<InventorySlot>();

            for (int i = 0; i < slotsList.Count; i++) {
                if (slotsList[i].isEmpty()) continue;
                if (slotsList[i].Item.GetId() == id)
                    slots.Add(slotsList[i]);
            }
            return slots;
        }
        protected List<InventorySlot> TakeSlotsBySize(InventorySlot slot, Vector2 size) {

            Vector2Int slotPosition = slot.position;

            List<InventorySlot> neighbors = new List<InventorySlot>();

            int neighborsCount = 0;

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
        /// Получаем слоты, соседи слота который передаём. Отбирает пустые слоты.
        /// </summary>
        protected List<InventorySlot> TakeSlotsForLanding(InventorySlot slot, Vector2 size) {

            List<InventorySlot> neighbors = TakeSlotsBySize(slot, size);

            int emptyNeighbors = 0;

            for (int i = 0; i < neighbors.Count; i++) {
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

        #region Select
        /// <summary>
		/// Находит при помощи предмета все его части.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected List<InventorySlot> FindItemSlots(Item item) {
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
        protected Item2DModel FindModelByItem(Item item) {
            Item2DModel model = null;
            for (int i = 0; i < currentModels.Count; i++) {
                if (currentModels[i].reference == item.GetId()) {
                    model = currentModels[i];
                    break;
                }
            }
            return model;
        }

        /// <summary>
        /// Возвращает слоты содержащие айтем.
        /// </summary>
        /// <returns></returns>
        protected List<InventorySlot> SelectAllNotEmptySlots() {
            List<InventorySlot> slots = new List<InventorySlot>();
            for (int i = 0; i < slotsList.Count; i++) {
                if (!slotsList[i].isEmpty()) {
                    slots.Add(slotsList[i]);
                }
            }
            return slots;
        }
        #endregion
        #endregion
    }
}