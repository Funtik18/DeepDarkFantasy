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
        Character_stats cS = other.gameObject.GetComponent<Character_stats>();
        string myname = gameObject.transform.root.name;
        string hisname = other.name;
        if(myname != hisname)
            if(active){
                if(moveSpeed>1){
                    if(cS != null){
                        cS.getHit((int)moveSpeed%dmg);
                    }    
                } 
            }
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
