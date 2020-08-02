using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_scripts : MonoBehaviour
{
    public int dmg = 10;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other) {
        Character_stats stat = other.GetComponent<Character_stats>();
        if(stat != null){
            stat.getHit(dmg,gameObject.transform.root.gameObject);
        }
        Debug.Log("Hit "+other.name);
    }
    
}
