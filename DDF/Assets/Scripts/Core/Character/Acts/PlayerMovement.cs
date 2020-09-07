using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public Transform CameraTransform;
    public CharacterStatus characterStatus;


    public Vector3 rotationDirection;
    public Vector3 moveDirection;

    public float vertical;
    public float horizontal;
    public float moveAmount;
    public float rotationSpeed;

    [HideInInspector]
    public bool freezMovement = false;
    
    private Animator Animator;
    private CharacterController controller;

    public void Start()
    {
        Animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public void FixedUpdate()
    {
        MoveUpdate();
    }
    public void MoveUpdate()
    {
        if (!freezMovement)
        {
            int speed = 0;//временная переменная
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

            //moveAmountV = Mathf.Clamp01(Mathf.Abs(vertical)+Mathf.Abs(horizontal));

            if(Input.GetAxis("Run")>0)
            {
                vertical*=2;
                horizontal*=2;
                speed = 7;
            }else
                {
                    speed = 0;
                }

            Animator.SetFloat("vertical", vertical, 0.15f, Time.deltaTime);
            Animator.SetFloat("horizontal", horizontal, 0.15f, Time.deltaTime);
            /*
            Vector3 moveDir = CameraTransform.forward * vertical;
            moveDir += CameraTransform.right * horizontal;
            moveDir.Normalize();
            moveDirection = moveDir;
            rotationDirection = CameraTransform.forward;
            */
            Vector3 moveDir = transform.forward * vertical;
            moveDir += transform.right * horizontal;
            moveDir.Normalize();
            moveDirection = moveDir;
            rotationDirection = transform.forward;

            if(vertical>0)
                RotationNormal();
            characterStatus.isGround = Ground();
            if(!Ground())
                moveDirection+=-transform.up;   

            if(controller!= null)
            controller.Move(moveDirection/(10-speed));//Нужно придумать как регулировать скорость
        }
    }

    public void RotationNormal()
    {
        //rotationDirection = moveDirection;
        Vector3 moveDir = CameraTransform.forward * vertical;
        moveDir += CameraTransform.right * horizontal;
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
        Animator.SetBool("open", false);
        freezMovement = false;
    }
}
