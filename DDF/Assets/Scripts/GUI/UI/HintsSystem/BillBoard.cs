using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour {
    public Transform camTransform;


	private void Awake() {
        camTransform = Camera.main.transform;

    }

	void Start() {
    }

    void Update() {
    }
}
