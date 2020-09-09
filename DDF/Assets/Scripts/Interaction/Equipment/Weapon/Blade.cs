using DDF.Atributes;
using DDF.Character;
using DDF.Environment;
using UnityEngine;

namespace DDF.Environment {
    [RequireComponent(typeof(BoxCollider))]
    /// <summary>
    /// Взаимодействие, само лезвие орудия, его нанесение урона.
    /// </summary>
    public class Blade : Interaction {
        Entity owner;
        [ReadOnly] public float currentMoveSpeed = 1;
        [HideInInspector] public bool bladeActive;

        public void Init(Entity owner) {
            this.owner = owner;
        }

        public override void OnTriggerEnter(Collider other) {
            if (!bladeActive) return;
            base.OnTriggerEnter(other);
            Entity entity = other.GetComponent<Entity>();
            if (entity == null) return;

            if (currentMoveSpeed > 1) {
                entity.TakeDamage(owner.GetMeleeDamage());
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
                currentMoveSpeed = Vector3.Distance(oldPos, newPos);
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