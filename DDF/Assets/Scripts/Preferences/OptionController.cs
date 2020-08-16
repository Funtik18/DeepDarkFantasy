using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionController : WindowBase {

	/// <summary>
	/// OptionController - взаимодействие с ui и принятие нового разрешения
	/// </summary>

/*	public static OptionController _instace;

	public TMPro.TMP_Dropdown display;//delete
	public  TMPro.TMP_Dropdown quality;//delete
	public TMPro.TMP_Dropdown resolution;//delete


	public Button btnDefault;
	public Button btnApply;
	public Button btnAccept;
	public Button btnCancel;


	Settings setting;
	GameOptions options;

	private void Awake() {
		_instace = this;

		btnDefault.onClick.AddListener(delegate { DefaultSettings(); });

		btnApply.onClick.AddListener(delegate { ApplySettings(); });

		btnAccept.onClick.AddListener(delegate { StartCoroutine(AcceptSettings()); });

		btnCancel.onClick.AddListener(delegate { StartCoroutine(CancelSettings()); });
	}

	private List<Resolution> filteredResolutions = new List<Resolution>();
	private int currentIndexResolution = -1;
	private int currentIndexDisplay = -1;

	private IEnumerator Start() {
		yield return null;
		setting = Settings.getInstance();
		while (!setting.IsReady) {
			yield return null;//wait one frame
		}

		options = setting.gameOptions;

		//ui
		//quality
		quality.ClearOptions();
		quality.AddOptions(setting.QualityNames);
		quality.value = options.quality;

		
		//resolution
		int index = 0;
		int lw = -1, lh = -1;
		List<string> templist = new List<string>();
		for (int i = 0; i < setting.ResolutionSettings.Count; i++) {
			Resolution temp = setting.ResolutionSettings[i];
			if (lw != temp.width && lh != temp.height) {
				lw = temp.width;
				lh = temp.height;

				if (lw == options.width && lh == options.height) {
					currentIndexResolution = index;
				}
				templist.Add($"{temp.width}X{temp.height}");//add string 
				filteredResolutions.Add(temp);
				index++;
			}
		}

		resolution.AddOptions(templist);
		resolution.value = currentIndexResolution;


		//fullscreen
		currentIndexDisplay = setting.DisplayNames.FindIndex(x => x == Screen.fullScreenMode.ToString());

		display.ClearOptions();

		display.AddOptions(setting.DisplayNames);
		display.value = currentIndexDisplay;


		templist.Clear();
	}

	protected override void OnDisable() {
		base.OnDisable();
		display = null;
		quality = null;
		resolution = null;

		btnDefault = null;
		btnApply = null;
		btnAccept = null;
		btnCancel = null;

		setting = null;
		options = null;
	}

	private void DefaultSettings() {

		quality.value = 5;//лучшее качество
		resolution.value = filteredResolutions.Count-1;//лучшее разрешение
		display.value = 3;//полный экран

		ApplySettings();
	}

	private IEnumerator AcceptSettings() {

		if (CheckChanges()) {//если есть изменения 
			MessageBox box = UIManager._instance.InstanceWindow();

			box.ShowDialoge("Accept changes?", "ATTENTION", MessageBoxButtons.YesNo);

			DialogResult result = box.GetResult();
			while (result==DialogResult.None) {
				result = box.GetResult();
				yield return null;//wait one frame				

			}
			
			if(result == DialogResult.Yes) {
				ApplySettings();
			} else {
				RejectSettings();
			}
			Close();
		} else {
			Close();
		}
		StopCoroutine(AcceptSettings());
	}

	private IEnumerator CancelSettings() {
		if (CheckChanges()) {//если есть изменения 
			//MessageBox box = UIManager._instance.InstanceWindow();

			//box.ShowDialoge("Dispose changes?", "ATTENTION", MessageBoxButtons.YesNo);

			DialogResult result = box.GetResult();
			while (result == DialogResult.None) {
				result = box.GetResult();
				yield return null;//wait one frame				

			}

			if (result == DialogResult.Yes) {
				RejectSettings();
				Close();
			}
		} else {
			Close();
		}
		StopCoroutine(CancelSettings());
	}


	/// <summary>
	/// Отмена действий
	/// </summary>
	private void RejectSettings() {//старые настройки
		quality.value = options.quality;
		resolution.value = currentIndexResolution;
		display.value = currentIndexDisplay;
	}

	/// <summary>
	/// Принятие действий
	/// </summary>
	private void ApplySettings() {

		Settings settings = Settings.getInstance();

		currentIndexResolution = resolution.value;

		int currentQuality = quality.value;
		Resolution currentResolution = filteredResolutions[currentIndexResolution];
		int currentDisplay = display.value;

		settings.SaveOptions(currentQuality, currentResolution.width, currentResolution.height, currentDisplay);
		settings.ApplySettings();
	}

	/// <summary>
	/// Если true то были изменения в настройках.
	/// </summary>
	/// <returns></returns>
	private bool CheckChanges() {
		if (quality.value != options.quality) return true;
		if (resolution.value != currentIndexResolution) return true;
		if (display.value != currentIndexDisplay) return true;

		return false;
	}
	*/
}
