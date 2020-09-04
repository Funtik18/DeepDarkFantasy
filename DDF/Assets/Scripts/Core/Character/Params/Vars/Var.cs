using DDF.Atributes;
using UnityEngine;

namespace DDF.Character.Variables {
	[System.Serializable]
	public class Var {
		[ReadOnly]
		public string varName;
		public Var(string varName) {
			this.varName = varName;
		}
	}
	public class VarInt : Var {
		public int amount;

		public VarInt(string varName, int amount) : base(varName) {
			this.amount = amount;
		}
	}
	public class VarFloat : Var {
		public float amount;

		public VarFloat(string varName, float amount) : base(varName) {
			this.amount = amount;
		}
	}
	public class VarMinMax<T> : Var where T : Var {
		public T min;
		public T max;

		public VarMinMax(string varName, T min, T max) : base(varName) {
			this.min = min;
			this.max = max;
		}
	}
}

