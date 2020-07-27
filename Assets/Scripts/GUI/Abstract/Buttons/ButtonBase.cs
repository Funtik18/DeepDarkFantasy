using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour, IPointerUI {

	/// <summary>
	/// ButtonBase - основа кнопок
	/// </summary>

	[Tooltip("Main button")] public Button main;


	protected virtual void Awake() {
		if (main == null)
			main = GetComponent<Button>();

		if (main != null)
			main.onClick.AddListener(delegate {
				ButtonEvent();
			});
	}
	protected virtual void OnEnable() { }


	protected virtual void ButtonEvent() { print("+"); }


	public virtual void OnPointerClick( PointerEventData eventData ) { }
	public virtual void OnPointerDown( PointerEventData eventData ) { }
	public virtual void OnPointerEnter( PointerEventData eventData ) { }
	public virtual void OnPointerExit( PointerEventData eventData ) { }
	public virtual void OnPointerUp( PointerEventData eventData ) { }
}
