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

        internal void RecalculateDependencies() {
			if (dependent != null) {
				foreach (Value v in dependent) {
                    onRecalculate?.Invoke(v);
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
            base.RecalculateDependencies();
        }

        /// <summary>
        /// Изменение значения у этого value.
        /// </summary>
        public void Sum( int sum ) {
            value += sum;

            onChange?.Invoke();
            base.RecalculateDependencies();
        }
    }
    public class ValueFloatReference : ValueReference {
        public float value;

        public ValueFloatReference( Value _valueBase, float _value = 0 ) {
            valueBase = _valueBase;
            value = _value;
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


        internal void Sum( float sum ) {
            value += sum;

            onChange?.Invoke();
            base.RecalculateDependencies();

        }
    }
}