using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    //敵の攻撃力のための変数やプレイヤーにアニメーションをするために変数を用意しています。
    [Header("敵のステータス専用のスクリプト")]
    [SerializeField]
    private EnemyStatus enemyStatus;

    [Header("プレイヤーの行動全般を処理するスクリプト")]
    [SerializeField]
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyStatus = transform.root.GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //雑魚敵の攻撃がプレイヤーに当たった時の処理
    private void OnTriggerEnter(Collider other)
    {
        //この処理はプレイヤーが死亡状態や回避状態ではないときにダメージを与える処理です。
        if (gameObject.CompareTag("Z_Arm") && other.tag == "Player"&&playerScript.GetAvoid()==false&&playerScript.GetState()!=PlayerScript.MyState.Dead)
        {
            Debug.Log("当たり");
            other.GetComponent<PlayerScript>().TakeDamage(transform.root, other.ClosestPoint(transform.position), enemyStatus.GetAttackPower());

        }
       
    }
    //プレイヤーの情報を他のスクリプトに渡す関数
    public void SetPlayer(PlayerScript player)
    {
        playerScript = player;
        Debug.Log("プレイヤーの情報を渡した。");
    }

}
