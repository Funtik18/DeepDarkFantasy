using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_stats : MonoBehaviour
{
    public float HP;
    public bool dead;

    [HideInInspector]
    public GameObject Iam;
    // Start is called before the first frame update
    public void getHit(int dmg,GameObject place){
        HP -= dmg; 
        Iam = place;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
