using UnityEngine;

namespace DDF.Inventory.Items {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/ArmorTorsoType")]
    public class TorsoType : ArmorType {
        public Torso torso = Torso.ChestArmor;
    }
    public enum Torso {
        ChestArmor,
    }
}