using System.Collections;
using System.Collections.Generic;
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
        CharacterStats cS = other.gameObject.GetComponent<CharacterStats>();
        string myname = gameObject.transform.root.name;
        string hisname = other.name;
        if(myname != hisname)
            if(active){
                if(moveSpeed>1){
                    if(cS != null){
                        cS.TakeDamage((int)moveSpeed%dmg,gameObject.transform.root.gameObject);
                    }    
                } 
            }
    }

    private void Start(){
        if(gameObject.transform.root.GetComponent<CharacterStats>() != null)
        dmg = (int)gameObject.transform.root.GetComponent<CharacterStats>().meleeDamage;
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
