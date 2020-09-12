using UnityEngine;
using DDF.UI.Inventory;
using DDF.UI.Inventory.Items;

namespace DDF.Character {
    public class HumanoidEntity : Entity {
        public Inventory inventory;
        public Equipment equipment;

        #region Actions
        public virtual bool Take(Item item, Inventory inventory) {
            if (this.inventory.AddItem(item, false) == false) {
                return false;
            }
            inventory?.DeleteItem(item);
            return true;
        }
        public virtual bool Equip(Item item, Inventory from) {
            Item equipedItem = equipment.Equip(item, from);
            if (equipedItem == null) return false;
            from?.DeleteItem(item);
            return true;
        }
        public virtual void TakeOff(Item item, Inventory inventory) {
            Item dropedItem = equipment.TakeOff(item, inventory);
            if (dropedItem == null) return;
            this.inventory.AddItem(dropedItem);
        }

        public virtual void Drink(ConsumableItem item, Inventory inventory) {
            for (int i = 0; i < item.effects.Count; i++) {
                effects.AddEffect(Instantiate(item.effects[i]));
            }
            inventory.DeleteItem(item);
        }
        #endregion
    }
}
