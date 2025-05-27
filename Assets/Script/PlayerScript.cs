using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Playables;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    //const float moveSpeed = 5.0f;
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
    private float jumpPower = 15f;

    private ChangeEquipScript changeequipscript;
    //ロックオン関連
    [SerializeField]
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
    [SerializeField]
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
    [SerializeField]
    private MoveEnemyScript moveEnemyScript;

    //走るスピード
    //[SerializeField]
    //private float dashSpeed = 32f;
    //走っているかどうか
    [SerializeField]
    private bool run = false;
    //最初に移動ボタンを押したかどうかの変数
    [SerializeField]
    private bool push = false;
    //次に移動ボタンが押されるまでの時間
    [SerializeField]
    private float nextButtonDownTime = 0.3f;
    //最初に移動ボタンが押されてからの経過時間
    private float nowTime = 0f;

    //最初に押した方向との違いの限度角度
    [SerializeField]
    private float limitAngle = 3f;
    //移動キーの押した方向
    private Vector2 direction = Vector2.zero;
    //ジャスト回避が出来るかのフラグ
    private bool isJustAvoid;
    //ジャスト回避の後の攻撃が出来るかのフラグ
    [SerializeField]
    private bool isJustAvoidAttack;
    //ジャスト回避時フラグを立たせると敵に移動する変数
    [SerializeField]
    private bool isJustAvoidMove;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject serchCircle;
    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();

    [SerializeField]
    private Vector3 debugposition;
    [SerializeField]
    List<Vector3> enemyPositions = new List<Vector3>();
    [SerializeField]
    private MoveEnemyScript nearestEnemyScript;
    [SerializeField]
    private int avoidCaunter;
    [System.Serializable]
    public class MoveSettings
    {
        public float moveSpeed = 5.0f;
        public float dashSpeed = 9.0f;
        //public float sprintSpeed = 50.0f;
    }
    private MoveSettings moveSettings;
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
        //enemies=GameObject.FindGameObjectsWithTag("Enemy").ToList();
        moveSettings = new MoveSettings();
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
            isJustAvoid = true;
            velocity = Vector3.zero;
            animator.SetTrigger("Damage");
            camera3D.StartShake();
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

    public void KnockBack(int damage)
    {
        //ただダメージUIをどこでもよいので表示したい用の処理
        if (state != MyState.Dead)
        {
            animator.SetTrigger("KnockBack");
            velocity = new Vector3(0f, velocity.y, 0f);
            isJustAvoid = true;
            move = Vector3.zero;
            state = MyState.Damage;
            var damageEffectIns = Instantiate<GameObject>(damageEffect, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.identity);
            Destroy(damageEffectIns, 1f);
            myStatus.SetHp(myStatus.GetHp() - damage);
        }
        if (myStatus.GetHp() <= 0)
        {
            Dead();
        }
    }

    public void SetState(MyState tempState)
    {
        if(tempState == MyState.Normal)
        {
            state = MyState.Normal;
            isJustAvoid = false;
        }else if(tempState==MyState.Attack)  //攻撃行動
        {
            velocity = Vector3.zero;
            state = MyState.Attack;
            move = Vector3.zero;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            
            if (isJustAvoidAttack)
            {
                isJustAvoidMove = true;
                Debug.Log("距離を詰める");
            }
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
            camera3D.StartShake();
            isJustAvoid = true;
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
    //ここまでCompotに見てもらったところ

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
        if (run)
        {
            rb.velocity = moveForward * moveSettings.dashSpeed * move.magnitude + new Vector3(0, rb.velocity.y, 0);
        }
        if (move.magnitude > 0)
        { 
            if(!run)
            {
                rb.velocity = moveForward * moveSettings.moveSpeed * move.magnitude + new Vector3(0, rb.velocity.y, 0);
            }
        }
        else
        {
            rb.velocity= new Vector3(0, rb.velocity.y, 0);
            animator.SetFloat("Speed", 0f);
        }
       

        //入力された方向に応じてプレイヤーの向きを変える処理
        if (move.magnitude > 0
                   && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
                   && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
                   && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        {
            if (!run)
            {
                animator.SetFloat("Speed", rb.velocity.magnitude);
            }
            //入力している方向に回避するための処理
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
        if (transform.position.y < 0&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            rb.useGravity = false;
            
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
        if (context.started && !animator.IsInTransition(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && changeequipscript.GetEquipment() >= 0)
        {
            SetState(MyState.Attack);
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
    public void SetEnemyScript(MoveEnemyScript moveEnemyScripts)
    {
        moveEnemyScript = moveEnemyScripts;
    }

    public MoveEnemyScript GetEnemyScript()
    {
        return moveEnemyScript;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public bool GetIsJustAvoid()
    {
        return isJustAvoid;
    }

    void Playerhauding()
    {
        if (Input.GetKeyDown(KeyCode.F) && transform.position.y < 0/*&&!isJump*//*&& !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")*/ || Input.GetKeyDown("joystick button 3") && transform.position.y < 0)
        {
            animator.SetBool("Jump", true);
            //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(new Vector3(rb.velocity.x, /*rb.velocity.y + */jumpPower, rb.velocity.z),ForceMode.VelocityChange);
            //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpPower, rb.velocity.z);
            isJump = true;
            Debug.Log("ジャンプ");
            animator.applyRootMotion = false;
        }
        else
        {

            isJump = false;
            animator.SetBool("Jump", false);
        }
       
        if (transform.position.y <= 0 && !isJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            animator.applyRootMotion = true;
        }

        if (mov/*&&!isJustAvoidMove*/)
        {
            Move();
        }

        if (troll == null)
        {
            gameclearUI.SetActive(true);
        }
        UpdateEnemyPositions();
        nearestEnemyScript = FindNearestEnemyScript();
       
        //if (nearestEnemyScript != null)
        //{
        //    avoidCaunter = nearestEnemyScript.GetJustAvoidCaunter();
        //}
        characterController.Move(rb.velocity * Time.deltaTime);
        if (!isJustAvoidAttack)
        {
            //characterController.Move(rb.velocity * Time.deltaTime);
            //transform.position = new Vector3(1000, 0, 100);
        }

        //if (nearestEnemyScript != null && nearestEnemyScript.GetJustAvoidCaunter() >= 30 && nearestEnemyScript.GetJustAvoidCaunter() <= 40 && !isJustAvoid && avoid)
        //{
        //    StartCoroutine("JustAvoidCoroutine");
        //}

        //走っていない時
        if (!run)
        {
            //移動キーを押した
            if ((Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")))
            {
                //最初に1回押していない時は押した事にする
                if(!push)
                {
                    push = true;
                    // 最初に移動キーを押した時にその方向ベクトルを取得
                    direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                    nowTime = 0f;
                }
                else  //2回目のボタンだったら1→2までの制限時間内だったら走る
                {
                    //2回目に移動キーを押した時の方向ベクトルを取得
                    var nowDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                    //確認の為,最初に押した方向と2回目に押した方向の角度をコンソールに出力
                    Debug.Log(Vector2.Angle(nowDirection, direction));

                    //押した方向がリミットの角度を超えていない かつ 制限時間内に移動キーが押されていれば走る
                    if (Vector2.Angle(nowDirection, direction) < limitAngle
                        && nowTime <= nextButtonDownTime)
                    {
                        run = true;
                        animator.SetBool("Run", true);
                    }
                }
            }
            if(Input.GetKeyDown("joystick button 4"))
            {
                run = true;
                
            }
        }
        //走っているときにキーを押すのをやめたら走るのをやめる
        else
        {
            animator.SetBool("Run", true);
            if (/*!Input.GetButton("Horizontal") && !Input.GetButton("Vertical") || rb.velocity.magnitude<=0*/move.x==0&&move.z==0)
            {
                run = false;
                push = false;
                animator.SetBool("Run", false);
            }
        }
        //最初の移動キーを押していれば時間計測
        if(push)
        {
            //時間計測
            nowTime += Time.deltaTime;

            if(nowTime>nextButtonDownTime)
            {
                push = false;
            }
        }
        //MoveTowardsClosestEnemy(this.transform);
        //if (isJustAvoidMove)
        //{
        //    MoveTowardsClosestEnemy();
        //    //徐々に移動する
        //    //transform.position = Vector3.LerpUnclamped(this.transform.position, new Vector3(debugposition.x,transform.position.y,debugposition.z), Time.deltaTime * 1f);
            
        //}
    }

    public void PlayerSpeedOff()
    {
        rb.velocity = Vector3.zero;
        move = Vector3.zero;
    }

    public void PlayerSpeedOn()
    {
        Move();
    }

    public Rigidbody SetRigitbody()
    {
        return rb;
    }

    private IEnumerator JustAvoidCoroutine()
    {
        Debug.Log("ジャスト回避中");
        Time.timeScale = 0.2f;
        isJustAvoidAttack = true;
        //MoveTowardsClosestEnemy(this.transform);
        yield return new WaitForSecondsRealtime(5.0f);
        isJustAvoid = false;
        isJustAvoidAttack = false;
        isJustAvoidMove = false;
        Time.timeScale = 1;
    }

    void UpdateEnemyPositions()
    {
        enemyPositions.Clear();
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys)
        {
            if (enemy != null)
            {
                enemyPositions.Add(enemy.transform.position);
            }
        }
        Debug.Log("敵の情報を格納");
    }

   

    void MoveTowardsClosestEnemy()
    {
        if (enemyPositions.Count == 0) return;

        //最も近い敵の座標を取得
        Vector3 closestEnemyPos = enemyPositions[0];
        float minDistance = Vector3.Distance(transform.position, closestEnemyPos);
        foreach (Vector3 enemyPos in enemyPositions)
        {
            float distance = Vector3.Distance(transform.position, enemyPos);
            if(distance<minDistance)
            {
                closestEnemyPos = enemyPos;
                minDistance = distance;
            }
        }
        Debug.Log("徐々に移動する");
        debugposition = closestEnemyPos;
        //徐々に移動する
        this.transform.position = Vector3.Lerp(this.transform.position, closestEnemyPos,5f);
    }

    

    MoveEnemyScript FindNearestEnemyScript()
    {
        MoveEnemyScript[] enemyScripts = FindObjectsOfType<MoveEnemyScript>();
        MoveEnemyScript closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach(MoveEnemyScript enemyScript in enemyScripts)
        {
            float distance = Vector3.Distance(currentPosition, enemyScript.transform.position);
            if(distance<minDistance)
            {
                minDistance = distance;
                closest = enemyScript;
            }
        }
        return closest;
    }

   

    public bool IsJustAvoidAttack()
    {
        return isJustAvoidAttack;
    }

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.CompareTag("Field"))
       {
            //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            isJump = false;
            rb.useGravity = false;
        }
    }



}
