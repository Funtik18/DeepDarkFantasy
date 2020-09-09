using System.Collections;
using System.Collections.Generic;
using DDF.Inputs;
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
						Take();
					}
				}
			}
		}

		private void Take() {
			interactEntity.Equip(physicalModel.item, null);
			Help.HelpFunctions.TransformSeer.DestroyObject(physicalModel.gameObject);
		}
	}
}