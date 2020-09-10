using DDF.AI;
using DDF.Character.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_AI : AI_Entity
{
    public int fire_dist = 70;
    public GameObject partic_fire;
    private bool fair;
   

    protected override void myMind(){
        partic_fire.SetActive(fair);

        if((min>fire_dist && !heviatack))
        {
            GetComponent<Animator>().applyRootMotion = true;
            //transform.position = Vector3.MoveTowards(transform.position,Wvc,speed*Time.deltaTime);
            myanim.SetFloat("X",1f);
        }
        else
            if(min<=fire_dist && min>60)
            {
                GetComponent<Animator>().applyRootMotion = true;
                myanim.SetBool("Hevi_Attak", true);
                heviatack = true;
            }
            else
                if(min>sparing_distance && !attacking && !heviatack)
                {
                    GetComponent<Animator>().applyRootMotion = true;
                    myanim.SetFloat("X",1f);
                   //transform.position = Vector3.MoveTowards(transform.position,Wvc,speed*Time.deltaTime);
                }
                else
                {
                myanim.SetFloat("X",0f);
                if(!heviatack)
                    {
                    if(!attacking)
                    {
                        attacking = true;
                        bool rootM = false;
                        int attack = Random.Range(1,4);
                        Debug.Log("Choose Attack "+ attack);
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
                            myanim.SetBool("Hevi_Attak", true);
                        }
                    GetComponent<Animator>().applyRootMotion = rootM;
                    }
                }
            } 
    }
    public override void endAnim()
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
        partic_fire.SetActive(false);
    }

    public void FAIIR()
    {
        fair = !fair;
    }

}
