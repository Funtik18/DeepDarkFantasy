using DDF.Character;
using DDF.Inputs;
using DDF.UI.Inventory.Items;
using UnityEngine;

namespace DDF.Environment {
	/// <summary>
	/// Подойти, взять, посмотреть.
	/// </summary>
    public class WeaponInteraction : EquipmentInteraction {

		[SerializeField] private Item3DWeaponModel physicalModel;

		private int clicks = 0;
		private void Update() {
			if (IsInField) {
				if (Input.GetButtonDown(InputManager.ButtonUse)) {
					clicks++;
					if (clicks > 1) {
						clicks = 0;
					} else {
						if(interactEntity)
							Take();
					}
				}
			}
		}
		/// <summary>
		/// Подбор 3д предмета, если оружие то сразу надевает.
		/// </summary>
		private void Take() {
			if (physicalModel.owner) return;//если предмет уже чей то
			Item item = physicalModel.item;
			if(item is WeaponItem || item is ArmorItem) {
				bool result = (interactEntity as HumanoidEntity).Equip(item, null);
				if(result == false) {
					bool result2 = ( interactEntity as HumanoidEntity).Take(item, null);
					if(result2 == false) {
						Debug.LogError("Can't equip item and add it to inventory - " + item.itemName);
					}
				}
			}
			Help.HelpFunctions.TransformSeer.DestroyObject(physicalModel.gameObject);
		}

		protected override void OnEnter() {
			if(!physicalModel.owner)
				base.OnEnter();
		}
		protected override void OnStay() {
			if (!physicalModel.owner)
				base.OnStay();
		}
		protected override void OnExit() {
			if(!physicalModel.owner)
				base.OnExit();
		}
	}
}