using System.Collections.Generic;
using DDF.Character;
using DDF.UI.Inventory.Items;
using UnityEngine;

namespace DDF.Environment {
	/// <summary>
	/// Физическая 3D модель для айтема
	/// </summary>
    public class ItemPhysicalModel : MonoBehaviour {
        public WeaponItem item;

        public List<Blade> blades;

		public bool isEnable = true;

		private void Awake() {//если есть хозяин предмета

			if (blades == null)
				blades = new List<Blade>();
			for (int i = 0; i < blades.Count; i++) {
				blades[i].Init(item.damage);
			}
			EnableModel(isEnable);
		}

		public void EnableModel(bool enable) {
			for(int i=0; i<blades.Count; i++) {
				blades[i].EnableBlade(enable);
			}
		}
	}
}

