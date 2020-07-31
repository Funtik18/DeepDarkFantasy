using UnityEngine;

namespace DDF.UI.Inventory.Items {
    [CreateAssetMenu(fileName = "Data", menuName = "DDF/Inventory/ItemType/ArmorHeadType")]
    public class HeadType : ArmorType {
        public Head head = Head.Helm;
    }
    public enum Head {
        Helm,
    }
}