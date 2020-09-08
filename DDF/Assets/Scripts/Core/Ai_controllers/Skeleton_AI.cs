using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDF.Atributes;
using DDF.Character.Stats;
using DDF.AI;

public class Skeleton_AI : AI_Entity
{

    protected override void myMind(){
        if((min>sparing_distance && !heviatack))
        {
            GetComponent<Animator>().applyRootMotion = true;
            //transform.position = Vector3.MoveTowards(transform.position,Wvc,speed*Time.deltaTime);
            //nowPoint = transform.position;
            myanim.SetFloat("X",stats.CurrentSpeed);
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
    }
}
