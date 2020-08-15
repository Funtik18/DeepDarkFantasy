namespace DDF.Character.Stats {
	public class Stat {
		public string statName;

		public virtual string Output() {
			return statName;
		}
	}
	public class StatInt : Stat {
		public int amount;

		string regex = "";

		public StatInt(string newName, int max, string _regex = "" ) {
			statName = newName;
			amount = max;
			regex = _regex;
		}
		public override string Output() {
			return statName + "|" + amount + regex;
		}
	}
	public class StatFloat : Stat {
		public float amount;

		string regex = "";

		public StatFloat( string newName, float max, string _regex = "" ) {
			statName = newName;
			amount = max;
			regex = _regex;
		}
		public override string Output() {
			return statName + "|" + amount + regex;
		}
	}
	public class StatRegularInt : StatInt {
		public int currentInamount;

		string regex = "";

		public StatRegularInt( string newName, int value, int max, string _regex = "/" ) : base(newName, max) {
			currentInamount = value;
			regex = _regex;
		}
		public override string Output() {
			return statName + "|" + currentInamount + regex + amount;
		}
	}
	public class StatRegularFloat : StatFloat {
		public float currentInamount;

		string regex = "";

		public StatRegularFloat( string newName, float value, float max, string _regex = "/" ) : base(newName, max) {
			currentInamount = value;
			regex = _regex;
		}
		public override string Output() {
			return statName + "|" + currentInamount + regex + amount;
		}
	}
}