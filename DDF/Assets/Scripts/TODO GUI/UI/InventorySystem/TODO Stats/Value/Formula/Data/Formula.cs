using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Stats {
    public abstract class Formula : ScriptableObject {

        public abstract List<Value> GetRefernces();
    }
    public abstract class FormulaInt : Formula {
        public abstract int Calculate( Stats stats );
    }
    public abstract class FormulaFloat : Formula {
        public abstract float Calculate( Stats stats );

    }
}