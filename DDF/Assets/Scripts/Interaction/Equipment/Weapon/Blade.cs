using DDF.Atributes;
using DDF.Character;
using DDF.Environment;
using DDF.UI.Inventory.Items;
using UnityEngine;

namespace DDF.Environment {
    [RequireComponent(typeof(BoxCollider))]
    /// <summary>
    /// Взаимодействие, само лезвие орудия, его нанесение урона.
    /// </summary>
    public class Blade : Interaction {
        public VarMinMax<float> itemDamage;
        [ReadOnly] public float currentMoveSpeed = 1;
        [HideInInspector] public bool bladeActive;

        public void Init(VarMinMax<float> itemDamage) {
            this.itemDamage = itemDamage;
            oldPos = transform.position;
        }

        public override void OnTriggerEnter(Collider other) {
            if (!bladeActive) return;
            base.OnTriggerEnter(other);
            Entity entity = other.transform.root.GetComponent<Entity>();
            if (entity == null) return;
            if (currentMoveSpeed > 1) {
                entity.TakeDamage(currentMoveSpeed % Random.Range(itemDamage.min, itemDamage.max));
            }
        }
        public override void OnTriggerStay(Collider other) {
            if (!bladeActive) return;
            base.OnTriggerStay(other);
        }
        public override void OnTriggerExit(Collider other) {
            if (!bladeActive) return;
            base.OnTriggerExit(other);
        }

        private Vector3 oldPos;
        private Vector3 newPos;
        private bool speedOrder;
        void FixedUpdate() {
            if (!bladeActive) return;
            if (speedOrder) {
                newPos = transform.position;
                currentMoveSpeed = Vector3.Distance(oldPos, newPos)*10;
            } else
                oldPos = transform.position;
            speedOrder = !speedOrder;
        }

        public void EnableBlade(bool enable) {
            bladeActive = enable;
            GetComponent<BoxCollider>().enabled = bladeActive;
        }
    }
}
/*NPSEntity ncS = other.gameObject.GetComponent<NPSEntity>();
               CharacterEntity cS = other.gameObject.GetComponent<CharacterEntity>();
               string myname = gameObject.transform.root.name;
               string hisname = other.name;
               if(myname != hisname)
                   if(active){
                       if(moveSpeed>1){
                           if(ncS != null){
                               ncS.TakeDamage(moveSpeed%dmg);
                               //cS.TakeDamage((int)moveSpeed%dmg,gameObject.transform.root.gameObject);
                           } 
                           if(cS != null){
                               cS.TakeDamage(moveSpeed%dmg);
                               //cS.TakeDamage((int)moveSpeed%dmg,gameObject.transform.root.gameObject);
                           }     
                       } 
                   }*/