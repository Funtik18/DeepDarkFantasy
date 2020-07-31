using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Stats {
    [CreateAssetMenu(fileName = "Value", menuName = "DDF/Inventory/Stats/Value/Value")]
    public class Value : ScriptableObject {
        public string name;
        public Formula formula;
    }
}
