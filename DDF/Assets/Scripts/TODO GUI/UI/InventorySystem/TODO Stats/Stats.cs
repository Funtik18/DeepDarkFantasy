using System;
using System.Collections.Generic;

namespace DDF.UI.Inventory.Stats {
    public class Stats {
        public List<ValueReference> valueList;

        public Stats() {
            valueList = new List<ValueReference>();
        }

        internal void Subscribe( Action action, Value v ) {
            ValueReference valueReference = GetValueReference(v);

            valueReference.onChange += action;
        }
        internal void Subscribe( Action<Value> action, Value dependency, Value v ) {
            ValueReference valueReference = GetValueReference(v);

            if (valueReference.onRecalculate == null) {
                valueReference.onRecalculate = action;
            }
            if (valueReference.dependent == null) {
                valueReference.dependent = new List<Value>();
            }

            valueReference.dependent.Add(dependency);
        }

        public string GetText( Value v ) {
            ValueReference valueReference = GetValueReference(v);

            return valueReference.Text;
        }

        public void Get( Value v, out int state ) {
            ValueReference valueReference = GetValueReference(v);

            ValueIntReference valueIntReference = (ValueIntReference)valueReference;

            if (valueIntReference == null) {
                state = 0;
            } else {
                state = valueIntReference.value;
            }
        }
        public void Get( Value v, out float state ) {
            ValueReference valueReference = GetValueReference(v);

            ValueFloatReference valueFloatReference = (ValueFloatReference)valueReference;

            if (valueFloatReference == null) {
                state = 0;
            } else {
                state = valueFloatReference.value;
            }
        }

        public ValueReference GetValueReference( Value v ) {
            return valueList.Find(x => x.valueBase == v);
        }


        public void Sum( Value v, int sum ) {
            ValueReference valueReference = GetValueReference(v);

            if (valueReference != null) {
                ValueIntReference reference = (ValueIntReference)valueReference;
                reference.Sum(sum);
            } else {
                Add(v, sum);
            }
        }
        public void Sum( Value v, float sum ) {
            ValueReference valueReference = GetValueReference(v);

            if (valueReference != null) {
                ValueFloatReference reference = (ValueFloatReference)valueReference;
                reference.Sum(sum);
            } else {
                Add(v, sum);
            }
        }

        private void Add( Value v, int sum ) {
            valueList.Add(new ValueIntReference(v, sum));
        }
        private void Add( Value v, float sum ) {
            valueList.Add(new ValueFloatReference(v, sum));
        }

    }
}