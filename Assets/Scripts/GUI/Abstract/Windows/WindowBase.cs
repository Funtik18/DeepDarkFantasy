using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class WindowBase : MonoBehaviour, IClousableUI {

	[SerializeField] protected bool isOpen = false;
	[SerializeField] protected bool isCanBeIteractable = false;
	[SerializeField] protected bool isCanBlockRaycast = false;

	private CanvasGroup canvasGroup;

	protected virtual void OnEnable() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	protected virtual void Start() {

		if (!isOpen) Close();
		else Show();
	}

	protected virtual void OnDisable() {
		canvasGroup = null;
	}

	public virtual void Show() {
		canvasGroup.alpha = 1;
		if(isCanBeIteractable)
			canvasGroup.interactable = true;
		if (isCanBlockRaycast)
			canvasGroup.blocksRaycasts = true;


		isOpen = true;
	}
	public virtual void ShowDialoge() {
		Show();
	}
	public virtual void Close() {
		canvasGroup.alpha = 0;
		if(isCanBeIteractable)
			canvasGroup.interactable = false;
		if(isCanBlockRaycast)
			canvasGroup.blocksRaycasts = false;

		isOpen = false;
	}


	public bool IsOpen() {
		return isOpen;
	}
}
