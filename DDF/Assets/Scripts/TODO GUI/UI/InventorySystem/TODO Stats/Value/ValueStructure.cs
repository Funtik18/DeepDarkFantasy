using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Stats {
    [CreateAssetMenu(fileName = "ValueStructure", menuName = "DDF/Inventory/Stats/Value/Struct")]
    public class ValueStructure : ScriptableObject {
        public List<Value> values;
    }

    [CreateAssetMenu(fileName = "ValueStructurePresset", menuName = "DDF/Inventory/Stats/Value/StructPresset")]
    public class ValueStructurePresset : ValueStructure {
        public List<int> integers;
    }
}