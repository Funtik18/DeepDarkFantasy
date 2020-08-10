using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Character/Stats/Value/Value")]
    public class Value : ScriptableObject {
        public string valueName;
        public Formula formula;
    }
}
