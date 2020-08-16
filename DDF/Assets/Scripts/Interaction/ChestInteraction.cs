using DDF.Inputs;
using DDF.UI;
using DDF.UI.Inventory;
using DDF.UI.Inventory.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Environment {
	public class ChestInteraction : CaseInteraction {
		public Inventory chestPrefab;

		public List<Item> startItems;


		private Inventory insInventory = null;
		private bool isCreated = false;

		private void Start() {

		}

		private void OpenChest() {
			if (isCreated == false) {
				insInventory = Help.HelpFunctions.TransformSeer.CreateObjectInParent(DinamicUI._instance.transform, chestPrefab.gameObject, chestPrefab.name).GetComponent<Inventory>();
				insInventory.currentItems = startItems;
				isCreated = true;
			} else {
				Help.HelpFunctions.CanvasGroupSeer.EnableGameObject(insInventory.canvasGroup, true);
			}
		}
		private void CloseChest() {
			if (isCreated) {
				Help.HelpFunctions.CanvasGroupSeer.DisableGameObject(insInventory.canvasGroup);
			}
		}

		public override void OpenCase() {
			base.OpenCase();

			
		}
		public override void StayCase() {
			base.StayCase();
			InputManager.onUse = OpenChest;
		}
		public override void CloseCase() {
			base.CloseCase();
			InputManager.onUse = null;
			CloseChest();
		}
	}
}