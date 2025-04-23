using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.ProBuilder.MeshOperations;

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
    //ロックオン関連
    private CameraScript camera3D;
    //ロックオン状態の時に代入する変数
    private Quaternion rotation;
    [SerializeField]
    private Vector3 move;
    private Vector3 moveForward;
    [SerializeField]
    private float moveSpeed2;
    [SerializeField]
    private float turnTimeRate = 0.5f;

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
    [SerializeField]
    private GameObject gameclearUI;
    [SerializeField]
    private GameObject troll;
    private int startavoidcooltime;
    //多段ヒット防止
    private AttackSwordScript attacksowrdscript;

    //討伐カウンター
    [SerializeField]
    private int deadcaunter;
    [SerializeField]
    private LifeGauge hpgauge;
    //ゲームオーバーフラグ
    [SerializeField]
    private bool isGameOver;

    //プレイヤースクリプトに敵のスプリクトを格納
    private MoveEnemyScript MoveEnemyScript;

    public enum MyState
    {
        Normal,
        Damage,
        Attack,
        SkillAttack,
        Dead
    };
    [SerializeField]
    private MyState state;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        changeequipscript = GetComponent<ChangeEquipScript>();

        rb = this.gameObject.GetComponent<Rigidbody>();
        camera3D = Camera.main.GetComponent<CameraScript>();
        gameoverUI.SetActive(false);
        timeline[0].Stop();
        timeline[1].Stop();
        gameclearUI.SetActive(false);
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの行動関連の処理
        Playerhauding();
    }

    public void TakeDamage(Transform enemyTransform,Vector3 attackedPlace,int damage)
    {
        //倒されていなかったらHPを減らしたりアニメーションなどをする処理
        if (state != MyState.Dead)
        {
            state = MyState.Damage;
            velocity = Vector3.zero;
            animator.SetTrigger("Damage");
            var damageEffectIns = Instantiate<GameObject>(damageEffect, attackedPlace, Quaternion.identity);
            Destroy(damageEffectIns, 1f);
            myStatus.SetHp(myStatus.GetHp() - damage);
           
            hpgauge.SetDamageLifeGauge(damage);
            
        }
        //HPが0になったら倒される処理
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
        }else if(tempState==MyState.Attack)  //攻撃行動
        {
            velocity = Vector3.zero;
            state = MyState.Attack;
            move = Vector3.zero;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            //もし剣を装備していたら剣のアニメーションをする処理
            if (changeequipscript.GetEquipment() == 0|| changeequipscript.GetEquipment() == 2||
                changeequipscript.GetEquipment() ==3)
            {
                animator.SetTrigger("Attack");
            }
            //もし斧を装備していたら斧のアニメーションをする処理
            else if (changeequipscript.GetEquipment()==1 || changeequipscript.GetEquipment() == 4 ||
                    changeequipscript.GetEquipment() == 5)
            {
                animator.SetTrigger("AxeAttack");
            }
            
        }
        else if(tempState==MyState.SkillAttack)
        {
            state = MyState.SkillAttack;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            //宝箱から入手した武器に応じてアニメーションや立ち止まって攻撃するようにする処理
            if (changeequipscript.GetEquipment()==2|| changeequipscript.GetEquipment() == 3)
            {
                animator.SetTrigger("SwordSkillAttack");
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
            else if (changeequipscript.GetEquipment() == 4)
            {
                animator.SetTrigger("AxeSkillAttack2");
            }
            else if(changeequipscript.GetEquipment() == 5)
            {
                animator.SetTrigger("AxeSkillAttack");
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                move = Vector3.zero;
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
        //ただダメージUIをどこでもよいので表示したい用の処理
        if (state != MyState.Dead)
        {
            animator.SetTrigger("Damage");
            velocity = new Vector3(0f, velocity.y, 0f);
            state = MyState.Damage;
            var damageEffectIns = Instantiate<GameObject>(damageEffect, new Vector3(transform.position.x,transform.position.y-1,transform.position.z), Quaternion.identity);
            Destroy(damageEffectIns, 1f);
            myStatus.SetHp(myStatus.GetHp() - damage);
        }
        if (myStatus.GetHp() <= 0)
        {
            Dead();
        }
    }

    public void DeadCaunter(int caunter)
    {
        deadcaunter = deadcaunter + caunter;
    }

    public MyState GetState()
    {
        return state;
    }

    public int SetDeadCaunter()
    {
        return deadcaunter;
    }

    void Dead()
    {
        gameoverUI.SetActive(true);
        SetState(MyState.Dead);
        state = MyState.Dead;
        if(animator.GetBool("Dead")==true)
        {
            isGameOver = true;
        }
    }

    void FixedUpdate()
    {
        if (rotate)
        {
            //ロックオンの時
            if (camera3D.rock)
            {
                //プレイヤーの向きを敵を常に向くようにする
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
        //移動処理
        if (move.magnitude > 0)
        {
            rb.velocity = moveForward * moveSpeed * move.magnitude + new Vector3(0, rb.velocity.y, 0);
        }
        //if(!isJump)
        //{
        //    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //}
        
        //入力された方向に応じてプレイヤーの向きを変える処理
        if (move.magnitude > 0
                   && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
                   && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
                   && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        {
            animator.SetFloat("Speed", rb.velocity.magnitude);
            //入力している方向に回避するための処理y
            if (!camera3D.rock)
            {
                transform.LookAt(transform.position + moveForward);
            }
            else
            {
                transform.LookAt(transform.position);
            }
        }
        //入力していなかったらアニメーションとスピードを0にする処理
        else
        {
            animator.SetFloat("Speed", 0f);
        }
        //地面か空中かで重力を入れるかどうかの処理
        if(transform.position.y<0)
        {
            rb.useGravity = false;
            rb.velocity = moveForward * moveSpeed * move.magnitude + new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            rb.useGravity = true;
        }
    }

    private void Rotation()
    {
        //カメラのずれをなくすための処理
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
        //攻撃以外で移動するようにする処理
        if (state != MyState.Attack || state != MyState.SkillAttack)
        {
            move = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //地面にいて武器を持っていたら攻撃する処理
        if (characterController.isGrounded)
        {
            if (context.started && !animator.IsInTransition(0) && changeequipscript.GetEquipment() >= 0)
            {
                SetState(MyState.Attack);
            }
        }
    }

    public void OnSkillAttack(InputAction.CallbackContext context)
    {
        //宝箱で取った武器だけスキルを使えるようにする処理
        if (context.started && !animator.IsInTransition(0) && changeequipscript.GetEquipment() >=2&&
            context.started && !animator.IsInTransition(0) && changeequipscript.GetEquipment() <= 5)
        {
            SetState(MyState.SkillAttack);

        }
    }
    //回避中に移動できるかどうかの処理
    public void OnMoveOn() { mov = true; }
    public void OnMoveOff() { mov = false; }
    public void RotationOn() { rotate = true; }
    public void RotationOff() { rotate = false; }
    public void ActionFlagReset() { avoid = false; }
    
    public void OnAvoid(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            //入力しているかどうかとダメージ中ではないかの処理
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

    public bool IsGameOver()
    {
        return isGameOver;
    }
    public void SetEnemyScript(MoveEnemyScript moveEnemyScript)
    {
        MoveEnemyScript = moveEnemyScript;
    }

    public MoveEnemyScript GetEnemyScript()
    {
        return MoveEnemyScript;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    void Playerhauding()
    {
        if (Input.GetKeyDown(KeyCode.F) && transform.position.y < 0 || Input.GetKeyDown("joystick button 3") && transform.position.y < 0)
        {
            animator.SetBool("Jump", true);
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.velocity = new Vector3(0, rb.velocity.y + jumpPower, 0);
            isJump = true;
        }
        else
        {
            isJump = false;
            animator.SetBool("Jump", false);
        }

        if (mov)
        {
            Move();
        }

        if (troll == null)
        {
            gameclearUI.SetActive(true);
        }
        
        characterController.Move(rb.velocity * Time.deltaTime);
        
    }

}
