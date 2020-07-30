using System;

namespace DDF.Atributes {
	public enum InfoBoxType {
		Normal,
		Warning,
		Error
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class InfoBoxAttribute : DrawerAttribute {
		public string Text { get; private set; }
		public InfoBoxType Type { get; private set; }

		public InfoBoxAttribute( string text, InfoBoxType type = InfoBoxType.Normal ) {
			Text = text;
			Type = type;
		}
	}
}
