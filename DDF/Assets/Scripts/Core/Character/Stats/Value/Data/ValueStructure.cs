using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Character/Stats/Value/Struct")]
    public class ValueStructure : ScriptableObject {
        public List<Value> values;
    }
}