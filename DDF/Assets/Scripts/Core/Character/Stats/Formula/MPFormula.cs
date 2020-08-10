using DDF.Atributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {

	[CreateAssetMenu(fileName = "Data", menuName = "DDF/Character/Stats/Formula/MP")]
	public class MPFormula : FormulaInt {

		public Value valueIntillegence;
		private int intelligence;
		
		public override int Calculate( Stats stats ) {
			stats.Get(valueIntillegence, out intelligence);

			return 5 + intelligence * 2;
		}

		public override List<Value> GetRefernces() {
			List<Value> values = new List<Value>();
			values.Add(valueIntillegence);

			return values;
		}
	}
}