using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory {
    public class DragParentsGUI : DragParents {
        public static new DragParentsGUI _instance { get; private set; }

        public static void Init() {
            if (_instance == null)
                _instance = FindObjectOfType<DragParentsGUI>();
        }
    }
}