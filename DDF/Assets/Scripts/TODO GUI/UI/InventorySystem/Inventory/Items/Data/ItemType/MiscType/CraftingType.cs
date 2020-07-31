using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/MiscCraftingType")]
    public class CraftingType : ItemType {
        public Crafting crafting = Crafting.CraftingMaterial;
    }

    public enum Crafting {
        CraftingMaterial,
        BlacksmithPlan,
        JewelerDesign,
        PageOfTraining,
        Dye,
        Gem,
    }
}