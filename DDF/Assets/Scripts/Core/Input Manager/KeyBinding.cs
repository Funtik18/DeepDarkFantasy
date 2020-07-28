using UnityEngine;

namespace DDF.Inputs {
    [CreateAssetMenu(fileName = "NewKeyBinding", menuName = "DDF/KeyBinding")]
    public class KeyBinding : ScriptableObject {

        [System.Serializable]
        public class Binding {
            public KeyBindingActions actions;
            public KeyCode key;
        }

        public Binding[] bindings;
    }
}