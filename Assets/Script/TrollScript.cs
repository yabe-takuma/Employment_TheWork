using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.AI;
public class TrollScript : MonoBehaviour
{

    public enum TrollState
    {
        idle,
        patrol,
        chase,
        attack,
        shockwaveAttack,
        charge,
        Jump,
        installation,
        explocion,
        Damage,
        Dead
    }

    private CharacterController characterController;
    private Animator animator;
    //トロールが登場した最初の位置
    private Vector3 defaultPos;
    //トロールの状態
    [SerializeField]
    private TrollState trollState = TrollState.idle;
    //目的地
    private Vector3 destination;
    //移動範囲
    [SerializeField]
    private float movementRange = 20f;
    //移動速度
    private Vector3 velocity = Vector3.zero;
    //歩くスピード
    [SerializeField]
    private float walkSpeed = 0.01f;
    //追いかけるスピード
    [SerializeField]
    private float chaseSpeed = 0.6f;
    //向きを回転する速さ
    [SerializeField]
    private float rotateSpeed = 2f;
    //idle状態の経過時間
    private float elapsedTimeOfIdleState = 0f;
    //idle状態で止まる時間
    [SerializeField]
    private float timeToStayInIdle = 3f;
    //攻撃対象のTransform
    private Transform attackTargetTransform;
    //攻撃時の対象の位置
    private Vector3 attackTargetPos;
    //敵のステータス管理スプリクト
    [SerializeField]
    private TrollStatus trollStatus;
    //メイスのコライダー
    [SerializeField]
    private CapsuleCollider maceCapsuleCollider;
    [SerializeField]
    private SphereCollider maceSphereCollider;
    //エージェント
    private NavMeshAgent navMeshAgent;

    private SetPosition1 setposition1;

    [SerializeField]
    private Vector3 pos;
    [SerializeField]
    private float dis;

    //突進時間
    [SerializeField]
    private int chargetimer;
    [SerializeField]
    private bool Isshockwave, Isinstallation,Isexplocion;

    [SerializeField]
    private GameObject gameclearUI;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        defaultPos = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        setposition1 = GetComponent<SetPosition1>();
        SetRandomDestination();
        gameclearUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        //トロールの状態によって処理を変える
        if(trollState == TrollState.idle)
        {
            Idle();
        }
        else if(trollState == TrollState.patrol)
        {
            Patrol();
        }
        else if(trollState == TrollState.chase)
        {
            Chase();
        }
        else if(trollState == TrollState.attack)
        {
            Attack();
        }
        else if(trollState == TrollState.shockwaveAttack)
        {
            ShockwaveAttack();
        }
        else if(trollState == TrollState.charge)
        {
            ChargeAttack();
            chargetimer++;
            Debug.Log("突進中");
        }
        else if(trollState == TrollState.Jump)
        {
            Debug.Log("ジャンプ中");
        }
        else if(trollState == TrollState.installation)
        {

            Debug.Log("設置物配置");
        }
        else if(trollState==TrollState.explocion)
        {
            Debug.Log("爆発中");
        }
        //共通するCharacterControllerの移動処理
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        
    }

    //目的地を設定する
    void SetRandomDestination()
    {
        //最初の位置から有効範囲のランダム位置を取得

        var randomPos = defaultPos + Random.insideUnitSphere*movementRange;
        var ray = new Ray(randomPos + Vector3.up * 10f, Vector3.down);
        RaycastHit hit;
        //目的地が地面になるように再設定
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Field")))
        {
            destination = hit.point;
        }
        else
        {
            randomPos = Vector3.zero;
            destination = Vector3.zero;
        }
    }

    //状態変更メソッド
    public void SetState(TrollState tmpState,Transform playerTransform =null)
    {
        trollState = tmpState;
        if(trollState == TrollState.idle)
        {
            velocity = new Vector3(0f, velocity.y, 0f);
            animator.SetFloat("WalkSpeed", 0f);
            animator.SetBool("Chase", false);
            while (true)
            {
                if (dis < 100)
                {
                    SetRandomDestination();
                    break;
                }
            }
            chargetimer = 0;
            Isshockwave = false;
            Isinstallation = false;
            Isexplocion = false;
            Debug.Log("アイドル");
        }
        else if(trollState == TrollState.patrol)
        {
            Debug.Log("パトロール");
        }
        else if(trollState == TrollState.attack)
        {
            attackTargetTransform = playerTransform;
            attackTargetPos = attackTargetTransform.position;
            velocity = new Vector3(0f, velocity.y, 0f);
            animator.SetTrigger("Attack");
            animator.SetBool("Chase", false);
            navMeshAgent.isStopped = true;
            Debug.Log("通常攻撃");
        }
        else if(trollState == TrollState.shockwaveAttack)
        {
            attackTargetTransform = playerTransform;
            attackTargetPos = attackTargetTransform.position;
            velocity = new Vector3(0f, velocity.y, 0f);
            animator.SetTrigger("ShockwaveAttack");
            animator.SetBool("Chase", false);
            Isshockwave = true;
            Isinstallation = false;
            Isexplocion = false;
            navMeshAgent.isStopped = true;
            Debug.Log("衝撃波攻撃");
        }
        else if(trollState == TrollState.charge)
        {
            animator.SetTrigger("Charge");
            animator.SetBool("Chase", false);
            Debug.Log("突進");
        }
        else if(trollState == TrollState.chase)
        {
            animator.SetBool("Chase", true);
            attackTargetTransform = playerTransform;
            navMeshAgent.SetDestination(attackTargetTransform.position);
            navMeshAgent.isStopped = false;
            Debug.Log("チェイス");
        }
        else if(trollState == TrollState.Jump)
        {
            animator.SetTrigger("Jump");
        }
        else if(trollState == TrollState.installation)
        {
            attackTargetTransform = playerTransform;
            attackTargetPos = attackTargetTransform.position;
            velocity = new Vector3(0f, velocity.y, 0f);
            animator.SetTrigger("ShockwaveAttack");
            animator.SetBool("Chase", false);
            Isshockwave = false;
            Isinstallation = true;
            Isexplocion = false;
            navMeshAgent.isStopped = true;
            Debug.Log("設置物配置攻撃");
        }
        else if (trollState == TrollState.explocion)
        {
            //attackTargetTransform = playerTransform;
            //attackTargetPos = attackTargetTransform.position;
            velocity = new Vector3(0f, velocity.y, 0f);
            animator.SetTrigger("ShockwaveAttack");
            animator.SetBool("Chase", false);
            Isshockwave = false;
            Isinstallation = false;
            Isexplocion = true;
            navMeshAgent.isStopped = true;
            Debug.Log("爆発攻撃");
        }

        else if(trollState == TrollState.Damage)
        {
            //velocity = Vector3.zero;
            //animator.ResetTrigger("Attack");
            //animator.SetTrigger("Damage");

        }
        else if(trollState == TrollState.Dead)
        {
            animator.SetTrigger("Dead");
            Destroy(this.gameObject, 3f);
            velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
    }

    //状態取得メソッド
    public TrollState GetState()
    {
        return trollState;
    }

    //Idle状態の時の処理
    private void Idle()
    {
        elapsedTimeOfIdleState += Time.deltaTime;
        //一定時間が経過したらpatrol状態にする
        if(elapsedTimeOfIdleState >= timeToStayInIdle)
        {
            elapsedTimeOfIdleState = 0f;
            SetState(TrollState.patrol);
        }
    }
    //Patrol状態の時の処理
    private void Patrol()
    {
        //通常移動処理
        if (characterController.isGrounded)
        {
            velocity = Vector3.zero;
            //目的地の方向を計算し、向きを変えて前方に進める
            var direction = (destination - transform.position).normalized;
            animator.SetFloat("WalkSpeed", direction.magnitude);
            var targetRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(destination - transform.position), Time.deltaTime * rotateSpeed);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetRot.eulerAngles.y, transform.eulerAngles.z);
            velocity = transform.forward * walkSpeed;
            dis = Vector3.Distance(transform.position, destination);
            pos = destination;
        }
        //目的地に着いたらidle状態にする
        if (Vector3.Distance(transform.position,destination)<0.5f)
        {
            SetState(TrollState.idle);
           
        }
    }
    //Chase状態の時の処理
    private void Chase()
    {
        //目的地を毎回設定し直す
        //destination = attackTargetTransform.position;
        //追いかける処理
        if (characterController.isGrounded)
        {
            velocity = Vector3.zero;
            //追いかける時はキャラクターの向きに回転して進ませる
            var direction = (destination - transform.position).normalized;
            var targetRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(destination - transform.position),Time.deltaTime * rotateSpeed);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetRot.eulerAngles.y, transform.eulerAngles.z);
            velocity = transform.forward * chaseSpeed;
            Debug.Log("追いかける");
        }
    }
    //Attack状態の時の処理
    private void Attack()
    {
        //攻撃状態になった時のキャラクターの向きを計算し、徐々にそちらの向きに回転させる
        var targetRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(attackTargetPos - transform.position), Time.deltaTime * 2f);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetRot.eulerAngles.y, transform.eulerAngles.z);

        //Attackアニメーションが終了したらIdle状態にする
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            SetState(TrollState.idle);
            Debug.Log("トロル攻撃");
        }
    }
    //ShockwaveAttackの時の処理
    private void ShockwaveAttack()
    {
        //攻撃状態になった時のキャラクターの向きを計算し、徐々にそちらの向きに回転させる
        var targetRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(attackTargetPos - transform.position), Time.deltaTime * 2f);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetRot.eulerAngles.y, transform.eulerAngles.z);

        //Attackアニメーションが終了したらIdle状態にする
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShockwaveAttack")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            SetState(TrollState.idle);
            Debug.Log("トロル強化攻撃");
        }
    }

    private void ChargeAttack()
    {
       
        velocity = Vector3.zero;
        if (chargetimer > 0 && chargetimer <= 300)
        {
            //追いかける時はキャラクターの向きに回転して進ませる
            var direction = (destination - transform.position).normalized;
            var targetRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(destination - transform.position), Time.deltaTime * rotateSpeed);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetRot.eulerAngles.y, transform.eulerAngles.z);
            velocity = transform.forward * chaseSpeed;
            
            Debug.Log("突進攻撃");
        }
        else
        {
            SetState(TrollState.idle);
        }
    }

    private void JumpAttack()
    {
        
    }

    private void Installation()
    {
        // 攻撃状態になった時のキャラクターの向きを計算し、徐々にそちらの向きに回転させる
        var targetRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(attackTargetPos - transform.position), Time.deltaTime * 2f);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetRot.eulerAngles.y, transform.eulerAngles.z);

        //Attackアニメーションが終了したらIdle状態にする
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShockwaveAttack")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            SetState(TrollState.idle);
            Debug.Log("設置物を置いた");
        }
    }

    private void Explocion()
    {
        // 攻撃状態になった時のキャラクターの向きを計算し、徐々にそちらの向きに回転させる
        //var targetRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(attackTargetPos - transform.position), Time.deltaTime * 2f);
        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetRot.eulerAngles.y, transform.eulerAngles.z);

        //Attackアニメーションが終了したらIdle状態にする
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShockwaveAttack")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            SetState(TrollState.idle);
            Debug.Log("設置物を置いた");
        }
    }

    public void TakeDamage(int damage,Vector3 attackedPlace)
    {
        //SetState(TrollState.Damage);
        //maceCapsuleCollider.enabled = false;
        //maceSphereCollider.enabled = false;
        //WeakUIをインスタンス化。登場位置はコライダの中心からカメラの方向に少し寄せた位置
      
        trollStatus.SetHp(trollStatus.GetHp() - damage);
        navMeshAgent.isStopped = true;
        if (trollStatus.GetHp()<=0)
        {
            Dead();
        }
    }

    void Dead()
    {
        gameclearUI.SetActive(true);
        SetState(TrollState.Dead);
    }

    public bool GetShockwave()
    {
        return Isshockwave;
    }

    public bool GetInstallation()
    {
        return Isinstallation;
    }

    public bool GetExplocion()
    {
        return Isexplocion;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //TakeDamage(1)
    }

}

