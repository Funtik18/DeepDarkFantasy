using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
	public class Stat {
		public string name;
	}
	public class StatInt : Stat {
		public int amount;

		public StatInt(string newName, int max ) {
			name = newName;
			amount = max;
		}
	}
	public class StatFloat : Stat {
		public float amount;

		public StatFloat( string newName, float max ) {
			name = newName;
			amount = max;
		}
	}
	public class StatRegularInt : StatInt {
		public int currentInamount;
		public StatRegularInt( string newName, int value, int max ) : base(newName, max) {
			currentInamount = value;
		}
	}
	public class StatRegularFloat : StatFloat {
		public float currentInamount;
		public StatRegularFloat( string newName, float value, float max ) : base(newName, max) {
			currentInamount = value;
		}
	}
}