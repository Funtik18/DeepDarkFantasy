using UnityEngine;

namespace DDF.UI.Inventory {
    /// <summary>
    /// Класс ссылка, в этом объекте создаётся ui.
    /// </summary>
    public class DragParents : MonoBehaviour {
        public static DragParents _instance { get; private set; }

        public static void Init() {
            if(_instance == null)
                _instance = FindObjectOfType<DragParents>();
        }
    }
}