﻿using DDF.Character;
using DDF.Character.Stats;
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
        //if(_timer>20){
        //    Destroy(plumage);
       // }
    }

    private void OnTriggerEnter(Collider other) {
        if(fly){
            Debug.Log("Arrow hit" +other.name);
            Entity entity = other.transform.root.GetComponent<Entity>();
            string myname = gameObject.transform.root.name;
            string hisname = other.name;
            if(myname != hisname)
                entity?.TakeDamage(dmg);
                    //cS.TakeDamage(dmg,gameObject.transform.root.gameObject);
            plumage.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.transform.root.parent = other.transform;
            fly = false;
        }
    }
}
