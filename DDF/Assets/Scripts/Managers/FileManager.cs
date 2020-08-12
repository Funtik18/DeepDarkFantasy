using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DDF.IO {
    public class FileManager {
        public static string PERSISTENT_DATA_PATH { get { return Application.persistentDataPath; } }
        public static string DATA_PATH { get { return Application.dataPath; } }

        public static string EXP { get { return ".ddf"; } }

        public static string SAVE_PATH { get { return PERSISTENT_DATA_PATH + "/Data"; } }


        public readonly static string SETTINGS_FILE = "/settings.json";


        //public readonly static string CHARACTER_FILE = "/character" + EXP;


        /// <summary>
        /// C:/Users/Пользователь/AppData/LocalLow/God Valley/StartProject
        /// </summary>
        public static string settingsPath { get { return Path.Combine(PERSISTENT_DATA_PATH, SETTINGS_FILE); } }
        //public static string charactersPath = "Prefabs/Characters";

        public static void SaveFile( string path, string file,  string data, bool append = false ) {
            CheckFolder(path);
            string fullPath = Path.Combine(path, file);
            StreamWriter sw = new StreamWriter(fullPath, append);
            sw.WriteLine(data);
            sw.Close();
        }
        public static void SaveFileJSON(string filename, object obj, bool pretty = true ) {
            string data = JsonUtility.ToJson(obj, pretty);
            SaveFile(SAVE_PATH, filename+EXP, data);
        }

        public static string LoadFile( string path ) {
            string data;
            StreamReader sr = new StreamReader(path);
            data = sr.ReadToEnd();
            sr.Close();

            return data;
        }

        public static void CheckFolder(string path) {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Существует ли такой файл.
        /// </summary>
        public static bool IsFileExist( string path ) {
            if (File.Exists(path))
                return true;
            return false;
        }
        /// <summary>
        /// Удаление файла.
        /// </summary>
        public static void DeleteFile( string path ) {
            File.Delete(path);
        }
    }
}