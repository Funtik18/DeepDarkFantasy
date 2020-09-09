using System.Collections.Generic;
using DDF.Character;
using DDF.UI.Inventory.Items;
using UnityEngine;

namespace DDF.Environment {
    public class ItemPhysicalModel : MonoBehaviour {
		public Entity ownerEntity;
        public Item item;

        public List<Blade> blades;

		private void Start() {
			if (blades == null)
				blades = new List<Blade>();
			if(ownerEntity != null) {
				for (int i = 0; i < blades.Count; i++) {
					blades[i].Init(ownerEntity);
				}
				EnableModel(true);
			}
		}

		public void EnableModel(bool enable) {
			for(int i=0; i<blades.Count; i++) {
				blades[i].EnableBlade(enable);
			}
		}
	}
}

