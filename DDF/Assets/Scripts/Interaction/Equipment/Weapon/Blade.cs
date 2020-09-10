using DDF.Atributes;
using DDF.Character;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Environment {
    [RequireComponent(typeof(BoxCollider))]
    /// <summary>
    /// Взаимодействие, само лезвие орудия, его нанесение урона.
    /// </summary>
    public class Blade : Interaction {

        public UnityAction<Entity> onEnter;
        public UnityAction<Entity> onStay;
        public UnityAction<Entity> onExit;

        private Entity currentEntity;

        public override void OnTriggerEnter(Collider other) {
            base.OnTriggerEnter(other);
            currentEntity = other.transform.root.GetComponent<Entity>();
            onEnter?.Invoke(currentEntity);
        }
        public override void OnTriggerStay(Collider other) {
            base.OnTriggerStay(other);
            onStay?.Invoke(currentEntity);
        }
        public override void OnTriggerExit(Collider other) {
            base.OnTriggerExit(other);
            onExit?.Invoke(currentEntity);
            currentEntity = null;
        }

        public void EnableBlade(bool enable) {
            GetComponent<BoxCollider>().enabled = enable;
        }
    }
}