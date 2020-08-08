using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDF.Atributes;

public class knight_AI : MonoBehaviour
{
    public float spare_dis = 50;
    public float speed = 20, speedRotation = 20, gravity = 3, sparing_distance = 15;
    
    [InfoBox("maxPain - Болевой порог, до которого я игнорирую попадание", InfoBoxType.Normal)]
    public float maxPain = 2;
    private CharacterController characterController;
    private Animator myanim;
    public bool walk,endbattle;
    public GameObject axeReady, axenotready;

    [InfoBox("LHandPoint - Место для крепления левой руки на пушке", InfoBoxType.Normal)]
    public Transform LHandPoint;
    private List<GameObject> enemys = new List<GameObject>();

    [InfoBox("Targets_Tag - Теги тех кого я не люблю", InfoBoxType.Normal)]
    public List<string> Targets_Tag = new List<string>();
    private bool heviatack,isee,attacking,hiting,weapon,Agressive;
    private float _timer = 0,_timere = 0,mYHp;
    private Vector3 moveDirection = Vector3.zero;
    private RayScan Myeyes; 
    private CharacterStats stats; 
    private int nap = 0;
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
            mYHp = stats.currentHP;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lookMySost();
        if(Agressive){
            _timer+=Time.deltaTime;
            if(_timer>=2.5f)
                Ballte_mode();
        }else{
            if(walk && !stats.dead && !endbattle)
                move_to_point();
        }
    }

    private void lookMySost(){
        if(stats.currentHP<=0){
            enemys.Clear();
            GetComponent<Animator>().applyRootMotion = false;
            myanim.SetBool("Dead",true);
            stats.dead = true;
            _timere+=Time.deltaTime;

            GetComponentInChildren<RayScan>().enabled = false;
            GetComponent<CharacterController>().enabled = false;

            if(_timere >= 3.25f){
                GetComponent<Animator>().enabled = false;
                this.enabled = false;
            }
        }

        if(mYHp>stats.currentHP && (mYHp-stats.currentHP)>maxPain){
            if(!hiting){
                myanim.SetBool("Hit",true);
                }
            mYHp = stats.currentHP;
            if(!Agressive)
                IseeSomething(stats.Iam);
        }
        
        if(enemys.Count != 0){
            Agressive = true;
        }
        else{
            Agressive = false;
            GetComponent<IK_Controls>().lookObj = null;
            _timer = 0;
            myanim.SetFloat("X",0);
        }
        myanim.SetBool("Battle",Agressive);

        if(weapon){
            axeReady.SetActive(true);
            axenotready.SetActive(false);
        }
        else{
            axenotready.SetActive(true);
            axeReady.SetActive(false);
        }

        foreach(GameObject g in Myeyes.inview){
            IseeSomething(g);
        }

         if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        if(!stats.dead)
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
        Quaternion Qvc = enemy.transform.rotation;

        Myeyes.targetTag = enemy.tag;
        isee = Myeyes.isee;

        if(!isee && !heviatack){
            Quaternion targetRotation = Quaternion.LookRotation(Wvc - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
        }

        //int hevi_dist = 40;

        if((min>spare_dis && !heviatack))
        {
            GetComponent<Animator>().applyRootMotion = true;
            //transform.position = Vector3.MoveTowards(transform.position,Wvc,speed*Time.deltaTime);
            myanim.SetFloat("X",0.6f);
            //myanim.SetFloat("Y",Mathf.Abs(startPoint.z-nowPoint.z));
        }
        else
            if(min<=spare_dis && min>sparing_distance)
            {
                GetComponent<Animator>().applyRootMotion = true;
                myanim.SetBool("Attak", true);
                GetComponent<IK_Controls>().leftHandObj = LHandPoint;
                heviatack = true;
            }
            else
                if(min>=sparing_distance && !attacking)
                {
                    myanim.SetFloat("X",stats.speed);
                    //transform.position = Vector3.MoveTowards(transform.position,Wvc,speed*Time.deltaTime);
                }
                else
                {
                if(!heviatack)
                    {
                    if(!attacking)
                    {
                        attacking = true;
                        bool rootM = false;
                        int attack = Random.Range(1,4);
                        //Debug.Log("Choose Attack "+ attack);
                    if(attack == 1){
                            rootM = true;
                            myanim.SetBool("Low_Attak", true);
                        }
                    if(attack == 2){
                            rootM = true;
                            myanim.SetBool("Low_Attak", true);
                        }
                    if(attack == 3){
                            rootM = true;
                            heviatack = true;
                            myanim.SetBool("around_attack", true);
                        }
                    GetComponent<Animator>().applyRootMotion = rootM;
                    }
                } 
            } 

        if(enemy.GetComponent<CharacterStats>()!=null)
        if(enemy.GetComponent<CharacterStats>().dead){
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
        heviatack = false;
        attacking = false;
        hiting = false;
        endbattle = false;
        GetComponent<IK_Controls>().leftHandObj = null;
    }

    public void hide_weapon(){
        weapon = false;
        endbattle = true;
    }

    public void picup_weapon(){
        weapon = true;
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
                    if(!other.GetComponent<CharacterStats>().dead)
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
		if (Physics.Raycast (pos, Wvc, out hit, 10))
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
