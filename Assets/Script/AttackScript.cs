using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    //敵の攻撃力のための変数やプレイヤーにアニメーションをするために変数を用意しています。
    [SerializeField]
    private EnemyStatus enemyStatus;

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
