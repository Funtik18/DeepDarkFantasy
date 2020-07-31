﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_AI : MonoBehaviour
{
    public float speed = 20, speedRotation = 20, gravity = 3, sparing_distance = 15;
    private CharacterController characterController;
    private Animator myanim;
    public GameObject axeReady, axenotready;
    public bool walk;
    private List<GameObject> enemys = new List<GameObject>();
    public List<string> Targets_Tag = new List<string>();
    //public List<Transform> Waypoints = new List<Transform>();
    private Vector3 startPoint, nowPoint;
    private bool heviatack,isee,attacking,hiting,weapon,Agressive,irotate,endbattle;
    private float _timer = 0,_timere = 0,mYHp;
    private Vector3 moveDirection = Vector3.zero, moveforward = Vector3.zero;
    private RayScan Myeyes; 
    private Character_stats stats; 
    private int way_number = 0,nap = 0;
    // Start is called before the first frame update
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        myanim = this.GetComponent<Animator>();
        if(GetComponentInChildren<RayScan>()!=null){
            Myeyes = GetComponentInChildren<RayScan>();
        }
        if(GetComponentInChildren<Character_stats>()!=null){
            stats = GetComponentInChildren<Character_stats>();
            mYHp = stats.HP;
        }
    }

    // Update is called once per frame
    void Update()
    {
            lookMySost();
        if(Agressive && !stats.dead){
            //walk = false;
            _timer+=Time.deltaTime;
            if(_timer>=1.5f)
                Ballte_mode();
        }else{
            if(walk && !endbattle)
                move_to_point();
        }
    }

    private void lookMySost(){
        if(stats.HP<=0){
            enemys.Clear();
            GetComponent<Animator>().applyRootMotion = false;
            myanim.SetBool("Dead",true);
            stats.dead = true;
            _timere+=Time.deltaTime;

            GetComponentInChildren<RayScan>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            if(_timere >= 3.25f){
                GetComponent<Animator>().enabled = false;
                this.enabled = false;
            }
        }
        
        if(mYHp>stats.HP){
            if(!hiting && (mYHp-stats.HP)>2)
                myanim.SetBool("Hit",true);
            mYHp = stats.HP;
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

        if(axeReady != null && axenotready != null)
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
        //Quaternion Qvc = enemy.transform.rotation;

        Myeyes.targetTag = enemy.tag;
        isee = Myeyes.isee;

        //if(!isee && !heviatack){
            Quaternion targetRotation = Quaternion.LookRotation(Wvc - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
       // }

        //int hevi_dist = 70;

        if((min>sparing_distance && !heviatack))
        {
            GetComponent<Animator>().applyRootMotion = false;
            transform.position = Vector3.MoveTowards(transform.position,Wvc,speed*Time.deltaTime);
            nowPoint = transform.position;
            myanim.SetFloat("X",0.6f);
            //myanim.SetFloat("Y",Mathf.Abs(startPoint.z-nowPoint.z));
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
                      //  Debug.Log("Choose Attack "+ attack);
                if(attack == 1){
                        rootM = true;
                        myanim.SetBool("Attak", true);
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

        if(enemy.GetComponent<Character_stats>()!=null)
        if(enemy.GetComponent<Character_stats>().dead){
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

    public void hide_weapon(){
        weapon = false;
    }

    public void picup_weapon(){
        weapon = true;
    }

    public void IseeSomething(GameObject other){
        bool frendly = true;
        foreach(string s in Targets_Tag){
            //Debug.Log(other.name+" "+other.tag+" "+ s);
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
                if(other.GetComponent<Character_stats>()!=null)
                    if(!other.GetComponent<Character_stats>().dead)
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
            myanim.SetFloat("X",0.8f);
            Quaternion targetRotation = Quaternion.LookRotation(Wvc - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);

            GetComponent<Animator>().applyRootMotion = false;
            transform.position = Vector3.MoveTowards(transform.position,Wvc,speed*Time.deltaTime);
        
    }
}