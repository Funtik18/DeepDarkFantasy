using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDF.Atributes;
using DDF.Character.Stats;
using DDF.AI;

public class Zombie_AI : AI_Entity
{
    public float fall = 5;
    private int heal = 0;
    private float _timereat = 0;
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

    protected override void lookMySost(){
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
        if(stats.CurrentHealthPoints <= stats.MaxHealthPoints/3){
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
            //if(!Agressive)
               // IseeSomething(stats.Iam);
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


    protected override void myMind(){
        if(min>sparing_distance && !attacking)
            {
                GetComponent<Animator>().applyRootMotion = true;
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
                    myanim.SetBool("Attak", true);
                    GetComponent<Animator>().applyRootMotion = true;
                }
            }
        } 
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
            myanim.SetFloat("X",stats.CurrentSpeed);
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
