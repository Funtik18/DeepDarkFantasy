using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    public class Stats {

        public List<ValueReference> valueList;

        public Stats() {
            valueList = new List<ValueReference>();
        }


        /// <summary>
        /// Подписка на onChange
        /// </summary>
        /// <param name="action"></param>
        /// <param name="value"></param>
        public void SubscribeOnChange( Action action, Value value ) {
            ValueReference valueReference = GetValueReference(value);

            if(valueReference == null) {
                Debug.LogError(value.valueName + " does not assign in stats");
                return;
			}

            valueReference.onChange += action;
        }
        /// <summary>
        /// Подписка на OnRecalculate
        /// </summary>
        /// <param name="action"></param>
        /// <param name="dependency"></param>
        /// <param name="value"></param>
        public void SubscribeOnRecalculate( Action<Value> action, Value dependency, Value value ) {
            ValueReference valueReference = GetValueReference(value);

            if (valueReference.onRecalculate == null) {
                valueReference.onRecalculate = action;
            }
            if (valueReference.dependent == null) {
                valueReference.dependent = new List<Value>();
            }

            valueReference.dependent.Add(dependency);
        }


        /// <summary>
        /// Получает текст имя + значение по value.
        /// </summary>
        public string GetText( Value value ) {
            ValueReference valueReference = GetValueReference(value);

            return valueReference.Text;
        }


        /// <summary>
        /// Получение значения определённого стата.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="state"></param>
        public void Get( Value value, out int state ) {
            ValueReference valueReference = GetValueReference(value);

            ValueIntReference valueIntReference = (ValueIntReference)valueReference;

            if (valueIntReference == null) {
                state = 0;
            } else {
                state = valueIntReference.value;
            }
        }
        /// <summary>
        /// Получение значения определённого стата.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="state"></param>
        public void Get( string value, out int state ) {
            ValueReference valueReference = GetValueReference(value);

            ValueIntReference valueIntReference = (ValueIntReference)valueReference;

            if (valueIntReference == null) {
                state = 0;
            } else {
                state = valueIntReference.value;
            }
        }
        /// <summary>
        /// Получение значения определённого стата.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="state"></param>
        public void Get( Value value, out float state ) {
            ValueReference valueReference = GetValueReference(value);

            ValueFloatReference valueFloatReference = (ValueFloatReference)valueReference;

            if (valueFloatReference == null) {
                state = 0;
            } else {
                state = valueFloatReference.value;
            }
        }
        /// <summary>
        /// Получение значения определённого стата.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="state"></param>
        public void Get( string value, out float state ) {
            ValueReference valueReference = GetValueReference(value);

            ValueFloatReference valueFloatReference = (ValueFloatReference)valueReference;

            if (valueFloatReference == null) {
                state = 0;
            } else {
                state = valueFloatReference.value;
            }
        }


        /// <summary>
        /// Находит Value по ValueReference из списка valueList.
        /// </summary>
        public ValueReference GetValueReference( Value value ) {
            return valueList.Find(x => x.valueBase == value);
        }
        public ValueReference GetValueReference( string value ) {
            return valueList.Find(x => x.valueBase.valueName.ToLower() == value.ToLower());
        }


        /// <summary>
        /// Изменение значения у этого value.
        /// </summary>
        public void Sum( Value value, int sum ) {
            ValueReference valueReference = GetValueReference(value);

            if (valueReference != null) {
                ValueIntReference reference = (ValueIntReference)valueReference;
                reference.Sum(sum);
            } else {
                Add(value, sum);
            }
        }
        /// <summary>
        /// Изменение значения у этого value.
        /// </summary>
        public void Sum( Value value, float sum ) {
            ValueReference valueReference = GetValueReference(value);

            if (valueReference != null) {
                ValueFloatReference reference = (ValueFloatReference)valueReference;
                reference.Sum(sum);
            } else {
                Add(value, sum);
            }
        }


        private void Add( Value value, int sum ) {
            valueList.Add(new ValueIntReference(value, sum));
        }
        private void Add( Value value, float sum ) {
            valueList.Add(new ValueFloatReference(value, sum));
        }
    }
}