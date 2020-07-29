using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Environment {
	public class FloorDesign : MonoBehaviour {
		[SerializeField]
		private Floor[] floors;

		private void Awake() {
			GetFloors();
		}

		public void GetFloors() {
			floors = GetComponentsInChildren<Floor>();
		}

		public void SetFloor(Material material) {
			GetFloors();
			for (int i = 0; i < floors.Length; i++) {
				floors[i].SetFloorMaterial(material);
			}
		}
	}
}