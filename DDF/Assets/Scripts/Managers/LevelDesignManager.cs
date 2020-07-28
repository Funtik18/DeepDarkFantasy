using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDesignManager : MonoBehaviour {

    public static LevelDesignManager _instance;

	private void Awake() {
		_instance = this;
	}

	public Material material;
}
