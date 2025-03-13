using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneretor : MonoBehaviour
{
    //プレハブ化した敵の情報
    [SerializeField]
    private GameObject enemy;
    //敵の攻撃に必要なコライダー
    [SerializeField]
    private SphereCollider spherecollider;  //敵のコライダー
    //プレハブ化した敵に渡すスクリプトたち
    [SerializeField]
    private PlayerScript playerScript;      //プレイヤー
    private AttackScript attackScript;      //敵の攻撃
    [SerializeField]
    private GameObject damageEffect;        //敵のエフェクト
    private MoveEnemyScript moveEnemyScript; //敵の行動
    [SerializeField]
    private TrollScript trollScript;         //ボスの行動
    //------------------------------//

    private Vector3[] enemytransform=new Vector3[5];  //複数の敵の座標
  

    private int enemycaunter;  //一つ一つの敵にスクリプトを渡す為の変数

    // Start is called before the first frame update
    void Start()
    {
        //雑魚敵の配置と雑魚敵の数を格納する変数の初期化
        enemytransform[0] = new Vector3(974.509f, 0.999f, 51.15654f);
        enemytransform[1] = new Vector3(904.509f, 0.999f, 51.15654f);
        enemytransform[2] = new Vector3(1015.5f, 0.999f, 51.15654f);
        enemytransform[3] = new Vector3(910f, 0.999f, 121.15654f);
        enemytransform[4] = new Vector3(1026.4f, 0.999f, 110.9f);
        enemycaunter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //プレハブ化したものにスクリプトやエフェクトなどの情報を渡す処理
        for (int i = 0; i < 5; i++)
        {
            if (enemycaunter == i)
            {
                GameObject enemys=Instantiate(enemy, enemytransform[i], Quaternion.identity);
                attackScript = enemys.GetComponentInChildren<AttackScript>();
                attackScript.SetPlayer(playerScript);
                moveEnemyScript = enemys.GetComponent<MoveEnemyScript>();
                moveEnemyScript.SetDamageEffect(damageEffect);
                moveEnemyScript.SetTrollScript(trollScript);
                playerScript.SetEnemyScript(moveEnemyScript);
                enemycaunter += 1;
            }
        }
    }

   
}
