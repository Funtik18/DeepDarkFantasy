using DDF.Atributes;
using UnityEditor;
using UnityEngine;

namespace DDF.Editor {
	[CustomPropertyDrawer(typeof(InfoBoxAttribute))]
	public class InfoBoxDecoratorDrawer : DecoratorDrawer {
		public override float GetHeight() {
			return GetHelpBoxHeight();
		}

		public override void OnGUI( Rect rect ) {
			InfoBoxAttribute infoBoxAttribute = (InfoBoxAttribute)attribute;

			float indentLength = DDFEditorGUI.GetIndentLength(rect);
			Rect infoBoxRect = new Rect(
				rect.x + indentLength,
				rect.y,
				rect.width - indentLength,
				GetHelpBoxHeight() - 2.0f);

			DrawInfoBox(infoBoxRect, infoBoxAttribute.Text, infoBoxAttribute.Type);
		}

		private float GetHelpBoxHeight() {
			return EditorGUIUtility.singleLineHeight * 3.0f;
		}

		private void DrawInfoBox( Rect rect, string infoText, InfoBoxType infoBoxType ) {
			MessageType messageType = MessageType.None;
			switch (infoBoxType) {
				case InfoBoxType.Normal:
				messageType = MessageType.Info;
				break;

				case InfoBoxType.Warning:
				messageType = MessageType.Warning;
				break;

				case InfoBoxType.Error:
				messageType = MessageType.Error;
				break;
			}

			DDFEditorGUI.HelpBox(rect, infoText, messageType);
		}
	}
}
