using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.AI;
using static MoveEnemyScript;
using UnityEngine.Playables;

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
        wave,
        continuous,
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
    [SerializeField]
    private Vector3 destination;
    //移動範囲
    [SerializeField]
    private float movementRange = 20f;
    //移動速度
    [SerializeField]
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

    private SetPosition1 setposition1;

    //壁との接触を判定するレイを飛ばす場所
    [SerializeField]
    private Transform rayTransform;
    //レイを飛ばす距離
    [SerializeField]
    private float rayDistance = 3f;
    //最初に壁に衝突してから経過時間
    [SerializeField]
    private float elapsedCollisionWall = Mathf.Infinity;
    //最初に壁に衝突してから次に判定するまでの時間
    [SerializeField]
    private float avoidanceTimeCollisionWall = 5f;

    //ステージの端っこに行ってしまった時の処理
    [SerializeField]
    private int collisiontimer;
    [SerializeField]
    private float dis;
   
    //何の攻撃をしているかを他のスクリプトに分かるようにする処理
    [SerializeField]
    private bool Isshockwave, Isinstallation,Isexplocion,Iscontinuous, isWave;

    [SerializeField]
    private bool isabnormal;  //状態異常になったかのトリガー
    [SerializeField]
    private int abnormalcounter;  //状態異常になっている時間
    [SerializeField]
    private GameObject fireeffect;  //炎のエフェクトが格納された変数
    private GameObject fireEffectIns;

    //予備動作
    [SerializeField]
    private PlayableDirector[] timeline;

    [SerializeField]
    private ReceiveAttackEventScript receiveAttackEventScript;

    private AnimatorStateInfo stateInfo;

   
    private const int maxloop = 3;

    private bool isstarttimeline;

    private Vector3 lastPos;
    private float stuckTimer;
    [SerializeField]
    private HitStopScript hitStopScript;
    [SerializeField]
    private Animator playeranimator;
   

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        defaultPos = transform.position;
        setposition1 = GetComponent<SetPosition1>();
        SetRandomDestination();
        timeline[0].Stop();
        timeline[1].Stop();
       
        animator.SetTrigger("ShockwaveAttack");
       // Time.timeScale = 0;
       // animator.updateMode = AnimatorUpdateMode.UnscaledTime;
      
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
        else if(trollState == TrollState.chase && Isexplocion==false)
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
        else if(trollState == TrollState.installation)
        {
            Debug.Log("設置物配置");
        }
        else if(trollState==TrollState.explocion)
        {
            Explocion();
            Debug.Log("爆発中");
        }
        else if(trollState==TrollState.wave)
        {
            WaveAttack();
            Debug.Log("波発生");
        }
        else if(trollState==TrollState.continuous)
        {
            ContinuousAttack();
            Debug.Log("連続攻撃5");
        }
        TrollUpdate();

  
    }

    //目的地を設定する
    void SetRandomDestination()
    {
        //最初の位置から有効範囲のランダム位置を取得

        var randomPos = defaultPos + Random.insideUnitSphere * movementRange;
        var ray = new Ray(randomPos + Vector3.up * 10f, Vector3.down);
        RaycastHit hit;
        //目的地が地面になるように再設定
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Field")) &&
            Physics.OverlapSphere(hit.point, 1.0f, LayerMask.GetMask("Tree")).Length == 0)
        {
            destination = hit.point;
        }
        else
        {
            SetRandomDestination(); // 再試行
        }

    }

    //状態変更メソッド
    public void SetState(TrollState tmpState,Transform playerTransform =null)
    {
        trollState = tmpState;
        if(trollState == TrollState.idle)
        {
            animator.SetFloat("WalkSpeed", 0f);
            animator.SetBool("Chase", false);
            animator.ResetTrigger("WaveAttack");
            SetRandomDestination();
          
            Isshockwave = false;
            Isinstallation = false;
            Isexplocion = false;
            Iscontinuous = false;
            isWave = false;
            Debug.Log("アイドル");
        }
        else if(trollState == TrollState.patrol)
        {
            Debug.Log("パトロール");
            SetRandomDestination();
        }
        else if(trollState == TrollState.attack)
        {
            attackTargetTransform = playerTransform;
            attackTargetPos = attackTargetTransform.position;
            velocity = new Vector3(0f, velocity.y, 0f);
            animator.SetTrigger("Attack");
            animator.SetBool("Chase", false);
           
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
            Iscontinuous = false;
            isWave = false;
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
            timeline[1].Play();
            timeline[1].playableGraph.GetRootPlayable(0).SetSpeed(0.5f);
            animator.SetBool("Chase", false);
            Isshockwave = false;
            Isinstallation = true;
            Isexplocion = false;
            Iscontinuous = false;
            isWave = false;
            Debug.Log("設置物配置攻撃");
        }
        else if (trollState == TrollState.explocion)
        {
           
            animator.ResetTrigger("ShockwaveAttack");
            animator.ResetTrigger("ContinuousAttack");
            velocity = new Vector3(0f, velocity.y, 0f);
            animator.SetTrigger("Explocion");
            animator.SetBool("Chase", false);
            Isshockwave = false;
            Isinstallation = false;
            Isexplocion = true;
            Iscontinuous = false;
            isWave = false;
            Debug.Log("爆発攻撃");
        }
        else if(trollState==TrollState.wave)
        {
            velocity = new Vector3(0f, velocity.y, 0f);
            animator.SetBool("Wave",true);
            animator.SetBool("Chase", false);
            Isshockwave = false;
            Isinstallation = false;
            Isexplocion = false;
            Iscontinuous = false;
            isWave = true;
            Debug.Log("波攻撃");
        }

        else if (trollState == TrollState.continuous && trollState != TrollState.explocion)
        {
            attackTargetTransform = playerTransform;
            attackTargetPos = attackTargetTransform.position;
            velocity = new Vector3(0f, velocity.y, 0f);
            animator.SetBool("Wave", true);
            animator.SetBool("Chase", false);
            Isshockwave = false;
            Isinstallation = false;
            Isexplocion = false;
            Iscontinuous = true;
            isWave = false;
            Debug.Log("波攻撃");
        }

        
        else if(trollState == TrollState.Dead)
        {
            animator.SetTrigger("Dead");
            Destroy(this.gameObject, 2f);
            velocity = Vector3.zero;
            timeline[1].Pause();
        }
        //レイを視覚化して表示
        Debug.DrawLine(rayTransform.position, rayTransform.position + rayTransform.forward * rayDistance, Color.red);
        Debug.DrawLine(rayTransform.position, rayTransform.position + (rayTransform.forward + rayTransform.right).normalized * rayDistance, Color.blue);
        Debug.DrawLine(rayTransform.position, rayTransform.position + (rayTransform.forward - rayTransform.right).normalized * rayDistance, Color.yellow);
        //ボスキャラから目的地までのレイを表示
        Debug.DrawLine(rayTransform.position, destination, Color.green);
    }

    //状態取得メソッド
    public TrollState GetState()
    {
        return trollState;
    }

    //Idle状態の時の処理
    private void Idle()
    {
       // navMeshAgent.isStopped = true;
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
           
        }

        //目的地に着いたらidle状態にする
        if (Vector3.Distance(transform.position, destination) < 2.0f)
        {
            SetState(TrollState.idle);

        }

        Vector3 rayOrigin = transform.position + Vector3.up * 0.3f;
        Vector3[] directions = new Vector3[]
        {
            rayTransform.forward,
            (rayTransform.forward + rayTransform.right).normalized,
            (rayTransform.forward - rayTransform.right).normalized,
            (rayTransform.forward + Vector3.down * 0.5f).normalized  // 低めの角度
        };





        if (elapsedCollisionWall >= avoidanceTimeCollisionWall)
        {
            foreach (var dir in directions)
            {
                if (Physics.Linecast(rayTransform.position, rayTransform.position + dir * rayDistance, LayerMask.GetMask("Tree")))
                {
                   
                    AvoidObstacleImmediately();

                    // すぐにループを抜けて二重に回避しないようにする
                    break;
                }
            }
        }
        //一旦目的地を再設定したら一定の回避時間を設ける
        elapsedCollisionWall += Time.deltaTime;
        if (elapsedCollisionWall >= avoidanceTimeCollisionWall)
        {
            elapsedCollisionWall = avoidanceTimeCollisionWall;
        }
     
    }
    //Chase状態の時の処理
    private void Chase()
    {
        //目的地を毎回設定し直す
        destination = attackTargetTransform.position;
        //追いかける処理
        if (characterController.isGrounded)
        {
            velocity = Vector3.zero;
            //追いかける時はキャラクターの向きに回転して進ませる
            var direction = (destination - transform.position).normalized;
            var targetRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(destination - transform.position), Time.deltaTime * rotateSpeed);
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

        ////Attackアニメーションが終了したらIdle状態にする
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShockwaveAttack")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            SetState(TrollState.idle);
            Debug.Log("トロル強化攻撃");
        }
    }

    private void WaveAttack()
    {
       
        velocity = Vector3.zero;

        //追いかける時はキャラクターの向きに回転して進ませる
        var targetRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(attackTargetPos - transform.position), Time.deltaTime * 2f);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetRot.eulerAngles.y, transform.eulerAngles.z);

        Debug.Log("突進攻撃");

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WaveAttack")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            SetState(TrollState.idle);
        }
        
    }

  

    private void Explocion()
    {
        //Attackアニメーションが終了したらIdle状態にする
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExplocionAttack")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            SetState(TrollState.idle);
            Isexplocion = false;
            Debug.Log("爆発発生");
        }
    }

  

    private void ContinuousAttack()
    {

        //攻撃状態になった時のキャラクターの向きを計算し、徐々にそちらの向きに回転させる
        var targetRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(attackTargetPos - transform.position), Time.deltaTime * 2f);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetRot.eulerAngles.y, transform.eulerAngles.z);

        //Attackアニメーションが終了したらIdle状態にする
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ContinuousAttack")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 3)
        {
            SetState(TrollState.idle);
            Debug.Log("連続波攻撃");
        }
    }

    public void TakeDamage(int damage,Vector3 attackedPlace)
    {
        //WeakUIをインスタンス化。登場位置はコライダの中心からカメラの方向に少し寄せた位置
        trollStatus.SetHp(trollStatus.GetHp() - damage);
        if (playeranimator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        {
            HitStopScript.instance.StartHitStop(0.3f);
        }
        if (trollStatus.GetHp()<=0)
        {
            Dead();
        }
    }

    void Dead()
    {
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

    public bool GetContinuous()
    {
        return Iscontinuous;
    }

    public bool GetIsWave()
    {
        return isWave;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

   

   

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetVelocity(Vector3 velo)
    {
        velocity = velo;
    }

    public void AbnormalCondition()
    {
        isabnormal = true;
        abnormalcounter = 0;
    }

    public bool GetIsAbnormal()
    {
        return isabnormal;
    }

    public void DestroyFire()
    {
        Destroy(fireEffectIns);
    }

    public void ContinuousOff()
    {
        isstarttimeline = false;
    }

    public void ContinuousOn()
    {
        isstarttimeline = true;
    }

    void TrollUpdate()
    {
        if (trollStatus.GetHp() <= 0&&this!=null)
        {
            Time.timeScale = 0.2f;
        }
        //敵が炎上している時
        if (isabnormal)
        {
            abnormalcounter++;
            if (!fireEffectIns)
            {
                fireEffectIns = Instantiate<GameObject>(fireeffect);
            }
            fireEffectIns.transform.position = transform.position;
        }

        if (abnormalcounter > 1000)
        {
            isabnormal = false;
            abnormalcounter = 0;
            Destroy(fireEffectIns);
        }
        //移動処理
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);


        if (timeline[1].time >= timeline[1].duration)
        {
            Isinstallation = false;
        }
        if (receiveAttackEventScript.GetIsExplocion())
        {
            Isexplocion = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShockwaveAttack") ||
           animator.GetCurrentAnimatorStateInfo(0).IsName("WaveAttack") ||
           isstarttimeline)
        {
            velocity = new Vector3(0f, velocity.y, 0f);
        }

        if (Vector3.Distance(transform.position, lastPos) < 0.1f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > 2f)
            {
                Debug.Log("スタック検出 → 再目的地");
                SetRandomDestination();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPos = transform.position;

        // 万が一現在地が木と重なってしまっていたら、目的地を強制リセット
        if (Physics.CheckSphere(transform.position + Vector3.up * 0.2f, 0.4f, LayerMask.GetMask("Tree")))
        {
            Debug.Log("木にスタック → 再パトロール");
            SetRandomDestination();
            velocity = transform.forward * walkSpeed; // すぐ動けるように
            SetState(TrollState.patrol);              // 強制的に動作状態に戻す
            velocity = transform.forward * walkSpeed; // ← 動作直後の力を与える

        }


    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Tree") &&
            hit.collider is BoxCollider)
        {
            float angle = Random.Range(90f, 180f);
            transform.Rotate(0, angle, 0);
            SetRandomDestination();
            velocity = transform.forward * walkSpeed;


        }
    }



    void AvoidObstacleImmediately()
    {
        float angle = Random.Range(90f, 180f);
        transform.Rotate(0, angle, 0);
        SetRandomDestination();
        velocity = transform.forward * walkSpeed;
        elapsedCollisionWall = 0f; // 次回までの猶予タイマーリセット
    }

}

