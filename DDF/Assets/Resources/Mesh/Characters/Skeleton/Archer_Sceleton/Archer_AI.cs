using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDF.Atributes;
using DDF.Character.Stats;

public class Archer_AI : MonoBehaviour
{
    public float speed = 20, speedRotation = 20, gravity = 3, sparing_distance = 15;
    
    [InfoBox("maxPain - Болевой порог, до которого я игнорирую попадание", InfoBoxType.Normal)]
    public float maxPain = 2;
    private CharacterController characterController;
    private Animator myanim;
    public GameObject bowReady, bowNotReady;
    public GameObject arrowInKolchan;
    public Transform bowString;
    public bool walk;
    private List<GameObject> enemys = new List<GameObject>();

    [InfoBox("Targets_Tag - Теги тех кого я не люблю", InfoBoxType.Normal)]
    public List<string> Targets_Tag = new List<string>();
    //public List<Transform> Waypoints = new List<Transform>();
    private Vector3 startPoint, nowPoint;
    private bool heviatack,isee,attacking,hiting,weapon,Agressive,irotate,endbattle,arrow;
    private float _timer = 0,_timere = 0,mYHp;
    private Vector3 moveDirection = Vector3.zero, moveforward = Vector3.zero;
    private RayScan Myeyes; 
    private CharacterStats stats; 
    private int way_number = 0,nap = 0;
    // Start is called before the first frame update
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        myanim = this.GetComponent<Animator>();
        if(GetComponentInChildren<RayScan>()!=null){
            Myeyes = GetComponentInChildren<RayScan>();
        }
        if(GetComponentInChildren<CharacterStats>()!=null){
            stats = GetComponentInChildren<CharacterStats>();
            mYHp = stats.CurrentHealthPoints;
        }
    }

    // Update is called once per frame
    void Update()
    {
            lookMySost();
        if(Agressive && !stats.IsDead){
            //walk = false;
            _timer+=Time.deltaTime;
            if(_timer>=1.5f)
                Ballte_mode();
        }else{
            if(walk && !endbattle && !stats.IsDead)
                move_to_point();
        }
    }

    private void lookMySost(){
        if(stats.CurrentHealthPoints <= 0){

            GetComponent<IK_Controls>().rightHandObj = null;
            bowReady.GetComponent<bow>().shoot = true;

            enemys.Clear();
            GetComponent<Animator>().applyRootMotion = false;
            myanim.SetBool("Dead",true);
            stats.IsDead = true;
            _timere+=Time.deltaTime;

            GetComponentInChildren<RayScan>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            if(_timere >= 3.25f){
                GetComponent<Animator>().enabled = false;
                this.enabled = false;
            }
        }
        
        if(mYHp>stats.CurrentHealthPoints) {
            if(!hiting && (mYHp-stats.CurrentHealthPoints ) >maxPain)
                myanim.SetBool("Hit",true);
            mYHp = stats.CurrentHealthPoints;
            if(!Agressive)
                IseeSomething(stats.Iam);
        }

        if(enemys.Count != 0 || hiting){
            Agressive = true;
        }
        else{
            Agressive = false;
            GetComponent<IK_Controls>().lookObj = null;
            _timer = 0;
            myanim.SetFloat("X",0);
            startPoint = transform.position;
        }
        myanim.SetBool("Battle",Agressive);

        if(bowReady != null && bowNotReady != null)
            if(weapon){
                bowReady.SetActive(true);
                bowNotReady.SetActive(false);
            }
            else{
                bowNotReady.SetActive(true);
                bowReady.SetActive(false);
            }
        
         if(arrowInKolchan != null)
            if(arrow){
                arrowInKolchan.SetActive(true);
            }
            else{
                arrowInKolchan.SetActive(false);
            }

        foreach(GameObject g in Myeyes.inview){
            IseeSomething(g);
        }

         if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        if(!stats.IsDead)
            characterController.Move(moveDirection * Time.deltaTime);
    } 


    private void Ballte_mode(){
        float min = 100;
        int id = 0;
        for(int i = 0;i<enemys.Count;i++){
            float dist = Vector3.Distance(enemys[i].transform.position,transform.position);
            if(dist<min){
                min = dist;
                id = i;
            }
        }
        GameObject enemy = enemys[id];
        GetComponent<IK_Controls>().lookObj = enemy.transform;
        Vector3 Wvc = new Vector3 (enemy.transform.position.x,transform.position.y,enemy.transform.position.z);
        //Quaternion Qvc = enemy.transform.rotation;

        Myeyes.targetTag = enemy.tag;
        isee = Myeyes.isee;

        if(!isee && !heviatack){
            Quaternion targetRotation = Quaternion.LookRotation(Wvc - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
        }


        if((min>sparing_distance))
        {
            GetComponent<Animator>().applyRootMotion = true;
            myanim.SetFloat("X",stats.speed);
        }
         else
            if((min<sparing_distance-20))
            {
                myanim.SetFloat("X",stats.speed*-1);
            }
            else
            {
            if(bowReady.GetComponent<bow>().ready && !attacking)
                {
                GetComponent<IK_Controls>().rightHandObj = null;
                bowReady.GetComponent<bow>().shoot = true;
                myanim.SetBool("Attak",false);
                GetComponent<Animator>().applyRootMotion = true;
                }else{
                    attacking = true;
                    myanim.SetBool("Attak",true);
                }

            } 

        if(enemy.GetComponent<CharacterStats>()!=null)
        if(enemy.GetComponent<CharacterStats>().IsDead){
            GetComponent<IK_Controls>().rightHandObj = null;
            enemys.Remove(enemy);
            endbattle = true;
        }    
    }

    public void endAnim()
    {
        myanim.SetBool("Hit",false);
        myanim.SetBool("Attak",false);
        myanim.SetBool("Hevi_Attak",false);
        myanim.SetBool("Low_Attak", false);
        myanim.SetBool("around_attack", false);
        myanim.SetBool("jump_strafe", false);
        myanim.SetBool("right", false);
        myanim.SetBool("left", false);
        heviatack = false;
        attacking = false;
        hiting = false;
        irotate = false;
        endbattle = false;
    }

    public void readyToShoot(){
        bowReady.GetComponent<bow>().startPrepare = true;
        GetComponent<IK_Controls>().rightHandObj = bowString;
    }

    public void hide_weapon(){
        weapon = false;
    }

    public void picup_weapon(){
        weapon = true;
    }

    public void take_arrow(){
        arrow = !arrow;
    }

    public void IseeSomething(GameObject other){
        bool frendly = true;
        foreach(string s in Targets_Tag){
            //Debug.Log(other.name+" "+other.tag+" "+ s);
            if(other != null)
            if(other.tag.Equals(s)){
                frendly = false;
            }
        }
        bool have = false;
        if(!frendly){
            foreach(GameObject g in enemys){
                if(other.Equals(g)){
                    have = true;
                }
            }
            if(!have){
                if(other.GetComponent<CharacterStats>()!=null)
                    if(!other.GetComponent<CharacterStats>().IsDead)
                        enemys.Add(other);
            }
        }
    // Debug.Log("See Enemy, His friend? "+frendly+" i see him agane? "+have);
    }

    private void move_to_point(){

		Vector3 Wvc = transform.position;
        Vector3 offset = new Vector3(0,3,5);
        //if(!irotate)
        if(nap == 0){
            // Forward
            Wvc = new Vector3 (transform.position.x,transform.position.y,transform.position.z+5);
            offset = new Vector3(0,3,5);
        }else
        if(nap == 1){
           // Left
            Wvc = new Vector3 (transform.position.x-5,transform.position.y,transform.position.z);
            offset = new Vector3(-5,3,0);
        }else
        if(nap == 2){
             // right
            Wvc = new Vector3 (transform.position.x+5,transform.position.y,transform.position.z);
            offset = new Vector3(5,3,0);
        }else
        if(nap == 3){
            // Back
            Wvc = new Vector3 (transform.position.x,transform.position.y,transform.position.z-5);
            offset = new Vector3(0,3,-5);
        }

        RaycastHit hit = new RaycastHit();
		Vector3 pos = transform.position + offset;
        Debug.DrawRay(pos, Wvc, Color.green);
		if (Physics.Raycast (pos, Wvc, out hit, 10) && !irotate)
		{
            if(hit.transform.root.tag!=transform.tag)
                nap = Random.Range(0,4);
                //nap = (nap+1)%4;
            //Debug.Log(hit.transform.name+" "+nap);
        }
            //float dist = Vector3.Distance(transform.position,Wvc);
            myanim.SetFloat("X",stats.speed);
            Quaternion targetRotation = Quaternion.LookRotation(Wvc - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);

            GetComponent<Animator>().applyRootMotion = false;
            transform.position = Vector3.MoveTowards(transform.position,Wvc,speed*Time.deltaTime);
        
    }
}
