using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCloseBase : ButtonBase {

	/// <summary>
	/// ButtonCloseBase - скрипт предназначен только для закрытия объекта
	/// </summary>

	[Tooltip("Object to be opened, need object with IClousableUI")] public GameObject blank;

	private IClousableUI functional;

	protected override void OnEnable() {

		functional = blank.GetComponent<IClousableUI>();
	}


	protected override void ButtonEvent() {

		functional.Close();
	}
}
