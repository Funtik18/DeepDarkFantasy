using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bow : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator myanim;
    public GameObject arrow;
    public Transform bowstring;
    public bool startPrepare = false,shoot = false;

    [HideInInspector]
    public bool ready = false;
    private GameObject arW;
    void Start()
    {
        myanim = this.GetComponent<Animator>();
    }

    private void Update() {
        myanim.SetBool("ready",ready);
        myanim.SetBool("shoot",shoot);

        if(startPrepare){
            startPrepare = false;
            prepare();
            ready = true;
        }
        if(shoot && ready){
            Fire();
            ready = false;
        }else{
            shoot = false;
        }
    }
    // Update is called once per frame
    public void prepare(){
        arW = Instantiate(arrow, bowstring.position, bowstring.rotation);
        arW.transform.parent = bowstring;
    }

    public void Fire(){
        arW.transform.parent = null;
        arW.GetComponent<Rigidbody>().isKinematic = false;
        arW.GetComponent<Rigidbody>().AddForce(transform.right*1000);
        myanim.SetBool("shoot",shoot);
        shoot = false;
    }

}
