using DDF.Character;
using DDF.Inputs;
using DDF.UI.Inventory.Items;
using UnityEngine;

namespace DDF.Environment {
	/// <summary>
	/// Подойти, взять, посмотреть.
	/// </summary>
    public class WeaponInteraction : EquipmentInteraction {

		[SerializeField] private ItemPhysicalModel physicalModel;

		private int clicks = 0;
		private void Update() {
			if (IsInField) {
				if (Input.GetButtonDown(InputManager.ButtonUse)) {
					clicks++;
					if (clicks > 1) {
						//CloseChest();
						clicks = 0;
					} else {
						if(interactEntity)
							Take();
					}
				}
			}
		}

		private void Take() {
			Item item = physicalModel.item;
			if(item is WeaponItem) {
				bool result = (interactEntity as HumanoidEntity).Equip(item, null);
				if(result == false) {
					bool result2 = ( interactEntity as HumanoidEntity).Take (item, null);
					if(result2 == false) {
						print("Out");
					}
				}
			}
			Help.HelpFunctions.TransformSeer.DestroyObject(physicalModel.gameObject);
		}
	}
}