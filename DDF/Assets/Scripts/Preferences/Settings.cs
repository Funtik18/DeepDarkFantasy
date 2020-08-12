using DDF.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    private static Settings _instance;

    private string fullPath;//где лежит файл настроек

    public GameOptions gameOptions { get; set; }

    public bool IsReady { get; private set; }


    public List<string> DisplayNames { get; private set; }
    public List<string> QualityNames { get; private set; }
    public List<Resolution> ResolutionSettings { get; private set; }

    /// <summary>
    /// Если не находит текущих настроек, то создаёт объект и экземляр класса.
    /// Объект не уничтожается при переходе на другие сцены.
    /// </summary>
    /// <returns></returns>
    public static Settings getInstance() {

        if (_instance == null) {
            _instance = GameObject.FindObjectOfType<Settings>();
            if (_instance == null) {
                GameObject go = new GameObject("GameSETTINGS");
                _instance = go.AddComponent<Settings>();

                DontDestroyOnLoad(go);
            }
        }
        return _instance;
    }

    private void Awake() {
        


        fullPath = FileManager.settingsPath;
        //Load strings
        QualityNames = new List<string>(QualitySettings.names);

        ResolutionSettings = new List<Resolution>(Screen.resolutions);

        DisplayNames = new List<string>();
        DisplayNames.Add(FullScreenMode.ExclusiveFullScreen.ToString());
        DisplayNames.Add(FullScreenMode.FullScreenWindow.ToString());
        DisplayNames.Add(FullScreenMode.MaximizedWindow.ToString());
        DisplayNames.Add(FullScreenMode.Windowed.ToString());

        //Load options
        gameOptions = LoadOptions();
        ApplySettings(gameOptions);

        IsReady = true;
    }

    /// <summary>
    /// Применить новые или старые натсройки
    /// </summary>
    public void ApplySettings() {
        ApplySettings(gameOptions);
    }
    private void ApplySettings( GameOptions options ) {
        QualitySettings.SetQualityLevel(options.quality);

        Screen.SetResolution(options.width, options.height, (FullScreenMode)options.fullscreen);
    }

    /// <summary>
    /// Сохранение новых настроек
    /// </summary>
    /// <param name="_quality"></param>
    /// <param name="_width"></param>
    /// <param name="_height"></param>
    /// <param name="_fullscreen"></param>
    public void SaveOptions( int _quality, int _width, int _height, int _fullscreen ) {

        GameOptions options = new GameOptions() {
            quality = _quality,
            width = _width,
            height = _height,
            fullscreen = _fullscreen
        };

        if (FileManager.IsFileExist(fullPath)) {//сперва удаляем старый файл
            FileManager.DeleteFile(fullPath);
        }

        string data = JsonUtility.ToJson(options, true);
        //FileManager.SaveFile(fullPath, fullPath, data);

        gameOptions = options;
    }
    /// <summary>
    /// Загрузка текущих или дефолтных настроек
    /// </summary>
    /// <returns></returns>
    public GameOptions LoadOptions() {

        if (FileManager.IsFileExist(fullPath)) {
            string json = FileManager.LoadFile(fullPath);
            return JsonUtility.FromJson<GameOptions>(json);
        }
        return new GameOptions() { //если нет предустановленых настроек то дефолтные как в unity
            quality = QualitySettings.GetQualityLevel(),
            width = Screen.width,
            height = Screen.height,
            fullscreen = (int)Screen.fullScreenMode
        };
    }

}
