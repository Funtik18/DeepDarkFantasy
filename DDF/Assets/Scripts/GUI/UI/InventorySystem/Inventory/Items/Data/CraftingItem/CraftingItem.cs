using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "CraftingItem", menuName = "DDF/Inventory/Items/CraftingItem", order = 1)]
    public class CraftingItem : MonoBehaviour {
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