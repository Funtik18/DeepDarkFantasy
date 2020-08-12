using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character{
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Character/Quality")]
    public class CharacterQuality : ScriptableObject {
        public string qualityName;
    }
}
