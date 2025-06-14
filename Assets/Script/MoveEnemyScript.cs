using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static TrollScript;

public class MoveEnemyScript : MonoBehaviour
{

    public enum EnemyState
    {
        Walk,
        Wait,
        Chase,
        Attack,
        Freeze,
        Damage,
        KnockBack,
        Dead,
        Attack2,
        Attack3
    };

    private CharacterController enemyController;
    private Animator animator;

    //目的地
    //private Vector3 destination;
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private float rotateSpeed=45f;
    [SerializeField]
    private float walkSpeed = 1.0f;
    //速度
    [SerializeField]
    private Vector3 velocity;
    //移動方向
    private Vector3 direction;
    //到着フラグ
    [SerializeField]
    private bool arrived;
    //スタート位置
    private Vector3 startPosition;



    //SetPositionスプリクト
    private SetPosition setPosition;
    //待ち時間
    [SerializeField]
    private float waitTime = 5f;
    //経過時間
    [SerializeField]
    private float elapsedTime;
    //敵の状態
    [SerializeField]
    private EnemyState state;
    //プレイヤーTransform
    private Transform playerTransform;
    // 攻撃した後のフリーズ時間
    [SerializeField]
    private float freezeTime = 0.5f;
    //攻撃を受けた時のエフェクト
    [SerializeField]
    private GameObject damageEffect;
    [SerializeField]
    private SphereCollider handCollider;
    //敵のステータス管理スプリクト
    [SerializeField]
    private EnemyStatus enemyStatus;
    //敵が別の攻撃をする変数(まだ未実装)
    [SerializeField]
    private int randam;

    [SerializeField]
    private TrollScript trollScript;  //ボス

    [SerializeField]
    private PlayerScript playerscript;  //プレイヤー

    [SerializeField]
    private int collisiontimer;  //当たっている時間
    [SerializeField]
    private GameObject axe;  //斧
    private MyItemScript myItemScript;  //斧が生成されるタイミングでスクリプトを代入したいため
    private AttackAxe attackAxe;  //一つ上と同じ理由

    [SerializeField]
    private bool isabnormal;  //炎上フラグ
    [SerializeField]
    private int abnormalcounter;  //状態異常になっている時間
    [SerializeField]
    private GameObject fireeffect;  //炎のエフェクトが格納された変数
    private GameObject fireEffectIns;

    private bool isDead;
    //敵に武器を分けるために必要な変数
    [SerializeField]
    private ProcessEnemyAnimEventScript processEnemyAnimEventScript;

    //ジャスト回避出来る時間
    [SerializeField]
    private int justAvoidCaunter;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        setPosition = GetComponent<SetPosition>();
        //ランダム位置の作成と設定
        setPosition.CreateRandomPosition();
        velocity = Vector3.zero;
        arrived = false;
        elapsedTime = 0f;
        handCollider = GetComponentInChildren<SphereCollider>();
        processEnemyAnimEventScript = transform.root.GetComponent<ProcessEnemyAnimEventScript>();
        playerscript= GameObject.Find("Character_Female_Hotel Owner").GetComponent<PlayerScript>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetState(EnemyState.Wait);
        isDead = false;
        justAvoidCaunter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Enemyhauding();
    }


    //敵キャラクターの状態変更メソッド
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        state = tempState;
        velocity = Vector3.zero;
        if (tempState==EnemyState.Walk && state != EnemyState.Dead)
        {
            arrived = false;
            elapsedTime = 0f;
            //攻撃時以外はジャスト回避出来る時間をリセットする
            //justAvoidCaunter = 0;
            setPosition.CreateRandomPosition();
            if (navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
            {
                navMeshAgent.SetDestination(setPosition.GetDestination());
                navMeshAgent.isStopped = false;
            }
        }
        else if(tempState == EnemyState.Chase)
        {
            //待機状態から追いかける場合もあるのでoff
            arrived = false;
            //攻撃時以外はジャスト回避出来る時間をリセットする
            //justAvoidCaunter = 0;
            //追いかける対象をセット
            playerTransform = targetObj;
            navMeshAgent.SetDestination(playerTransform.position);
            navMeshAgent.isStopped = false;
        } 
        else if(tempState ==EnemyState.Wait)
        {
            //待っている時間をリセット
            elapsedTime = 0f;
            //待機状態にする
            arrived = true;
            //攻撃時以外はジャスト回避出来る時間をリセットする
            justAvoidCaunter = 0;
            //スピードやアニメーションもゼロにする
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
        else if(tempState ==EnemyState.Attack)
        {
            //攻撃時は歩かないようにする
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack", true);
           
            navMeshAgent.isStopped = true;
        }
        else if(tempState == EnemyState.Freeze)
        {
            //フリーズ時も歩かないようにする
            elapsedTime = 0f;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack", false);
            animator.SetBool("Attack2", false);
            animator.SetBool("Attack3", false);
        }
        else if(tempState==EnemyState.Damage)
        {
            //ダメージ時も歩かないようにするさらに攻撃もやめる
            velocity = Vector3.zero;
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");
            if (!playerscript.IsJustAvoidAttack())
            {
                HitStopScript.instance.StartHitStop(0.08f);
            }
            animator.SetTrigger("Damage");
            navMeshAgent.isStopped = true;
        }
        else if(tempState==EnemyState.KnockBack)
        {
            //ダメージ時も歩かないようにするさらに攻撃もやめる
            velocity = Vector3.zero;
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");
            animator.SetTrigger("KnockBack");
            navMeshAgent.isStopped = true;
        }
        else if(tempState == EnemyState.Dead)
        {
            //倒されるときアニメーションをして消滅をする
            animator.SetTrigger("Dead");
            Destroy(this.gameObject, 3f);
            Destroy(fireEffectIns, 3f);
            velocity = Vector3.zero;
            navMeshAgent.speed = 0f;
            navMeshAgent.isStopped = true;
        }
        //敵が別の攻撃をする処理(まだ未実装)
        else if (tempState ==EnemyState.Attack2)
        {
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            if (processEnemyAnimEventScript.GetWeaponCaunter() == 1)
            {
                animator.SetBool("Attack2", true);
            }
            else if(processEnemyAnimEventScript.GetWeaponCaunter()==0)
            {
                animator.SetBool("Attack3", true);
            }
            navMeshAgent.isStopped = true;
        }
        else if(tempState == EnemyState.Attack3)
        {
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack3", true);
            navMeshAgent.isStopped = true;
        }
        //----------------------//
    }
    //敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return state;
    }

    public void TakeDamage(int damage,Vector3 attackedPlace)
    {
        if (enemyStatus.GetHp() >= 0.0f)
        {
            //ダメージを受ける時攻撃用のコライダーを非表示にし、エフェクトを生成
            //さらに体力を減らす処理
            SetState(EnemyState.Damage);
            handCollider.enabled = false;
            var damageEffectIns = Instantiate<GameObject>(damageEffect);
            damageEffectIns.transform.position = attackedPlace;
            Destroy(damageEffectIns, 1f);
            enemyStatus.SetHp(enemyStatus.GetHp() - damage);
            
            
        }
        ////体力が0になると倒される処理
        if (enemyStatus.GetHp() <= 0.0f && !isDead)
        {
            Dead();
            isDead = true;
        }
    }

    public void KnockBackDamage(int damage, Vector3 attackedPlace)
    {
        if (enemyStatus.GetHp() >= 0.0f)
        {
            //ダメージを受ける時攻撃用のコライダーを非表示にし、エフェクトを生成
            //さらに体力を減らす処理
            SetState(EnemyState.KnockBack);
            handCollider.enabled = false;
            var damageEffectIns = Instantiate<GameObject>(damageEffect);
            damageEffectIns.transform.position = attackedPlace;
            Destroy(damageEffectIns, 1f);
            enemyStatus.SetHp(enemyStatus.GetHp() - damage);
            //ノックバック
            StartCoroutine(KnockBackCoroutine(attackedPlace));
        }
        ////体力が0になると倒される処理
        if (enemyStatus.GetHp() <= 0.0f && !isDead)
        {
            Dead();
            isDead = true;
        }
    }

    void Dead()
    {
        //倒す行動とプレイヤーに何体倒したか分かるようにする
        SetState(EnemyState.Dead);
        playerscript.DeadCaunter(1);
        if (playerscript.SetDeadCaunter() == 1)
        {
            GameObject Axe=Instantiate<GameObject>(axe, transform.position+Vector3.up, Quaternion.identity);
            attackAxe = Axe.GetComponent<AttackAxe>();
            attackAxe.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            attackAxe.SetMyItem(myItemScript);
        }
    }

    public void AbnormalCondition()
    {
        isabnormal = true;
        abnormalcounter = 0;
    }

    //他のスクリプトからもらってきたデータを格納するための関数
    public void SetDamageEffect(GameObject gameobject)
    {
        damageEffect = gameobject;
    }
    //他のスクリプトからもらってきたデータを格納するための関数
    public void SetTrollScript(TrollScript trollscript)
    {
        trollScript = trollscript;
    }
    //他のスクリプトからデータを参照するための関数
    public int GetJustAvoidCaunter()
    {
        return justAvoidCaunter;
    }

    private void OnTriggerStay(Collider other)
    {
        //ステージ外に行ったらまたとどまりまた違う目的地に行ってもらうための処理
        if (other.tag == "Collision" && collisiontimer < 10 && state != EnemyState.Wait)
        {
            state = EnemyState.Wait;
            collisiontimer++;
        }
        else if (other.tag == "Collision" && collisiontimer > 10)
        {
            state = EnemyState.Walk;

        }
    }
    //ステージ外じゃなかったら変数を0にして普通通りの行動をする処理
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Collision")
        {
            collisiontimer = 0;
        }
    }

    public void SetAxeSword(GameObject gameObject)
    {
        axe = gameObject;
    }

    public void SetMyItem(MyItemScript myitemscript)
    {
        myItemScript = myitemscript;   
    }

    public void SetFireEffect(GameObject gameObject)
    {
        fireeffect = gameObject;
    }

    public bool GetIsAbnormal()
    {
        return isabnormal;
    }

    public void DestroyFire()
    {
        Destroy(fireEffectIns);
        isabnormal = false;
        abnormalcounter = 0;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    void Enemyhauding()
    {
        if (state != EnemyState.Dead)
        {
            //見回りまたはキャラクターを追いかける状態
            if (state == EnemyState.Walk/*&& abnormalcounter == 1*/ || state == EnemyState.Chase /*&& abnormalcounter ==1 */)
            {
                if (!arrived)
                {
                    //キャラクターを追いかける状態であればキャラクターの目的地を再設定
                    if (state == EnemyState.Chase)
                    {
                        setPosition.SetDestination(playerTransform.position);
                        navMeshAgent.SetDestination(setPosition.GetDestination());
                    }

                    animator.SetFloat("Speed", navMeshAgent.desiredVelocity.magnitude);

                    if (state == EnemyState.Walk)
                    {

                        if (navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
                        {
                            //目的地に到着したかどうかの判定
                            if (navMeshAgent.remainingDistance < 0.1f)
                            {
                                Debug.Log("目的地に到着した");
                                SetState(EnemyState.Wait);
                                animator.SetFloat("Speed", 0.0f);
                            }
                        }
                    }
                    else if (state == EnemyState.Chase)
                    {
                        //攻撃する距離だったら攻撃
                        if (navMeshAgent.remainingDistance < 1.2f)
                        {
                             SetState(EnemyState.Attack);
                        }
                    }
                }

            }
            //到着していたら一定時間待つ
            else if (state == EnemyState.Wait)
            {
                elapsedTime += Time.deltaTime;

                //待ち時間を超えたら次の目的地を設定
                if (elapsedTime > waitTime)
                {
                    SetState(EnemyState.Walk);
                }
            }
            else if (state == EnemyState.Freeze)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime > freezeTime)
                {
                    SetState(EnemyState.Walk);
                }
            }
            else if (state == EnemyState.Attack||state==EnemyState.Attack2||state==EnemyState.Attack3)
            {
                //プレイヤーの方向を取得
                var playerDirection = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) - transform.position;
                //敵の向きをプレイヤーの向きに少しづつ変える
                var dir = Vector3.RotateTowards(transform.forward, playerDirection, rotateSpeed * Time.deltaTime, 0f);
                //算出した方向の角度を敵の角度に設定
                transform.rotation = Quaternion.LookRotation(dir);

                justAvoidCaunter++;
            }
            velocity.y += Physics.gravity.y * Time.deltaTime;

        }
        //体力が0になると倒される処理
        if (enemyStatus.GetHp() <= 0 && isabnormal && !isDead)
        {
            SetState(EnemyState.Dead);
            Dead();
            isDead = true;
        }

        if (isabnormal)
        {
            abnormalcounter++;
            enemyStatus.SetHp(enemyStatus.GetHp() - 0.02f);
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
    }

    public void JustAvoidEnd()
    {
        justAvoidCaunter = 0;
    }

    private IEnumerator KnockBackCoroutine(Vector3 attackDirection)
    {
        float knockbackTime = 0.2f;  //ノックバックの継続時間
        float knockbackStrength = 2.0f; //ノックバックの初期値
        float elapsed = 0f;

        while (elapsed<knockbackTime)
        {
            enemyController.Move(attackDirection.normalized * knockbackStrength * Time.deltaTime);
            knockbackStrength *= 0.9f; //ノックバックの減衰
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

}
