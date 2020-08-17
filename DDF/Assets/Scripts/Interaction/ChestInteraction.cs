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
		public Inventory chestPrefab;

		public List<Item> startItems;

		public Button closeButton;
		private Inventory insInventory = null;
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
				insInventory = Help.HelpFunctions.TransformSeer.CreateObjectInParent(DinamicUI._instance.transform, chestPrefab.gameObject, chestPrefab.name).GetComponent<Inventory>();
				insInventory.currentItems = startItems;
				//closeButton?.onClick.AddListener(() => CloseChest());
				isCreated = true;
			} else {
				insInventory.ShowInventory();
			}
		}
		private void CloseChest() {
			insInventory.HideInventory();
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