using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDF.UI.Inventory;
using DDF.UI.Inventory.Items;

namespace DDF.Randomizer {
    public class GetMoney : MonoBehaviour {
        private Inventory main;

		public Item money;

		private void Start() {
			main = InventoryOverSeer._instance.mainInventory;

			money = Resources.Load<Item>("Prefabs/ASSETS/Items/Gold");
		}

		public void AddMoney() {
			main.container.AddItem(money);
		}

	}
}