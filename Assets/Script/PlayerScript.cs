using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    const float moveSpeed = 5.0f;
    [SerializeField]
    private Vector3 velo;

    private CharacterController characterController;
    [SerializeField]
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

    //回避
    [SerializeField]
    private bool avoid = false;
    [SerializeField]
    private bool mov = true;
    [SerializeField]
    private bool rotate = true;
    [SerializeField]
    private PlayableDirector[] timeline;

    private bool isJump;

    [SerializeField]
    private GameObject gameoverUI;

    private int startavoidcooltime;

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
        gameoverUI.SetActive(false);
        timeline[0].Stop();
        timeline[1].Stop();
    }

    // Update is called once per frame
    void Update()
    {
        velo=rb.velocity;
        if (state == MyState.Normal)
        {
            if (characterController.isGrounded)
            {
                //velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
              

                if (Input.GetKey(KeyCode.Space) && !animator.IsInTransition(0) && changeequipscript.GetEquipment() >= 1)
                {
                    SetState(MyState.Attack);

                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    animator.SetBool("Jump", true);
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    rb.velocity = new Vector3(0, rb.velocity.y + jumpPower, 0);
                    isJump = true;
                }
                else
                {
                    isJump = false;
                   
                }
               
            }
            
        }
        
        if (mov)
        {
            Move();
        }
        else
        {
            if(avoid)
            {
                rb.AddForce(-transform.forward * 5.0f, ForceMode.Impulse);
            }
        }

        if(startavoidcooltime<10)
        {
            startavoidcooltime++;
        }

        //velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(rb.velocity  * Time.deltaTime);
    }

    public void TakeDamage(Transform enemyTransform,Vector3 attackedPlace,int damage)
    {
        if (state != MyState.Dead)
        {
            state = MyState.Damage;
            velocity = Vector3.zero;
            animator.SetTrigger("Damage");
            var damageEffectIns = Instantiate<GameObject>(damageEffect, attackedPlace, Quaternion.identity);
            Destroy(damageEffectIns, 1f);
            myStatus.SetHp(myStatus.GetHp() - damage);
        }
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
            state = MyState.Dead;
            animator.SetTrigger("Dead");
            velocity = Vector3.zero;
        }
        
    }

    public void Damage(int damage)
    {   
        if (state != MyState.Dead)
        {
            animator.SetTrigger("Damage");
            velocity = new Vector3(0f, velocity.y, 0f);
            state = MyState.Damage;
            myStatus.SetHp(myStatus.GetHp() - damage);
        }
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
        gameoverUI.SetActive(true);
        SetState(MyState.Dead);
        state = MyState.Dead;
    }

    void FixedUpdate()
    {
        if (rotate)
        {
            if (camera3D.rock)
            {

                var dir = camera3D.RockonTarget.transform.position - this.gameObject.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnTimeRate);
            }
            else
            {
                Rotation();
            }
        }
    }

    private void Move()
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1).normalized);
        moveForward = cameraForward * move.z + Camera.main.transform.right * move.x;
        moveForward = moveForward.normalized;

        if(move.magnitude>0)
        {
            rb.velocity = moveForward * moveSpeed2 * move.magnitude + new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            //rb.velocity = new Vector3(0, rb.velocity.y, 0);
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

        if (transform.position.y < 0)
        {
            rb.useGravity = false;
            rb.velocity = moveForward * moveSpeed2 * move.magnitude + new Vector3(0, rb.velocity.y, 0);
            
        }
        else
        {
            rb.useGravity = true;
            //rb.velocity = new Vector3(0, -1, 0);
        }

        if (avoid)
        {
            if(move.magnitude>0)
            {
                rb.AddForce(moveForward * 50.0f, ForceMode.Impulse);
            }
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

    public void OnMoveOn() { mov = true; }
    public void OnMoveOff() { mov = false; }
    public void RotationOn() { rotate = true; }
    public void RotationOff() { rotate = false; }
    public void ActionFlagReset() { avoid = false; }

    public void OnAvoid(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(!avoid&&state!=MyState.Damage)
            {
                if(move.magnitude>0)
                {
                    timeline[0].Play();
                    RotationOff();
                    Debug.Log("後ろ回避");
                }
                else
                {
                    timeline[1].Play();
                    OnMoveOff();
                    RotationOff();
                    Debug.Log("移動回避");
                }

                avoid = true;
            }
        }
      
    }

    public bool GetAvoid()
    {
        return avoid;
    }

    

}
