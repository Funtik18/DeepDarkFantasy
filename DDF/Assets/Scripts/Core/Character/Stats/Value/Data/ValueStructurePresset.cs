using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Character/Stats/Value/StructPresset")]
    public class ValueStructurePresset : ValueStructure {
        [Range(1, 10)]
        public List<int> integers;
    }
}