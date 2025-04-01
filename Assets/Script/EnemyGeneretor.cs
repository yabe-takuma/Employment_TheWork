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

    private Vector3[] enemyposition=new Vector3[10];  //複数の敵の座標

    //宝箱の情報
    [SerializeField]
    private GameObject chest;
   
    private Vector3[] chestsposition = new Vector3[5]; //複数の宝箱の座標
   
    private Quaternion[] chestsrotation = new Quaternion[5]; //複数の宝箱の回転座標
    //斧
    [SerializeField]
    private GameObject axe;  //敵のスクリプトに斧の情報を代入するため
    [SerializeField]
    private MyItemScript myItemScript;  //敵のスクリプトに斧の情報を代入するため

    private ChestScript chestScript;  //宝箱のスクリプトに複数の宝箱の情報を代入するため
    [SerializeField]
    private List<GameObject> chestOpensUI;  //宝箱を開ける時の複数の説明文
    [SerializeField]
    private ChangeEquipScript changeEquipScript;  //宝箱を何回開けたかの情報を代入するため

    private int chestscaunter; //指定した数通りにすることで重くならないようにしている

    private int enemycaunter;  //一つ一つの敵にスクリプトを渡す為の変数と指定した数通りにすることで重くならないようにしている

    [SerializeField]
    private GameObject fireEffect;  //炎のエフェクトを敵に渡すため

    [SerializeField]
    private GameObject collisionUI;

    // Start is called before the first frame update
    void Start()
    {
        //雑魚敵の配置と雑魚敵の数を格納する変数の初期化
        enemyposition[0] = new Vector3(974.509f, 0.999f, 51.15654f);
        enemyposition[1] = new Vector3(904.509f, 0.999f, 51.15654f);
        enemyposition[2] = new Vector3(1015.5f, 0.999f, 51.15654f);
        enemyposition[3] = new Vector3(910f, 0.999f, 121.15654f);
        enemyposition[4] = new Vector3(1026.4f, 0.999f, 110.9f);
        enemyposition[5] = new Vector3(974.509f, 0.999f, 11.15654f);
        enemyposition[6] = new Vector3(904.509f, 0.999f, 11.15654f);
        enemyposition[7] = new Vector3(1015.5f, 0.999f, 11.15654f);
        enemyposition[8] = new Vector3(980f, 0.999f, 139.15654f);
        enemyposition[9] = new Vector3(1026.4f, 0.999f, 139.9f);
        enemycaunter = 0;

        //宝箱の配置と格納する変数の初期化
        chestsposition[0] = new Vector3(954.509f, 0, 51.15654f);
        chestsposition[1] = new Vector3(909.509f, 0, 51.15654f);
        chestsposition[2] = new Vector3(1045.5f, 0, 11.15654f);
        chestsposition[3] = new Vector3(904f, 0, 154.15654f);
        chestsposition[4] = new Vector3(1050.4f, 0, 155.9f);


        chestsrotation[0] = Quaternion.Euler(0, 180f, 0);
        chestsrotation[3] = Quaternion.Euler(0, -180f, 0);
        chestsrotation[4] = Quaternion.Euler(0, -180f, 0);

    }
    // Update is called once per frame
    void Update()
    {
        //プレハブ化したものにスクリプトやエフェクトなどの情報を渡す処理
        for (int i = 0; i < 5; i++)
        {
            if (chestscaunter == i)
            {
                GameObject chests = Instantiate(chest, chestsposition[i], chestsrotation[i]);
                chestScript = chests.GetComponent<ChestScript>();
                chestScript.ChestOpenUI(chestOpensUI);
                chestScript.ChestCollisionUI(collisionUI);
                chestScript.GetChangeEquipScript(changeEquipScript);
                chestscaunter += 1;
            }

        }

        for(int i=0;i<10;i++)
        {
            if (enemycaunter == i)
            {
                GameObject enemys = Instantiate(enemy, enemyposition[i], Quaternion.identity);
                attackScript = enemys.GetComponentInChildren<AttackScript>();
                attackScript.SetPlayer(playerScript);
                moveEnemyScript = enemys.GetComponent<MoveEnemyScript>();
                moveEnemyScript.SetDamageEffect(damageEffect);
                moveEnemyScript.SetTrollScript(trollScript);
                moveEnemyScript.SetAxeSword(axe);
                moveEnemyScript.SetMyItem(myItemScript);
                moveEnemyScript.SetFireEffect(fireEffect);
                playerScript.SetEnemyScript(moveEnemyScript);
                enemycaunter += 1;
            }
        }
        
    }

   
}
