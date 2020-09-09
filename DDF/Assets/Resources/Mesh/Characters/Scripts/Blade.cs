using DDF.Atributes;
using DDF.Character;
using UnityEngine;

public class Blade : MonoBehaviour {
    Entity owner;
    [ReadOnly]
    public float currentMoveSpeed = 1;
    [HideInInspector] public bool bladeActive;

    public void Init(Entity owner) {
        this.owner = owner;    
    }


    private void OnTriggerEnter(Collider other) {
        Entity entity = other.GetComponent<Entity>();
        if (entity == null) return;
        if (bladeActive) {
            if (currentMoveSpeed > 1) {
                entity.TakeDamage(owner.GetMeleeDamage());
            }
        }
    }
    private Vector3 oldPos;
    private Vector3 newPos;
    private bool speedOrder;
    void FixedUpdate() {
        if (speedOrder) {
            newPos = transform.position;
            currentMoveSpeed = Vector3.Distance(oldPos, newPos);
        } else
            oldPos = transform.position;
        speedOrder = !speedOrder;
    }

    public void EnableBlade(bool enable) {
        bladeActive = enable;
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