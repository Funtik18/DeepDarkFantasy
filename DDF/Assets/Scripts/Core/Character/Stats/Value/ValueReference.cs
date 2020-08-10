using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    public abstract class ValueReference {
        public Value valueBase;

        /// <summary>
        /// Событие, произошло изменение значаения.
        /// </summary>
        public Action onChange;
        /// <summary>
        /// Событие, произошёл перерасчёт значения со всеми зависимастями.
        /// </summary>
        public Action<Value> onRecalculate;

        public List<Value> dependent;

        public void RecalculateDependencies() {
			if (dependent != null) {
				foreach (Value value in dependent) {
                    onRecalculate?.Invoke(value);
                }
			}
		}

        public virtual string Text { get; internal set; }
        public abstract void Null();
    }


    public class ValueIntReference : ValueReference {

        public int value;

        public ValueIntReference( Value valBase, int val = 0 ) {
            valueBase = valBase;
            value = val;
        }

        /// <summary>
        /// Получает текст имя + значение
        /// </summary>
		public override string Text {
			get {
                return valueBase.valueName + " " + value.ToString();
			}
        }

        /// <summary>
        /// Обнуление значаения.
        /// </summary>
		public override void Null() {
            value = 0;

            onChange?.Invoke();
            RecalculateDependencies();
        }

        /// <summary>
        /// Изменение значения у этого value.
        /// </summary>
        public void Sum( int sum ) {
            value += sum;

            onChange?.Invoke();
            RecalculateDependencies();
        }
    }
    public class ValueFloatReference : ValueReference {
        public float value;

        public ValueFloatReference( Value valBase, float val = 0 ) {
            valueBase = valBase;
            value = val;
        }

        public override string Text {
            get {
                return valueBase.valueName + " " + value.ToString();
            }
        }

        public override void Null() {
            value = 0f;

            onChange?.Invoke();
            base.RecalculateDependencies();
        }

        public void Sum( float sum ) {
            value += sum;

            onChange?.Invoke();
            base.RecalculateDependencies();
        }
    }
}