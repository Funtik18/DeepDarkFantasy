using UnityEngine;

namespace DDF {
    /// <summary>
    /// Класс ссылка, в этом объекте создаётся ui.
    /// </summary>
    public class DragParents : MonoBehaviour {
        public static DragParents _instance { get; private set; }
    }
}