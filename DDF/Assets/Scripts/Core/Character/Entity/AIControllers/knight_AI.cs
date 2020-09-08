using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDF.Atributes;
using DDF.Character.Stats;
using DDF.AI;

public class knight_AI : AI_Entity
{
    public float spare_dis = 50;

    [InfoBox("LHandPoint - Место для крепления левой руки на пушке", InfoBoxType.Normal)]
    public Transform LHandPoint;


    protected override void myMind(){
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
        GetComponent<IK_Controls>().leftHandObj = null;
    }

}
