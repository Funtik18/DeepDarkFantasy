using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DDF.UI.ScrollView {
    public class ScrollViewManipulatorCustomization : MonoBehaviour {
        public Transform content;

        public bool isDestoyObjects = true;
        public bool isClickObjects = true;
        public bool isNoDuplicate = true;

        private List<GameObject> objs;

		private void Awake() {
            objs = new List<GameObject>();
		}
        public void AddObject( GameObject obj ) {
            if (isNoDuplicate)
                if (IsContains(obj.name)) return;
            objs.Add(obj);
        }
        public void RemoveObject( GameObject obj ) {
            if(isDestoyObjects)
                objs.Remove(obj);
        }
        public bool IsContains(string objName) {
            return objs.Find(x=> x.name == objName);
		}
    }
}