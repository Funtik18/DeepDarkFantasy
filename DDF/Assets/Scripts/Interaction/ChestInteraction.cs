using DDF.Inputs;
using DDF.UI;
using DDF.UI.Inventory;
using DDF.UI.Inventory.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.Environment {
	/// <summary>
	/// TODO: почему то когда выходишь при открытом ui а потом входишь и пытаешься его ещё раз окрыть, то открывается с второго нажатия.
	/// </summary>
	public class ChestInteraction : CaseInteraction {
		public ChestInventory chestPrefab;

		public List<Item> startItems;

		private ChestInventory insInventory = null;
		private bool isCreated = false;

		private int clicks = 0;

		private void Update() {
			if (IsInField) {
				if (Input.GetButtonDown(InputManager.ButtonUse)) {
					clicks++;
					if (clicks > 1) {
						CloseChest();
						clicks = 0;
					} else {
						OpenChest();
					}
				}
			}
		}

		private void OpenChest() {
			if (isCreated == false) {
				insInventory = Help.HelpFunctions.TransformSeer.CreateObjectInParent(DinamicUI._instance.transform, chestPrefab.gameObject, chestPrefab.name).GetComponent<ChestInventory>();
				insInventory.currentItems = startItems;

				insInventory.buttonClose?.onClick.AddListener(() => CloseChest());
				insInventory.buttonTakeAll?.onClick.AddListener(() => EmptiedChest());

				isCreated = true;
			} else {
				insInventory.ShowInventory();
			}
		}
		private void CloseChest() {
			insInventory.HideInventory();
		}
		private void EmptiedChest() {
			Inventory inventory = InventoryOverSeerGUI._instance.mainInventory;
			List<Item> items = new List<Item>();
			for(int i = 0; i < insInventory.currentItems.Count; i++) {
				int result = inventory.AddItem(insInventory.currentItems[i], false);
				if(result == 1) {
					items.Add(insInventory.currentItems[i]);
				} else {
					Debug.LogError("error");
				}
			}
			for(int i = 0; i< items.Count; i++) {
				insInventory.DeleteItem(items[i]);
			}
		}

		public override void OpenCase() {
			base.OpenCase();
		}
		public override void StayCase() {
			base.StayCase();
		}
		public override void CloseCase() {
			base.CloseCase();
			CloseChest();
		}
	}
}