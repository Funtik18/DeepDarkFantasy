using System;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.UI.Inventory {
    
    [CreateAssetMenu(fileName ="Data",menuName = "DDF/Inventory/ItemTag/Tag")]
    [Serializable]
    public class ItemTag : ScriptableObject{
        public string tag;
    }
}