using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Inventory.Stats {
	#region Stats
	

	#endregion
	public class Character : MonoBehaviour {
        public ValueStructure statsStructure;

        public Stats stats;
        //public 

        private void Awake() {
			InitValues();
            InitFormulas();
		}

        public Value test;
        public Value test2;
        private void Update() {
            if (Input.GetKeyDown(KeyCode.X)) {
                stats.Sum(test, 1);
                stats.Sum(test2, 2);
            }
        }


        #region Setup

        private void InitValues() {
			stats = new Stats();

			for (int i = 0; i < statsStructure.values.Count; i++) {

				Value value = statsStructure.values[i];

				if (value is ValueFloat) {
					stats.valueList.Add(new ValueFloatReference(value, 0f));

				}
				if (value is ValueInt) {
					stats.valueList.Add(new ValueIntReference(value, 0));
				}
			}
		}
        private void InitFormulas() {
            foreach (ValueReference valueReference in stats.valueList) {

                Formula formula = valueReference.valueBase.formula;
                Value value = valueReference.valueBase;
                
                
                if (formula) {
                    valueReference.Null();
                    if (formula is FormulaInt) {
                        FormulaInt formulaInt = (FormulaInt)formula;
                        stats.Sum(value, formulaInt.Calculate(stats));

                    }
                    if (formula is FormulaFloat) {
                        FormulaFloat formulaInt = (FormulaFloat)formula;
                        stats.Sum(value, formulaInt.Calculate(stats));
                    }
                    List<Value> references = formula.GetRefernces();

					for (int i = 0; i < references.Count; i++) {
                        stats.Subscribe(ValueRecalculate, value, references[i]);

                    }
                }
            }
        }
        private void ValueRecalculate(Value v) {
            ValueReference valueNull = stats.GetValueReference(v);
            valueNull.Null();
            
            foreach (ValueReference valueReference in stats.valueList) {
                Formula formula = valueReference.valueBase.formula;

                if (formula) {
                    if (formula is FormulaInt) {
                        FormulaInt formulaInt = (FormulaInt)formula;

                        stats.Sum(valueReference.valueBase, formulaInt.Calculate(stats));

                    }
                    if (formula is FormulaFloat) {
                        FormulaFloat formulaInt = (FormulaFloat)formula;

                        stats.Sum(valueReference.valueBase, formulaInt.Calculate(stats));
                    }
                }
            }
        }

		#endregion
	}
}
