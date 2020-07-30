#define UNITY_EDITOR
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace DDF.Editor.GitIntegration {
    /// <summary>
    /// This class sets the current UnityYAMLMerge driver in the git project settings
    /// when updating scripts or changing the Unity Editor version.
    /// </summary>
    /// See more: <see cref="https://docs.unity3d.com/2020.1/Documentation/Manual/SmartMerge.html"/>
    [InitializeOnLoad]
    public class SmartMergeRegistrator {
        const string SmartMergeRegistratorEditorPrefsKey = "smart_merge_installed";
        const int Version = 1;
        static readonly string VersionKey = $"{Version}_{Application.unityVersion}";

        public static string ExecuteGitWithParams( string param ) {
            var processInfo = new System.Diagnostics.ProcessStartInfo("git") {
                UseShellExecute = false,
                WorkingDirectory = System.Environment.CurrentDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var process = new System.Diagnostics.Process {
                StartInfo = processInfo
            };
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = param;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new Exception(process.StandardError.ReadLine());

            return process.StandardOutput.ReadLine();
        }

        [MenuItem("Tools/Git/SmartMerge registration")]
        static void SmartMergeRegister() {
            try {
                var UnityYAMLMergePath = EditorApplication.applicationContentsPath + "/Tools" + "/UnityYAMLMerge.exe";
                ExecuteGitWithParams("config merge.unityyamlmerge.name \"Unity SmartMerge (UnityYamlMerge)\"");
                ExecuteGitWithParams($"config merge.unityyamlmerge.driver \"\\\"{UnityYAMLMergePath}\\\" merge -h -p --force --fallback none %O %B %A %A\"");
                ExecuteGitWithParams("config merge.unityyamlmerge.recursive binary");
                EditorPrefs.SetString(SmartMergeRegistratorEditorPrefsKey, VersionKey);
                Debug.Log($"Succesfuly registered UnityYAMLMerge with path {UnityYAMLMergePath}");
            } catch (Exception e) {
                Debug.Log($"Fail to register UnityYAMLMerge with error: {e}");
            }
        }

        //Unity calls the static constructor when the engine opens
        static SmartMergeRegistrator() {
            var instaledVersionKey = EditorPrefs.GetString(SmartMergeRegistratorEditorPrefsKey);
            if (instaledVersionKey != VersionKey)
                SmartMergeRegister();
        }
    }
}
#endif