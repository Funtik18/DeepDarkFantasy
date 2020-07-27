using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour {
    private readonly static string SETTINGS_FILE = "/settings.json";

    /// <summary>
    /// C:/Users/Пользователь/AppData/LocalLow/God Valley/StartProject
    /// </summary>
    public static string settingsPath { get { return Path.Combine(Application.persistentDataPath, SETTINGS_FILE); } private set { } }
    //public static string charactersPath = "Prefabs/Characters";

    public static void SaveFile( string path, string data, bool append = false ) {
        StreamWriter sw = new StreamWriter(path, append);
        sw.WriteLine(data);
        sw.Close();
    }
    public static string LoadFile( string path ) {
        string data = "";
        StreamReader sr = new StreamReader(path);
        data = sr.ReadToEnd();
        sr.Close();

        return data;
    }


    public static bool IsFileExist( string path ) {
        if (File.Exists(path))
            return true;
        return false;
    }
    public static void DeleteFile( string path ) {
        File.Delete(path);
    }
}
