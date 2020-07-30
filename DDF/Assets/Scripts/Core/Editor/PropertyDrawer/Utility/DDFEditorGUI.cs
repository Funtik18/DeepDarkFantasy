using UnityEditor;
using UnityEngine;

namespace DDF.Editor {
    public static class DDFEditorGUI {
		public static float GetIndentLength( Rect sourceRect ) {
			Rect indentRect = EditorGUI.IndentedRect(sourceRect);
			float indentLength = indentRect.x - sourceRect.x;

			return indentLength;
		}

		public static void HelpBox( Rect rect, string message, MessageType type, UnityEngine.Object context = null, bool logToConsole = false ) {
			EditorGUI.HelpBox(rect, message, type);

			if (logToConsole) {
				DebugLogMessage(message, type, context);
			}
		}
		private static void DebugLogMessage( string message, MessageType type, UnityEngine.Object context ) {
			switch (type) {
				case MessageType.None:
				case MessageType.Info:
				Debug.Log(message, context);
				break;
				case MessageType.Warning:
				Debug.LogWarning(message, context);
				break;
				case MessageType.Error:
				Debug.LogError(message, context);
				break;
			}
		}
	}
}