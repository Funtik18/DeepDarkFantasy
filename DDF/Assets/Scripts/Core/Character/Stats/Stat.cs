using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    [CreateAssetMenu(menuName = "DDF/Character/Stats/Stat", fileName ="Data")]
    public class Stat : ScriptableObject {
        public string name;
        [Min(1)]
        public float amount;
    }
}