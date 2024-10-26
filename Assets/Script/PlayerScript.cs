using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    const float moveSpeed = 5.0f;

    private CharacterController characterController;
    private Vector3 velocity;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private GameObject damageEffect;
    [SerializeField]
    private MyStatus myStatus;
    //ジャンプ力
    [SerializeField]
    private float jumpPower = 5f;

    private ChangeEquipScript changeequipscript;
    [SerializeField]
    private PlayerLockon lockon;
    private const float RotateSpeed = 900f;
    private const float RotateSpeedLockon = 500f;


    private Vector3 move;
    private Vector3 moveForward;
    [SerializeField]
    private float moveSpeed2;
    [SerializeField]
    private float turnTimeRate = 0.5f;

    private CameraScript camera3D;

    public enum MyState
    {
        Normal,
        Damage,
        Attack,
        Dead
    };
    [SerializeField]
    private MyState state;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        changeequipscript = GetComponent<ChangeEquipScript>();

        lockon = GetComponent<PlayerLockon>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
        camera3D = Camera.main.GetComponent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == MyState.Normal)
        {
            if (characterController.isGrounded)
            {
                //velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
              

                if (Input.GetKey(KeyCode.Space) && !animator.IsInTransition(0) && changeequipscript.GetEquipment() >= 1)
                {
                    SetState(MyState.Attack);

                }
                if (Input.GetKey(KeyCode.F))
                {
                    animator.SetBool("Jump", true);
                    velocity.y += jumpPower;
                }
            }
            
        }
        Move();
        //velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(rb.velocity * walkSpeed * Time.deltaTime);
      

        if(lockon.isLockon)
        {
            Quaternion from = transform.rotation;
            var dir = lockon.GetLockonCameraLookAtTransform().position - transform.position;
            dir.y = 0;
            Quaternion to = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(from, to, RotateSpeedLockon * Time.deltaTime);
        }
        else
        {
            //Quaternion from = transform.rotation;
            //Quaternion to = Quaternion.LookRotation(moveSpeed)
        }
    }

    public void TakeDamage(Transform enemyTransform,Vector3 attackedPlace,int damage)
    {
        state = MyState.Damage;
        velocity = Vector3.zero;
        animator.SetTrigger("Damage");
        var damageEffectIns = Instantiate<GameObject>(damageEffect, attackedPlace, Quaternion.identity);
        Destroy(damageEffectIns, 1f);
        myStatus.SetHp(myStatus.GetHp() - damage);
        if(myStatus.GetHp()<=0)
        {
            Dead();
        }
    }

    public void SetState(MyState tempState)
    {
        if(tempState == MyState.Normal)
        {
            state = MyState.Normal;
        }else if(tempState==MyState.Attack)
        {
            velocity = Vector3.zero;
            state = MyState.Attack;
            if (changeequipscript.GetEquipment() == 1)
            {
                animator.SetTrigger("Attack");
            }
            else if(changeequipscript.GetEquipment()==2)
            {
                animator.SetTrigger("AxeAttack");
            }
        }
        else if(tempState == MyState.Dead)
        {
            animator.SetTrigger("Dead");
            velocity = Vector3.zero;
        }
        
    }

    public void Damage(int damage)
    {
        animator.SetTrigger("Damage");
        velocity = new Vector3(0f, velocity.y, 0f);
        state = MyState.Damage;
        myStatus.SetHp(myStatus.GetHp() - damage);
        if (myStatus.GetHp() <= 0)
        {
            Dead();
        }
    }

    public MyState GetState()
    {
        return state;
    }

    void Dead()
    {
        SetState(MyState.Dead);
    }

    void FixedUpdate()
    {
        if (camera3D.rock)
        {

            var dir = camera3D.RockonTarget.transform.position - this.gameObject.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,/* Time.deltaTime **/ turnTimeRate);
        }
        else
        {
            Rotation();
        }
    }

    private void Move()
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1).normalized);
        moveForward = cameraForward * move.z + Camera.main.transform.right * move.x;
        moveForward = moveForward.normalized;

        if(move.magnitude>0)
        {
            rb.velocity = moveForward * moveSpeed2 * move.magnitude + new Vector3(0, velocity.y, 0);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        if (move.magnitude > 0
                   && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
                   && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
                   && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        {
            animator.SetFloat("Speed", rb.velocity.magnitude);
            transform.LookAt(transform.position + velocity);

        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    private void Rotation()
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1).normalized);
        moveForward = cameraForward * move.z + Camera.main.transform.right * move.x;
        moveForward = moveForward.normalized;

        if(move.magnitude>0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnTimeRate);
        }
        else
        {
            Quaternion targetRotation = transform.rotation;
            transform.rotation = targetRotation;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !animator.IsInTransition(0) && changeequipscript.GetEquipment() >= 1)
        {
            SetState(MyState.Attack);

        }
    }

}
