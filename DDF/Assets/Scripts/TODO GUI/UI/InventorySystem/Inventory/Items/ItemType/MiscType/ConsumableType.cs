using UnityEngine;

namespace DDF.Inventory.Items {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/MiscConsumableType")]
    public class ConsumableType : ItemType {
        public Consumable conumable = Consumable.Potion;
    }

    public enum Consumable {
        Potion,
        Food,
    }
}