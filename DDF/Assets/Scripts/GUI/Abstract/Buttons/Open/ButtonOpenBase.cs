using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpenBase : ButtonBase {

	/// <summary>
	/// ButtonOpenBase - скрипт предназначен только для открытия объекта
	/// </summary>

	[Tooltip("Object to be opened, need object with IClousableUI")] public GameObject blank;

	private IClousableUI functional;

	protected override void OnEnable() {

		functional = blank.GetComponent<IClousableUI>();
	}


	protected override void ButtonEvent() {

		functional.Show();
	}
}
