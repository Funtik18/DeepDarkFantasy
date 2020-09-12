using UnityEngine;
using DDF.Character;
using DDF.UI.Inventory;
using DDF.UI.Inventory.Items;
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
    public float gravity = 1f;
    public float jumpHight = 500;
    public Transform placeRHand,placeLHand;
    public GameObject TestWeapon;
    private int runSpeed;

    [HideInInspector]
    public bool freezMovement = false;
    
    private Animator animator;
    private CharacterController controller;
    private bool jumping = false;
    private string textRweapon = "";
    [SerializeField] private CharacterEntity characterEntity;

    public void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update() {

        if(Input.GetButtonDown("Jump") && Ground())
        {
            jumping = true;
            Debug.Log("Jumping");
        }

    }
    public void FixedUpdate()
    {

        //LookMyEquip();
        MoveUpdate();
        MyIsDead();
        
    }
    /*public void LookMyEquip(){
        Inventory RHand = characterEntity.equipment.rHandEquipment;
        if(RHand.currentItems.Count>0)
        {
            Item item =  RHand.currentItems[0];
            GameObject weapon = TestWeapon;//item.
            string nameWeapon = item.itemName;
            if(!textRweapon.Equals(nameWeapon))
            {
                textRweapon = nameWeapon;
                GameObject Rweapon = Instantiate(weapon, placeRHand.position, placeRHand.rotation);
                Rweapon.transform.parent = placeRHand;
                Rigidbody rig = Rweapon.GetComponent<Rigidbody>();
                if(rig!=null){
                    rig.isKinematic = true;
                }
            }
        }
    }*/
    public void MoveUpdate()
    {
        if (!freezMovement)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

            Vector3 becup = new Vector3(moveDirection.x,moveDirection.y,moveDirection.z);
            moveDirection = transform.forward*vertical;
            moveDirection += transform.right*horizontal;
            moveDirection.y += becup.y;
            /*
            Vector3 moveDir = transform.forward * vertical;
            moveDir += transform.right * horizontal;
            moveDir += transform.up;
            moveDir.Normalize();
            moveDirection = moveDir;
            //moveDirection.x = horizontal;
            //moveDirection.z = vertical;
            rotationDirection = transform.forward;*/

            

            CheckKeysPressed();

            animator.SetFloat("vertical", vertical, 0.15f, Time.deltaTime);
            animator.SetFloat("horizontal", horizontal, 0.15f, Time.deltaTime);

            if(jumping){
                moveDirection.y = 10;
                horizontal = 3;
            }

            RotationNormal();

            characterStatus.isGround = Ground();
            if(!Ground()){
                animator.SetBool("jump",true);
                runSpeed = 0;
                moveDirection.y += -gravity*Time.deltaTime*4;
            }else{
                animator.SetBool("jump",false);
                jumping = false;
            }   

            if(controller!= null)
                controller.Move(moveDirection*Time.deltaTime*(characterEntity.MaxSpeed+runSpeed));//moveDirection/(10-speed));//Нужно придумать как регулировать скорость
        }
    }

    public void CheckKeysPressed(){
        if(Input.GetAxis("Run")>0 && vertical>=0)
        {
            vertical *= 2;
            horizontal *= 2;
            runSpeed = bufSpeedtoRun;
        }else
        {
            runSpeed = 0;
        }
            
    }

    public void MyIsDead(){
        if(characterEntity.IsDead){
            freezMovement = true;
            animator.enabled = false;
            animator.SetBool("dead",true);
        }
    }
    public void RotationNormal()
    {
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
