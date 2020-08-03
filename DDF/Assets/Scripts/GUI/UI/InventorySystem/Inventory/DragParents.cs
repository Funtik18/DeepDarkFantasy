using UnityEngine;

namespace DDF.UI.Inventory {
    public class DragParents : MonoBehaviour {
        public static DragParents _instance;

		private void Awake() {
			_instance = this;
		}
	}
}