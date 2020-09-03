using DDF.Atributes;
using DDF.Character.Perks;
using System.Collections.Generic;

namespace DDF.Character.Stats {
	public class Stat {
		[ReadOnly]
		public string statName;

		public virtual string Output() {
			return statName;
		}
	}
	//для entity
	public class StatInt : Stat {
		public int amount;

		string regex = "";

		/// <summary>
		/// Костыль спасающий от выхода за ноль при распределении доступных очков.
		/// </summary>
		public bool IsCanDecreace {
			get {
				return ( amount - calculateExtra() ) <= 1 ? false : true ;
			}
		}
		List<PerkInt> extra;

		public StatInt(string newName, int max, string _regex = "" ) {
			statName = newName;
			amount = max;
			regex = _regex;

			extra = new List<PerkInt>();
		}
		public void AddExtra( PerkInt perk ) {
			extra.Add(perk);
		}
		public override string Output() {
			int extr = calculateExtra();
			if (extr != 0)
				return statName + "|" + (amount - extr) + "<color=lightblue>(+" + extr + ")</color> = " + amount + regex;
			else
				return statName + "|" + amount + regex;
		}
		private int calculateExtra() {
			int extr = 0;
			for (int i = 0; i < extra.Count; i++) {
				extr += extra[i].amount;
			}
			return extr;
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

	//для item
	[System.Serializable]
	public class VarFloat : Stat {
		public float amount;

		string regex = "";
		public VarFloat(string statName, float amount, string regex = "") {
			this.statName = statName;
			this.amount = amount;
			this.regex = regex;
		}
		public override string Output() {
			return statName +  amount + regex;
		}
	}

	[System.Serializable]
	public class VarMinMaxInt : Stat {
		public int min;
		public int max;

		string regex = "";

		public VarMinMaxInt(string statName, int min, int max, string regex = "-") {
			this.statName = statName;
			this.min = min;
			this.max = max;
			this.regex = regex;
		}
		public override string Output() {
			return min + regex + max;
		}
	}

	[System.Serializable]
	public class VarMinMaxFloat : Stat {
		public float min;
		public float max;

		string regex = "";

		public VarMinMaxFloat( string statName, float min, float max, string regex = "-" ) {
			this.statName = statName;
			this.min = min;
			this.max = max;
			this.regex = regex;
		}
		public override string Output() {
			return min + regex + max;
		}
	}
	
}