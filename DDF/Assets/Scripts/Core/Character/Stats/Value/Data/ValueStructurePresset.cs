using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Character/Stats/Value/StructPresset")]
    public class ValueStructurePresset : ValueStructure {
        public List<int> integers;
    }
}