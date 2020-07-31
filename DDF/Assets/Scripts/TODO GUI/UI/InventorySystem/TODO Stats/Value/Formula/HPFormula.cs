﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Stats {

	[CreateAssetMenu(fileName = "HP", menuName = "DDF/Inventory/Stats/Formula/HP", order = 1)]
	public class HPFormula : FormulaInt {

		public Value valueVitality;
		private int vitality;
		public Value valueStrength;
		private int strength;
		
		public override int Calculate( Stats stats ) {

			stats.Get(valueVitality,out vitality);

			stats.Get(valueStrength, out strength);

			return vitality*6 + strength*2+30;
		}

		public override List<Value> GetRefernces() {
			List<Value> values = new List<Value>();
			values.Add(valueVitality);
			values.Add(valueStrength);

			return values;
		}
	}
}