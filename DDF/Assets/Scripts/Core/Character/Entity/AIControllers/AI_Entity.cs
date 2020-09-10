using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDF.Atributes;
using DDF.Character.Stats;
using DDF.Character;
using System;

namespace DDF.AI {
    public class AI_Entity : MonoBehaviour
    {
    public float speed = 20, speedRotation = 20, gravity = 3, sparing_distance = 15;
    
    [InfoBox("maxPain - Болевой порог, до которого я игнорирую попадание", InfoBoxType.Normal)]
    public float maxPain = 2;

    [InfoBox("hevi_dist - Дистанция от врага для особой атаки. hevi_dist > sparing_distance", InfoBoxType.Normal)]
    public int hevi_dist = 70;
    protected CharacterController characterController;
    protected Animator myanim;
    public bool walk;
    public GameObject axeReady, axenotready;
    protected GameObject enemy;
    protected List<GameObject> enemies = new List<GameObject>();
    protected List<GameObject> curse = new List<GameObject>();

    [InfoBox("Targets_Tag - Теги тех кого я не люблю", InfoBoxType.Normal)]
    public List<string> Targets_Tag = new List<string>();
    protected bool heviatack,isee,attacking,hiting,weapon,Agressive,endbattle;
    protected float _timer = 0,_timere = 0,mYHp;
    protected Vector3 moveDirection = Vector3.zero,Wvc;
    protected RayScan Myeyes; 
    protected NPSEntity stats;
    protected int nap = 0;
    protected float min;
    // Start is called before the first frame update
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        myanim = this.GetComponent<Animator>();
        if(GetComponentInChildren<RayScan>()!=null){
            Myeyes = GetComponentInChildren<RayScan>();
        }
        if(GetComponentInChildren<NPSEntity>()!=null){
            stats = GetComponentInChildren<NPSEntity>();
            mYHp = stats.CurrentHealthPoints;
        }else{Debug.Log("Подключи NPS Entity");}
    }

    // Update is called once per frame
    void Update()
    {
        lookMySost();
        if(Agressive){
            _timer+=Time.deltaTime;
            if(_timer>=1.5f)
                Ballte_mode();
        }else{
            if(walk && !stats.IsDead && !endbattle)
                move_to_point();
        }
    }

    /// <summary>
        /// Проверяю свое состояние.
    /// </summary>
    protected virtual void lookMySost(){
        if(stats.CurrentHealthPoints <= 0){
            enemies.Clear();
            GetComponent<Animator>().applyRootMotion = false;
            myanim.SetBool("Dead",true);
            stats.IsDead = true;
            _timere+=Time.deltaTime;

				try {
                    GetComponentInChildren<RayScan>().enabled = false;
                    GetComponent<CharacterController>().enabled = false;
                    GetComponent<CapsuleCollider>().enabled = false;
                } catch(Exception e) {
                    
                }
            

            if(_timere >= 3.25f){
                myanim.enabled = false;
                this.enabled = false;
            }
        }
        
        if(mYHp>stats.CurrentHealthPoints) {
            if(!hiting && (mYHp-stats.CurrentHealthPoints ) >maxPain)
                myanim.SetBool("Hit",true);
            mYHp = stats.CurrentHealthPoints;
            //if(!Agressive)
               // IseeSomething(stats.Iam);
        }

        if(enemies.Count != 0 || hiting){
            Agressive = true;
        }
        else{
            Agressive = false;
            GetComponent<IK_Controls>().lookObj = null;
            _timer = 0;
            myanim.SetFloat("X",0);
        }
        myanim.SetBool("Battle",Agressive);

        if(axenotready!=null && axenotready!=null)
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
        if(characterController != null)
         if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        if(!stats.IsDead)
            if(characterController!= null)
                characterController.Move(moveDirection * Time.deltaTime);
    } 

    /// <summary>
        /// Мое состояние в бою.
    /// </summary>
    protected virtual void Ballte_mode(){
        min = 100;
        int id = 0;
        for(int i = 0;i<enemies.Count;i++){
            float dist = Vector3.Distance(enemies[i].transform.position,transform.position);
            if(dist<min){
                min = dist;
                id = i;
            }
        }
        enemy = enemies[id];
        GetComponent<IK_Controls>().lookObj = enemy.transform;
        Wvc = new Vector3 (enemy.transform.position.x,transform.position.y,enemy.transform.position.z);
        Quaternion Qvc = enemy.transform.rotation;

        Myeyes.targetTag = enemy.tag;
        isee = Myeyes.isee;

        if(!isee && !heviatack){
            Quaternion targetRotation = Quaternion.LookRotation(Wvc - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
        }

        myMind();
        Entity entity = enemy.transform.root.GetComponent<Entity>();
        if(entity !=null)
        if(entity.IsDead) {
            curse.Add(enemy);
            enemies.Remove(enemy);
            endbattle = true;
        }
 
    }

    /// <summary>
        /// Мои действия в бою.
    /// </summary>
    protected virtual void myMind(){
        if((min>hevi_dist && !heviatack))
        {
            GetComponent<Animator>().applyRootMotion = true;
            //transform.position = Vector3.MoveTowards(transform.position,Wvc,speed*Time.deltaTime);
            myanim.SetFloat("X",stats.CurrentSpeed);
            //myanim.SetFloat("Y",Mathf.Abs(startPoint.z-nowPoint.z));
        }
        else
            if(min<=hevi_dist && min>hevi_dist-10)
            {
                GetComponent<Animator>().applyRootMotion = true;
                myanim.SetBool("Hevi_Attak", true);
                heviatack = true;
            }
            else
                if(min>sparing_distance && !attacking)
                {
                    myanim.SetFloat("X",stats.CurrentSpeed);
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
                        int attack = UnityEngine.Random.Range(1,5);
                        //Debug.Log("Choose Attack "+ attack);
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
                    if(attack == 4){
                            myanim.SetBool("jump_strafe", true);
                            rootM = true;
                            heviatack = true;
                        }
                    GetComponent<Animator>().applyRootMotion = rootM;
                    }
                }
            } 
    }
    public virtual void endAnim()
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
    }

    public void hide_weapon(){
        weapon = false;
    }

    public void picup_weapon(){
        weapon = true;
    }

    public virtual void IseeSomething(GameObject other){
        bool frendly = true;
        foreach(string s in Targets_Tag){
            if(other != null)
            if(other.tag.Equals(s)){
                frendly = false;
            }
        }
        bool have = false;
        if(!frendly){
            foreach(GameObject g in enemies){
                if(other.Equals(g)){
                    have = true;
                }
            }
            if(!have){
               // Debug.Log(" Я знаю тебя Мы не встречались");
                Entity entity = other.transform.root.GetComponent<Entity>();
                if(entity!=null){
                    //Debug.Log("У тебя есть Entity");
                    if(!entity.IsDead)
                        enemies.Add(other);
                    //Debug.Log("ты мертв " +entity.IsDead);
                }
            }
        }
     //Debug.Log("See Enemy,"+other.tag+" His friend? "+frendly+" i see him agane? "+have);
    }

    protected virtual void move_to_point(){

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
                nap = UnityEngine.Random.Range(0,4);
                //nap = (nap+1)%4;
            Debug.Log(hit.transform.name+" "+nap);
        }
            //float dist = Vector3.Distance(transform.position,Wvc);
            myanim.SetFloat("X",stats.CurrentSpeed);
            Quaternion targetRotation = Quaternion.LookRotation(Wvc - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);

            GetComponent<Animator>().applyRootMotion = true;
            //transform.position = Vector3.MoveTowards(transform.position,Wvc,speed*Time.deltaTime);
        
    }
    }
}
