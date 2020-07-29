using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Environment {

    [RequireComponent(typeof(MeshRenderer))]
    public class Wall : MonoBehaviour {
		private MeshRenderer meshRenderer;

		private void Awake() {
			MeshRenderer();
		}
		private void MeshRenderer() {
			if (meshRenderer == null) {
				meshRenderer = GetComponent<MeshRenderer>();
			}
		}
		public void SetWallMaterial( Material material ) {
			MeshRenderer();
			meshRenderer.material = material;
		}
	}
}