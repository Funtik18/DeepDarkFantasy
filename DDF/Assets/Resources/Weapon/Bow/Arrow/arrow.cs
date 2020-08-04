using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    // Start is called before the first frame update
    public int dmg = 10;
    public int countInScene = 2;
    public GameObject plumage;
    private bool fly = true;
    private float _timer = 0;
    void Start()
    {
    }
    
    private void Update() {
        _timer+=Time.deltaTime;
        if(_timer>20){

        }
    }

    private void OnTriggerEnter(Collider other) {
        if(fly){
            Character_stats cS = other.gameObject.GetComponent<Character_stats>();
            if(cS != null)
                cS.getHit(dmg,gameObject.transform.root.gameObject);
            plumage.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.transform.root.parent = other.transform;
            fly = false;
        }
    }
}
