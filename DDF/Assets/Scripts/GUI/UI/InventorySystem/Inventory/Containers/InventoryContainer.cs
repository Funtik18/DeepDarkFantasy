using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DDF.Inventory {
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(InventoryGrid))]
    [DisallowMultipleComponent]
    [AddComponentMenu("Inventory/Container", 2)]
    [Serializable]
    public class InventoryContainer : MonoBehaviour {

        public InventoryView view;

        private InventoryGrid grid;
        private int width, height;

        [HideInInspector] public InventorySlot[,] slotsArray;
        [HideInInspector] public List<InventorySlot> slotsList;

        public List<Item> startItems = new List<Item>();

        private List<Item> currentItems = new List<Item>();



        #region Settup
        
        
        private void Awake() {

            grid = GetComponent<InventoryGrid>();
            grid.view = view;
        }
        private void Start() {
            width = grid.width;
            height = grid.height;

            slotsList = GetComponentsInChildren<InventorySlot>().ToList();
            slotsArray = new InventorySlot[width, height];

            ListToGridArray();

            foreach (var slot in slotsList) {
                SubscribeSlot(slot);
            }

            foreach (var item in startItems) {
                AddItem(item);
            }
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
            slot.OnClick.AddListener(OnPointerClick);
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
            slot.OnClick.RemoveAllListeners();
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
            if (!AddItemXY(item)) {
                print(item.name + " Can not assign this item");
            }
        }
        private bool AddItemXY( Item item ) {

            Vector2 size = item.size;

            for (int y = 0; y < height; y++) {

                for (int x = 0; x < width; x++) {

                    InventorySlot slot = slotsArray[x, y];

                    List<InventorySlot> neighbors = TakeSlotsForLanding(slot, size);

                    if (neighbors.Count > 0) {

                        Item clone = item.GetItem();

                        for (int i = 0; i < neighbors.Count; i++) {//добавление

                            neighbors[i].HighlightColor = view.baseColor;
                            neighbors[i].AssignItem(clone);

                            if (i == 0) {
                                RectTransform rect = SetupDraggedModel(clone);
                                rect.position = neighbors[i].GetComponent<RectTransform>().TransformPoint(neighbors[i].GetComponent<RectTransform>().rect.center);

                                grid.RecalculateCellProportion(rect, clone.size);
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
        /// <summary>
        /// TODO: все проверки должны быть здесь
        /// </summary>
        /// <param name="item"></param>
        /// <param name="positionSlot"></param>
        private void AddItemOnPosition( Item item, InventorySlot positionSlot ) {

            List<InventorySlot> slots = TakeSlotsBySize(positionSlot, item.size);

            RectTransform rect = positionSlot.GetComponent<RectTransform>();
            buffer.position = rect.TransformPoint(rect.rect.center);
            grid.RecalculateCellPosition(buffer, item.size);

            if (slots.Count > 0) {

                for (int i = 0; i < slots.Count; i++) {
                    slots[i].AssignItem(item);
                }
            }

            slots.Clear();



            SelectAllNotEmptySlots();
        }



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
        }

        #endregion


        #region Events

        RectTransform buffer;


        InventorySlot rootSlot;
        InventorySlot lastSlot;
        InventoryModel lastModel;
        InventoryModel rootModel;
        bool canDrag = false;
        bool isDrag = false;

        public void OnPointerEnter( PointerEventData eventData, InventorySlot slot ) {

            lastSlot = slot;


            if (!lastSlot.isEmpty()) {

                #region Наведение на предметы
                if (view.SolidItemSlot) {
                    lastModel = FindModelByItem(lastSlot.Item);
                    lastModel.Hightlight.color = view.highlightColor;
				} else {
                    SelectItemSlots(lastSlot);

                }

                //ToolTipShow();
                #endregion
            }
            if (isDrag) {
				#region Наведение на предмет с предметом
				if (view.SolidItemSlot) {
                    SelectLandingModels(lastSlot, rootModel.referenceItem.size);
				} else {
                    SelectLandingSlots(lastSlot, buffer.GetComponent<InventoryModel>().referenceItem.size);
                }
				#endregion
			}
            if (view.HoverHightlight) {
                if (!isDrag) {
                    SelectSlot(lastSlot, view.hoverColor);
                }
            }

             
        }

		

        public void OnPointerDown( PointerEventData eventData, InventorySlot slot ) {
            rootSlot = slot;
        }
        public void OnPointerClick( PointerEventData eventData, InventorySlot slot ) {

        }
        public void OnPointerUp( PointerEventData eventData, InventorySlot slot ) {

        }
        public void OnPointerExit( PointerEventData eventData, InventorySlot slot ) {

			lastSlot = slot;

			if (view.SolidItemSlot) {
                if (!lastSlot.isEmpty()) {
                    lastModel = FindModelByItem(lastSlot.Item);
                    lastModel.Hightlight.color = view.baseColor;
                }
            } else {
                DeselectAllSlots();
                SelectAllNotEmptySlots();
            }

            // HideToolTip();
        }

        int indexOldSibling = -1;
        private void OnBeginDrag( PointerEventData eventData ) {
            if (rootSlot.isEmpty()) return;
            
            isDrag = true;

            List<InventorySlot> slots = FindItemSlots(rootSlot.Item);
            rootSlot = slots[0];

            rootModel = FindModelByItem(rootSlot.Item);
            if(view.SolidItemSlot)
                rootModel.Hightlight.color = view.normalColor;


            DeleteItem(rootSlot.Item);

            #region Позиция в иерархии
            buffer = rootModel.GetComponent<RectTransform>();
            indexOldSibling = buffer.GetSiblingIndex();
            buffer.SetAsLastSibling();
			#endregion

		}
		private void Update() {
            if (isDrag)
                if (Input.GetMouseButtonDown(1)) {
                    SetOrientation(rootModel.referenceItem);
                }
        }

        private void SetOrientation(Item item) {
            item.size = new Vector2Int(item.size.y, item.size.x);

		}


		private void OnDrag( PointerEventData eventData ) {
            if (!isDrag) return;
            Vector2 mousePos2D = Input.mousePosition;

            buffer.position = grid.RectSetPositionToWorld(mousePos2D);

			
        }
        private void OnEndDrag( PointerEventData eventData ) {//если дропнул на тот же слот откуда взял или дропнул не известно куда
            if (!isDrag) return;

            if (isDrag != false) {

                AddItemOnPosition(rootModel.referenceItem, rootSlot);
                DeselectAllSlots();

                isDrag = false;
            }

            buffer.SetSiblingIndex(indexOldSibling);

            if (view.SolidItemSlot) {
                SelectAllNotEmptyModels();
			} else {
                DeselectAllSlots();
                SelectAllNotEmptySlots();
            }
        }
        private void OnDrop( PointerEventData eventData ) {
            if (!isDrag) return;

            if (actionSelection == -1) {//нельзя
                AddItemOnPosition(rootModel.referenceItem, rootSlot);

                DeselectAllSlots();

                isDrag = false;
            }
            if (actionSelection == 1) {//можно
                AddItemOnPosition(rootModel.referenceItem, lastSlot);

                isDrag = false;
            }

            if(actionSelection == 2) {//обмен

                AddItemOnPosition(rootModel.referenceItem, rootSlot);
                DeselectAllSlots();

                /*List<InventorySlot> slots = TakeSlotsBySize(lastSlot, buffer.GetComponent<InventoryModel>().referenceItem.size);
                List<Item> unionItems = TakeUnionSlots(slots);

                Item copy = unionItems[0].GetItem(true);
                DeleteItem(unionItems[0]);
                AddItemOnPosition(lastSlot);

                DeselectAllSlots();

                InventoryModel model = FindModelByItem(copy);
                model.referenceItem = copy;

                buffer = model.GetComponent<RectTransform>();*/




                isDrag = false;
            }

            if (view.SolidItemSlot) {
                SelectAllNotEmptyModels();
            } else {
                DeselectAllSlots();
                SelectAllNotEmptySlots();
            }
        }

        
        
        /* InventoryModel model = buffer.GetComponent<InventoryModel>();

             List<InventorySlot> slots = TakeSlotsBySize(positionSlot, model.referenceItem.size);

             buffer.position = positionSlot.GetComponent<RectTransform>().TransformPoint(positionSlot.GetComponent<RectTransform>().rect.center);

             grid.RecalculateCellPosition(buffer, model.referenceItem.size);


             if (slots.Count > 0) {

                 for (int i = 0; i < slots.Count; i++) {
                     slots[i].AssignItem(model.referenceItem);
                 }
             }

             slots.Clear();*/
        


        #region ToolTip
        private void ToolTipShow() {

            Item item = lastSlot.Item;


            List<InventorySlot> slots = TakeSlotsByItem(item);

            RectTransform rect = slots[slots.Count - 1].GetComponent<RectTransform>();

            
            ToolTip._instance.SetItem(item);
            ToolTip._instance.SetPosition(grid.RecalculateToolTipPosition(rect));
            ToolTip._instance.ShowToolTip();

            slots.Clear();
        }
        private void HideToolTip() => ToolTip._instance.HideToolTip();

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
        
        private List<InventorySlot> TakeSlotsByItem( Item item ) {
            string id = item.GetId();
            List<InventorySlot> slots = new List<InventorySlot>();

            for(int i = 0; i < slotsList.Count; i++) {
                if (slotsList[i].isEmpty()) continue;
                if(slotsList[i].Item.GetId() == id)
                    slots.Add(slotsList[i]);
            }
            return slots;
        }

       


        private RectTransform SetupDraggedModel(Item item) {
            RectTransform rect = grid.CreateModelByItem(item, view.SolidItemSlot);
            return rect;
        }

        private InventoryModel FindModelByItem(Item item) {
            List<InventoryModel> models = view.dragParent.GetComponentsInChildren<InventoryModel>().ToList();
            InventoryModel model = null;
            for (int i = 0; i < models.Count; i++) {
                if (models[i].reference == item.GetId()) {
                    model = models[i];
                    break;
                }
            }
            return model;
        }

        #region Select

        private int actionSelection = -1;

        private void SelectSlot( InventorySlot slot, Color color) {
            slot.HighlightColor = color;
        }
        private void SelectSlots( List<InventorySlot> slots, Color color) {
            for (int i = 0; i < slots.Count; i++) {
                SelectSlot(slots[i], color);
            }

            slots.Clear();
        }


        /// <summary>
        /// Находит при помощи предмета все его части.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private List<InventorySlot> FindItemSlots(Item item) {
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
        /// <summary>
        /// Подсветка предмета при наведении на его слот.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="size"></param>
        private void SelectItemSlots( InventorySlot slot) {

            List<InventorySlot> slots = FindItemSlots(slot.Item);

            if (slots.Count > 0) {
                for (int i = 0; i < slots.Count; i++) {
                    SelectSlot(slots[i], view.hoverColor);
                }
			}

            slots.Clear();
        }
        //intersection

        

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


        private void DeselectAllSlots() {
            for (int i = 0; i < slotsList.Count; i++) {
                SelectSlot(slotsList[i], view.normalColor);
            }
        }
        private void SelectAllNotEmptySlots() {
            for (int i = 0; i < slotsList.Count; i++) {
                if(!slotsList[i].isEmpty())
                    SelectSlot(slotsList[i], view.baseColor);
            }
        }

        private void SelectAllNotEmptyModels() {
            List<InventoryModel> models = view.dragParent.GetComponentsInChildren<InventoryModel>().ToList();

            for (int i = 0; i < models.Count; i++) {
                models[i].Hightlight.color = view.baseColor;
            }
        }

        #endregion
        #endregion
    }
}
