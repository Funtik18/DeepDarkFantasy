using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF {
    public class DragParentsUI : DragParents {
        public static new DragParentsUI _instance { get; private set; }

        public static void Init() {
            if (_instance == null)
                _instance = FindObjectOfType<DragParentsUI>();
        }
    }
}
