using DDF.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Settings))]
[RequireComponent(typeof(InputManager))]
public class ManagersManager : MonoBehaviour {
	
	public static ManagersManager _instance;

	private Settings settings;
	private InputManager inputManager;


	private void Awake() {
		if (_instance == null) {
			_instance = this;
		}

		settings = GetComponent<Settings>();
		inputManager = GetComponent<InputManager>();


		DontDestroyOnLoad(this);
	}
}
