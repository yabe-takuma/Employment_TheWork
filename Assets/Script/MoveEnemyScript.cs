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
    [SerializeField]
    private int randam;

    [SerializeField]
    private float distance;

    [SerializeField]
    private TrollScript trollScript;

    [SerializeField]
    private PlayerScript playerscript;

    [SerializeField]
    private int collisiontimer;

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
        //SetState(EnemyState.Walk);
        handCollider = GetComponentInChildren<SphereCollider>();
        playerscript= GameObject.Find("Character_Female_Hotel Owner").GetComponent<PlayerScript>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetState(EnemyState.Wait);
    }

    // Update is called once per frame
    void Update()
    {
        if (trollScript.GetState() != TrollScript.TrollState.Dead)
        {
            //見回りまたはキャラクターを追いかける状態
            if (state == EnemyState.Walk || state == EnemyState.Chase)
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
                    //if (enemyController.isGrounded)
                    //{
                    //    velocity = Vector3.zero;
                    //    animator.SetFloat("Speed", 2.0f);
                    //    direction = (setPosition.GetDestination() - transform.position).normalized;
                    //    transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));
                    //    velocity = direction * walkSpeed;
                    //}

                    if (state == EnemyState.Walk)
                    {

                        if (navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
                        {
                            //目的地に到着したかどうかの判定
                            if (navMeshAgent.remainingDistance < 0.1f)
                            {
                                Debug.Log("目的地に着いた");
                                SetState(EnemyState.Wait);
                                animator.SetFloat("Speed", 0.0f);
                            }
                        }
                    }
                    else if (state == EnemyState.Chase)
                    {
                        //攻撃する距離だったら攻撃
                        if (navMeshAgent.remainingDistance<1.2f)
                        {
                            randam = Random.Range(1, 10);
                            SetState(EnemyState.Attack);

                        }
                        distance = navMeshAgent.remainingDistance;
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
            else if(state==EnemyState.Attack)
            {
                //プレイヤーの方向を取得
                var playerDirection = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) - transform.position;
                //敵の向きをプレイヤーの向きに少しづつ変える
                var dir = Vector3.RotateTowards(transform.forward, playerDirection, rotateSpeed * Time.deltaTime, 0f);
                //算出した方向の角度を敵の角度に設定
                transform.rotation = Quaternion.LookRotation(dir);
            }
            velocity.y += Physics.gravity.y * Time.deltaTime;
           
        }
       
    }


    //敵キャラクターの状態変更メソッド
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        state = tempState;
        velocity = Vector3.zero;
        if (tempState==EnemyState.Walk)
        {
            arrived = false;
            elapsedTime = 0f;
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
            //追いかける対象をセット
            playerTransform = targetObj;
            navMeshAgent.SetDestination(playerTransform.position);
            navMeshAgent.isStopped = false;
        } 
        else if(tempState ==EnemyState.Wait)
        {
            elapsedTime = 0f;
            arrived = true;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
        else if(tempState ==EnemyState.Attack)
        {
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack", true);
            randam = 0;
            navMeshAgent.isStopped = true;
        }
        else if(tempState == EnemyState.Freeze)
        {
            elapsedTime = 0f;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack", false);
            animator.SetBool("Attack2", false);
            animator.SetBool("Attack3", false);
        }
        else if(tempState==EnemyState.Damage)
        {
            velocity = Vector3.zero;
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");
            animator.SetTrigger("Damage");
            navMeshAgent.isStopped = true;
        }
        else if(tempState == EnemyState.Dead)
        {
            animator.SetTrigger("Dead");
            Destroy(this.gameObject, 3f);
            velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
        else if(tempState ==EnemyState.Attack2)
        {
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack2", true);
            randam = 0;
            navMeshAgent.isStopped = true;
        }
        else if(tempState == EnemyState.Attack3)
        {
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack3", true);
            randam = 0;
            navMeshAgent.isStopped = true;
        }
    }
    //敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return state;
    }

    public void TakeDamage(int damage,Vector3 attackedPlace)
    {
        SetState(EnemyState.Damage);
        handCollider.enabled = false;
        var damageEffectIns = Instantiate<GameObject>(damageEffect);
        damageEffectIns.transform.position = attackedPlace;
        Destroy(damageEffectIns, 1f);
        enemyStatus.SetHp(enemyStatus.GetHp() - damage);
        if(enemyStatus.GetHp()<=0)
        {
            Dead();
        }
    }

    void Dead()
    {
        SetState(EnemyState.Dead);
        playerscript.DeadCaunter(1);
    }

    public void SetDamageEffect(GameObject gameobject)
    {
        damageEffect = gameobject;
    }

    public void SetTrollScript(TrollScript trollscript)
    {
        trollScript = trollscript;
    }

    private void OnTriggerStay(Collider other)
    {
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

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Collision")
        {
            collisiontimer = 0;
        }
    }

}
