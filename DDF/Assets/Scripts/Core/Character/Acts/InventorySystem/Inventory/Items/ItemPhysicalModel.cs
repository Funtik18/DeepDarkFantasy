using System.Collections.Generic;
using DDF.Atributes;
using DDF.Character;
using DDF.UI.Inventory.Items;
using UnityEngine;

namespace DDF.Environment {
	/// <summary>
	/// Физическая 3D модель для айтема
	/// </summary>
    public class ItemPhysicalModel : MonoBehaviour {
		public Entity owner;
        public WeaponItem item;

		public VarMinMax<float> itemDamage;
		[ReadOnly] public float currentMoveSpeed = 1;
		[HideInInspector] public bool bladeActive;
		public List<Blade> blades;

		public bool isEnable = true;

		private void Awake() {//если есть хозяин предмета
			oldPos = transform.position;
			if(owner == null) {
				owner = transform.root.GetComponent<Entity>();
			}

			itemDamage = item.damage;

			if (blades == null)
				blades = new List<Blade>();
			for (int i = 0; i < blades.Count; i++) {
				blades[i].onEnter = TakeDamage;
			}
			EnableModel(isEnable);
		}

		private Vector3 oldPos;
		private Vector3 newPos;
		private bool speedOrder;
		void FixedUpdate() {
			if (!bladeActive) return;
			if (speedOrder) {
				newPos = transform.position;
				currentMoveSpeed = Vector3.Distance(oldPos, newPos) * 10;
			} else
				oldPos = transform.position;
			speedOrder = !speedOrder;
		}
		private void TakeDamage(Entity entity) {
			if (!bladeActive) return;
			if (entity == null) return;
			if (entity == owner) return;
			if (currentMoveSpeed > 1) {
				entity.TakeDamage(/*currentMoveSpeed %*/ Random.Range(itemDamage.min, itemDamage.max));
			}
		}

		public void EnableModel(bool enable) {
			bladeActive = enable;

			for (int i=0; i<blades.Count; i++) {
				blades[i].EnableBlade(bladeActive);
			}
		}
	}
}

