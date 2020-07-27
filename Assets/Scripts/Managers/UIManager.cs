using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public static UIManager _instance;

	public Window BlackBlank = null;

	public MessageBox messageBox = null;



	private void Awake() {
		_instance = this;
	}


	public MessageBox InstanceWindow() {
		MessageBox box = Instantiate(messageBox);
		Transform child = box.transform;
		child.SetParent(transform);
		child.localPosition = Vector3.zero;
		child.localScale = Vector3.one;
		return box;
	}

	public void DisposeWindow(WindowBase window) {
		DestroyImmediate(window.gameObject);
	}
}
