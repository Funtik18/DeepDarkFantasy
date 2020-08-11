using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
	public class Stat {
		public string name;
	}
	public class StatInt : Stat {
		public int amount;

		public StatInt(string newName, int value) {
			name = newName;
			amount = value;
		}
	}
	public class StatFloat : Stat {
		public float amount;

		public StatFloat( string newName, float value ) {
			name = newName;
			amount = value;
		}
	}
	public class StatRegularInt : StatInt {
		public int max;
		public StatRegularInt( string newName, int value, int maximum ) : base(newName, value) {
			max = maximum;
		}
	}
	public class StatRegularFloat : StatFloat {
		public float max;
		public StatRegularFloat( string newName, float value, float maximum ) : base(newName, value) {
			max = maximum;
		}
	}
}