using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (state == MyState.Normal)
        {
            if (characterController.isGrounded)
            {
                velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

                if (velocity.magnitude > 0.1f
                    &&!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
                    &&!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
                    &&!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                {
                    animator.SetFloat("Speed", velocity.magnitude);
                    transform.LookAt(transform.position + velocity);
                   
                }
                else
                {
                    animator.SetFloat("Speed", 0f);
                }
                if (Input.GetKey(KeyCode.Space)&&!animator.IsInTransition(0))
                {
                    SetState(MyState.Attack);

                }
                if(Input.GetKey(KeyCode.F))
                {
                    animator.SetBool("Jump", true);
                    velocity.y += jumpPower;
                }
            }
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * walkSpeed * Time.deltaTime);
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
            animator.SetTrigger("Attack");
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

}
