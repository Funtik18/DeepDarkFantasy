using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DDF.Environment {

	[RequireComponent(typeof(MeshRenderer))]
	public class Floor : MonoBehaviour {

		private MeshRenderer meshRenderer;

		private void Awake() {
			MeshRenderer();
		}
		private void MeshRenderer() {
			if(meshRenderer == null) {
				meshRenderer = GetComponent<MeshRenderer>();
			}
		}
		public void SetFloorMaterial(Material material) {
			MeshRenderer();
			meshRenderer.material = material;
		}
	}

}
