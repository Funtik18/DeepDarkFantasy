using DDF.Character;
using DDF.Character.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_AI : MonoBehaviour
{
    public float speed = 20, speedRotation = 20, gravity = 3, sparing_distance = 15;
    private CharacterController characterController;
    private Animator myanim;
    public bool walk;
    private List<GameObject> enemys = new List<GameObject>();
    public List<string> Targets_Tag = new List<string>();
    public List<Transform> Waypoints = new List<Transform>();
    private Vector3 startPoint, nowPoint;
    private bool heviatack,isee,attacking,hiting,weapon,Agressive,endbattle,fair;
    private float _timer = 0,_timere = 0,mYHp;
    private Vector3 moveDirection = Vector3.zero;
    private RayScan Myeyes; 
    private CharacterEntity stats;
    private int way_number = 0,nap = 0;
    // Start is called before the first frame update
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        myanim = this.GetComponent<Animator>();
        if(GetComponentInChildren<RayScan>()!=null){
            Myeyes = GetComponentInChildren<RayScan>();
        }
        if(GetComponentInChildren<CharacterEntity>()!=null){
            stats = GetComponentInChildren<CharacterEntity>();
            mYHp = stats.CurrentHealthPoints;
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
        }
    }

    private void lookMySost(){
        if(stats.CurrentHealthPoints <= 0){
            enemys.Clear();
            GetComponent<Animator>().applyRootMotion = false;
            myanim.SetBool("Dead",true);
            stats.IsDead = true;
            _timere+=Time.deltaTime;

            GetComponentInChildren<RayScan>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            if(_timere >= 3.25f){
                myanim.enabled = false;
                this.enabled = false;
            }
        }
        
        if(mYHp>stats.CurrentHealthPoints) {
            if(!hiting && (mYHp-stats.CurrentHealthPoints ) >5)
                myanim.SetBool("Hit",true);
            mYHp = stats.CurrentHealthPoints;
        }

        if(enemys.Count != 0 || hiting){
            Agressive = true;
        }
        else{
            Agressive = false;
            GetComponent<IK_Controls>().lookObj = null;
            _timer = 0;
            startPoint = transform.position;
        }
        myanim.SetBool("Battle",Agressive);

        foreach(GameObject g in Myeyes.inview){
            IseeSomething(g);
        }
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

        int hevi_dist = 80;

        if(min>hevi_dist)
        {
        }
        else
            if(min<=hevi_dist && min>50)
            {
                myanim.SetBool("Hevi_Attak", true);
            }
            else
                if(min>sparing_distance)
                {
                    myanim.SetBool("around_attack",true);
                }
                else
                {
                if(!heviatack)
                    {
                    if(!attacking)
                    {
                        attacking = true;
                        myanim.SetBool("Attak", true);
                    }
                }
            } 

        if(enemy.GetComponent<CharacterEntity>()!=null)
        if(enemy.GetComponent<CharacterEntity>().IsDead) {
            enemys.Remove(enemy);
            endbattle = true;
        }   
    }

    public void endAnim()
    {
        myanim.SetBool("Attak",false);
        myanim.SetBool("Hevi_Attak",false);
        myanim.SetBool("around_attack", false);
        myanim.SetBool("Voise",false);
        attacking = false;
        hiting = false;
        endbattle = false;
    }

    public void FAIIR()
    {
        fair = !fair;
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
                if(other.GetComponent<CharacterEntity>()!=null)
                    if(!other.GetComponent<CharacterEntity>().IsDead) {
                        enemys.Add(other);
                        myanim.SetBool("Voise",true);
                        }
            }
        }
    }

}
