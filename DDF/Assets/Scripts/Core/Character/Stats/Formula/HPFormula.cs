﻿using DDF.Atributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {

	[CreateAssetMenu(fileName = "Data", menuName = "DDF/Character/Stats/Formula/HP")]
	public class HPFormula : FormulaInt {

		public Value valueStrength;
		private int strength;
		
		public override int Calculate( Stats stats ) {
			stats.Get(valueStrength, out strength);

			return 10 + strength * 2;
		}

		public override List<Value> GetRefernces() {
			List<Value> values = new List<Value>();
			values.Add(valueStrength);

			return values;
		}
	}
}