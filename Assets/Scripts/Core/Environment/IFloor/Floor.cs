using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Environment {

	[RequireComponent(typeof(MeshRenderer))]
	public class Floor : MonoBehaviour {

		private MeshRenderer meshRenderer;

		private void Awake() {
			meshRenderer = GetComponent<MeshRenderer>();
		}

		public void SetFloorMaterial(Material material) {
			meshRenderer.material = material;
		}
	}

}
