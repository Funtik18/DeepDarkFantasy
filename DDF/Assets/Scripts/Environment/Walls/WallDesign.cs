using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Environment {
	public class WallDesign : MonoBehaviour {
		[SerializeField]
		private Wall[] walls;

		private void Awake() {
			GetWalls();
		}

		public void GetWalls() {
			walls = GetComponentsInChildren<Wall>();
		}

		public void SetWalls( Material material ) {
			GetWalls();
			for (int i = 0; i < walls.Length; i++) {
				walls[i].SetWallMaterial(material);
			}
		}
	}
}