using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DDF.Character;
public class NPlayerMovement : MonoBehaviour
{
    public Transform CameraTransform;
    public CharacterStatus characterStatus;

    public Vector3 rotationDirection;
    public Vector3 moveDirection;

    public float vertical;
    public float horizontal;
    public float moveAmount;
    public float rotationSpeed;
    public int bufSpeedtoRun = 8;
    public float gravity = 9f;
    private int runSpeed;

    [HideInInspector]
    public bool freezMovement = false;
    
    private Animator animator;
    private CharacterController controller;
    [SerializeField] private CharacterEntity characterEntity;

    public void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update() {
        
    }
    public void FixedUpdate()
    {
        MoveUpdate();
        IsDead();
    }
    public void MoveUpdate()
    {
        if (!freezMovement)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

            Vector3 moveDir = transform.forward * vertical;
            moveDir += transform.right * horizontal;
            moveDir.Normalize();
            moveDirection = moveDir;
            rotationDirection = transform.forward;

            RotationNormal(vertical);

            CheckKeysPressed();

            animator.SetFloat("vertical", vertical, 0.15f, Time.deltaTime);
            animator.SetFloat("horizontal", horizontal, 0.15f, Time.deltaTime);


            characterStatus.isGround = Ground();
            if(!Ground()){
                animator.SetBool("jump",true);
                runSpeed = 0;
                moveDirection.y += -gravity*Time.deltaTime*10;
            }else{
                animator.SetBool("jump",false);
            }   

            if(controller!= null)
                controller.Move(moveDirection*Time.deltaTime*(characterEntity.MaxSpeed+runSpeed));//moveDirection/(10-speed));//Нужно придумать как регулировать скорость
        }
    }

    public void CheckKeysPressed(){
        if(Input.GetAxis("Run")>0 && vertical>=0)
        {
            vertical*=2;
            horizontal*=2;
            runSpeed = bufSpeedtoRun;
        }else
        {
            runSpeed = 0;
        }
            
        if(Input.GetButtonDown("Jump") && Ground())
        {
            Debug.Log("Jumping");
            moveDirection.y += 50;
        }
    }

    public void IsDead(){
        if(characterEntity.IsDead){
            freezMovement = true;
            animator.enabled = false;
            animator.SetBool("dead",true);
        }
    }
    public void RotationNormal(float nap)
    {
        //Vector3 moveDir = CameraTransform.forward * vertical;
        Vector3 moveDir = transform.forward * vertical;
        moveDir += CameraTransform.right * horizontal;
        //moveDir += transform.right * horizontal;
        moveDir.Normalize();
        moveDirection = moveDir;
        rotationDirection = CameraTransform.forward;

        Vector3 targetDir = rotationDirection;
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
            targetDir = transform.forward;


        Quaternion lookDir = Quaternion.LookRotation(targetDir);
        Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, 0.15f);
        transform.rotation = targetRot;
    }

    public bool Ground()
    {
        Vector3 origin = transform.position;
        origin.y += 0.6f;
        Vector3 dir = -Vector3.up;
        float dis = 0.7f;
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, dis))
        {
            Vector3 tp = hit.point;
            transform.position = tp;
            return true;
        }

        return false;
    }

    public void DoorOpen()
    {
        animator.SetBool("open", false);
        freezMovement = false;
    }
}
