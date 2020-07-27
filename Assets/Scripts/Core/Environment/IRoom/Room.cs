using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment {
    public class Room : MonoBehaviour {

		public Transform floorRoot;
		
		private Floor[] floor;


		private void Start() {

			floor = floorRoot.GetComponentsInChildren<Floor>();

			print(floor.Length);

			for (int i = 0; i < floor.Length; i++) {
				floor[i].SetFloorMaterial(LevelDesignManager._instance.material);
			}
		}
	}
}
