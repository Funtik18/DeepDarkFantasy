using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DDF.Inputs{
	public class InputManager : MonoBehaviour {

		public static InputManager _instance;

		[SerializeField]
		private KeyBinding curentKeys;

		private void Awake() {
			_instance = this;
		}


		public KeyCode GetKeyForAction( KeyBindingActions key ) {

			KeyBinding.Binding binding = FindKeyBinding(key);
			if (binding != null) {
				return binding.key;
			}

			return KeyCode.None;
		}

		public bool GetKeyDown( KeyBindingActions key ) {
			KeyBinding.Binding binding = FindKeyBinding(key);
			if (binding != null) {
				return Input.GetKeyDown(binding.key);
			}

			return false;
		}
		public bool GetKey( KeyBindingActions key ) {

			KeyBinding.Binding binding = FindKeyBinding(key);
			if (binding != null) {
				return Input.GetKey(binding.key);
			}


			return false;
		}
		public bool GetKeyUp( KeyBindingActions key ) {

			KeyBinding.Binding binding = FindKeyBinding(key);
			if (binding != null) {
				return Input.GetKeyUp(binding.key);
			}
			return false;
		}

		public KeyBinding.Binding FindKeyBinding( KeyBindingActions key ) { 
			KeyBinding.Binding binding = null;
			for ( int i = 0; i < curentKeys.bindings.Length; i++) {
				binding = curentKeys.bindings[i];
				if (binding.actions == key) {
					return binding;
				}
			}
			return null;
		}
	}
}