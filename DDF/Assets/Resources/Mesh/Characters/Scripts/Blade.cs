using DDF.Character;
using DDF.Character.Stats;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public int dmg = 1;
    public bool active;
    public float koef = 10f;
    private Vector3 oldPos;
    private Vector3 newPos;
    private bool speedOrder;
    public float moveSpeed;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        NPSEntity ncS = other.gameObject.GetComponent<NPSEntity>();
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
            }
    }

    private void Start(){
        if(gameObject.transform.root.GetComponent<NPSEntity>() != null)
        dmg = (int)gameObject.transform.root.GetComponent<NPSEntity>().GetMeleeDamage();
        if(dmg<=0)
            dmg = 1;
    }
       
void FixedUpdate () {
     if (speedOrder) {
            newPos = transform.position;
            moveSpeed = Vector3.Distance(oldPos, newPos) * koef;
     } else
            oldPos = transform.position;
            speedOrder = !speedOrder;          
}
}
