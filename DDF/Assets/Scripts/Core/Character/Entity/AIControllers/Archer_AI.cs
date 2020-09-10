using UnityEngine;
using DDF.AI;

public class Archer_AI : AI_Entity
{
    public GameObject bowReady, bowNotReady;
    public GameObject arrowInKolchan;
    public Transform bowString;
   
    private bool irotate,arrow;
   
    private Vector3 moveforward = Vector3.zero;
    
    private int way_number = 0;


    protected override void lookMySost(){
        if(stats.CurrentHealthPoints <= 0){

            GetComponent<IK_Controls>().rightHandObj = null;
            bowReady.GetComponent<bow>().shoot = true;

            enemies.Clear();
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

    protected override void myMind(){
             if((min>sparing_distance))
        {
            GetComponent<Animator>().applyRootMotion = true;
            myanim.SetFloat("X",stats.CurrentSpeed);
        }
         else
            if((min<sparing_distance-20))
            {
                myanim.SetFloat("X",stats.CurrentSpeed * -1);
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
    }

    public override void endAnim()
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

    public void take_arrow(){
        arrow = !arrow;
    }

}
