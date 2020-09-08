using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpenCloseBase : ButtonBase {
	/// <summary>
	/// ButtonOpenCloseBase - скрипт предназначен для открытия закрытия объекта
	/// </summary>

	[Tooltip("Объект для открытия, обязательно с интерфейсом IClousableUI")] public GameObject blank;

	private IClousableUI functional;

	protected override void OnEnable() {

		functional = blank.GetComponent<IClousableUI>();
	}


	protected override void ButtonEvent() {
		if(functional.IsOpen()) functional.Close();
		else functional.Show();
	}
}
