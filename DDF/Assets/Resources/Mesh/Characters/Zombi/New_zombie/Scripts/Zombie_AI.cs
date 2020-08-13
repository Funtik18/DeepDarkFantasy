using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDF.Atributes;

public class Zombie_AI : MonoBehaviour
{
    public float speed = 20, speedRotation = 20, gravity = 3, sparing_distance = 15;

    [InfoBox("maxPain - Болевой порог, до которого я игнорирую попадание", InfoBoxType.Normal)]
    public float maxPain = 2;

    [InfoBox("fall - Время, которое я провожу без сознания", InfoBoxType.Normal)]
    public float fall = 10;
    private CharacterController characterController;
    private Animator myanim;
    public bool walk;
    private List<GameObject> enemys = new List<GameObject>();
    private List<GameObject> curse = new List<GameObject>();

    [InfoBox("Targets_Tag - Теги тех кого я не люблю", InfoBoxType.Normal)]
    public List<string> Targets_Tag = new List<string>();
    private bool heviatack,isee,attacking,hiting,Agressive,endbattle;
    private float _timer = 0,_timere = 0,mYHp,_timereat = 0,allmYHp = 0;
    private Vector3 moveDirection = Vector3.zero;
    private RayScan Myeyes; 
    private CharacterStats stats;
    private int nap = 0, heal = 0;
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
            allmYHp = mYHp = stats.CurrentHealthPoints;
        }
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
                {
                    move_to_point();
                }else{
                    if(curse.Count!=0 && !stats.IsDead && !hiting && !attacking)
                        move_to_body();
                }
        }
    }

    private void lookMySost(){
        if(stats.CurrentHealthPoints <= 0){
            enemys.Clear();
            GetComponent<Animator>().applyRootMotion = true;
            myanim.SetBool("Dead",true);
            stats.IsDead = true;
            _timere+=Time.deltaTime;

            GetComponentInChildren<RayScan>().enabled = false;
            GetComponent<CharacterController>().enabled = false;

            if(_timere >= 3.25f){
                myanim.enabled = false;
                this.enabled = false;
            }
        }else
        if(stats.CurrentHealthPoints <= allmYHp/3){
            //Debug.Log(stats.HP+"Beee");
            endAnim();
            GetComponent<Animator>().applyRootMotion = true;
            myanim.SetBool("Dead",true);
            stats.IsDead = true;
            Agressive = false;
            GetComponentInChildren<RayScan>().enabled = false;
            enemys.Clear();
            _timere+=Time.deltaTime;
            //Debug.Log(_timere);
            if(_timere>=fall){
                GetComponentInChildren<RayScan>().enabled = true;
                stats.IsDead = false;
                myanim.SetBool("Dead",false);
                stats.CurrentHealthPoints = 60;
                _timere = 0;
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
        }
        myanim.SetBool("Battle",Agressive);

        foreach(GameObject g in Myeyes.inview){
            IseeSomething(g);
        }

        if(transform.position.y>0.7){
            moveDirection.y -= gravity * Time.deltaTime;
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
        Quaternion Qvc = enemy.transform.rotation;

        Myeyes.targetTag = enemy.tag;
        isee = Myeyes.isee;

        if(!isee && !heviatack){
            Quaternion targetRotation = Quaternion.LookRotation(Wvc - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
        }

        if(min>sparing_distance && !attacking)
            {
                GetComponent<Animator>().applyRootMotion = true;
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
                    myanim.SetBool("Attak", true);
                    GetComponent<Animator>().applyRootMotion = true;
                }
            }
        } 

        if(enemy.GetComponent<CharacterStats>()!=null)
        if(enemy.GetComponent<CharacterStats>().IsDead){
            curse.Add(enemy);
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
        myanim.SetFloat("X",0.0f);
        heviatack = false;
        attacking = false;
        hiting = false;
        endbattle = false;
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
		if (Physics.Raycast (pos, Wvc, out hit, 10) )
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

     private void move_to_body(){
        //Debug.Log("i want eat "+heal);
        GameObject body = curse[curse.Count-1];
        GetComponent<IK_Controls>().lookObj = body.transform;
        Vector3 Wvc = new Vector3 (body.transform.position.x,transform.position.y,body.transform.position.z);
        Quaternion Qvc = body.transform.rotation;
        float distance = Vector3.Distance(transform.position,Wvc);
        if((distance>5))
        {
             Quaternion targetRotation = Quaternion.LookRotation(Wvc - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
            GetComponent<Animator>().applyRootMotion = true;
            myanim.SetFloat("X",stats.speed);
        }
        else{
            myanim.SetBool("jump_strafe",true);
            if(_timereat >= 5){
                stats.CurrentHealthPoints += 5;
                heal+=5;
                _timereat = 0;
            }
            if(heal>=20){
                curse.RemoveAt(curse.Count-1);
                myanim.SetBool("jump_strafe",false);
                heal = 0;
            }
            _timereat+=Time.deltaTime;
        }
    }

}
