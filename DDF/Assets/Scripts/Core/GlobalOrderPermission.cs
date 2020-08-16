using DDF.UI.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF {
    public class GlobalOrderPermission : MonoBehaviour {
		private void Awake() {
			DragParents.Init();
		}
	}
}