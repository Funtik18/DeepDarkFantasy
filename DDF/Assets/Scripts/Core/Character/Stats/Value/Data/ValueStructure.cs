using DDF.Atributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Character/Stats/Value/Struct")]
    public class ValueStructure : ScriptableObject {
        public List<ValueStat> stats;
    }
    [Serializable]
    public class ValueStat {
        public Value value;
        [Range(1, 10)]
		[InfoBox("Если есть формулы то это не важное поле")]
        public float count;
	}
}