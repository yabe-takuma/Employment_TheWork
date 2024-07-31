using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveEnemyScript : MonoBehaviour
{

    public enum EnemyState
    {
        Walk,
        Wait,
        Chase
    };

    private CharacterController enemyController;
    private Animator animator;

    //目的地
    //private Vector3 destination;
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
        SetState(EnemyState.Walk);
    }

    // Update is called once per frame
    void Update()
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
                }
                if (enemyController.isGrounded)
                {
                    velocity = Vector3.zero;
                    animator.SetFloat("Speed", 2.0f);
                    direction = (setPosition.GetDestination() - transform.position).normalized;
                    transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));
                    velocity = direction * walkSpeed;
                }


                //目的地に到着したかどうかの判定
                if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 1.7f)
                {
                    Debug.Log("目的地に着いた");
                    arrived = true;
                    SetState(EnemyState.Wait);
                    animator.SetFloat("Speed", 0.0f);
                }
                //到着していたら一定時間待つ

            }
           

            velocity.y += Physics.gravity.y * Time.deltaTime;
            enemyController.Move(velocity * Time.deltaTime);
        }
        if (state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;
        }
        if (state == EnemyState.Wait)
        {


            //待ち時間を超えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(EnemyState.Walk);
            }

        }
    }


    //敵キャラクターの状態変更メソッド
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        if(tempState==EnemyState.Walk)
        {
            arrived = false;
            elapsedTime = 0f;
            state = tempState;
            setPosition.CreateRandomPosition();
        }else if(tempState == EnemyState.Chase)
        {
            state = tempState;
            //待機状態から追いかける場合もあるのでoff
            arrived = false;
            //追いかける対象をセット
            playerTransform = targetObj;
        } else if(tempState ==EnemyState.Wait)
        {
            elapsedTime = 0f;
            state = tempState;
            arrived = true;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
    }
    //敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return state;
    }
}
