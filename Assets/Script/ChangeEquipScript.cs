using RPGCharacterAnims.Lookups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeEquipScript : MonoBehaviour
{
    [Header("武器を複数格納するための変数")]
    [SerializeField]
    private GameObject[] weapons;  //武器を複数格納するための変数
    [Header("何番目の武器にしているかを確認する用の変数")]
    [SerializeField]
    private int equipment;
    //-----------------------//
    [Header("プレイヤーのステータス専用のスクリプト")]
    [SerializeField]
    private MyStatus myStatus;
    //武器の親のTransform
    [Header("武器を生成する場所のオブジェクト")]
    [SerializeField]
    private Transform equipTransform;
    //武器のコライダーをゲーム開始で付けるための変数
    private ProcessCharaAnimEventScript processCharaAnimEvent;
    private PlayerScript playerScript;
    //--------------------------//
    [Header("宝箱の動き全般を管理するスクリプト")]
    [SerializeField]
    private ChestScript chestScript;  //宝箱を開けた数に応じて使える武器を設定するため

    [Header("宝箱のアイテムのデータを管理するスクリプト")]
    [SerializeField]
    private ChestData chestData;  //宝箱のアイテムのデータを別のシーン内でも適用するために必要なスクリプト
    [Header("ポーズ中の処理をするスクリプト")]
    [SerializeField]
    private GameExplanationScript gameExplanationScript;
    ////武器の情報のみ(生成はしない)
    //[SerializeField]
    //private GameObject weaponinfo;
    [Header("剣の攻撃について記述されているスクリプト")]
    [SerializeField]
    private AttackSwordScript attackSwordScript;
    //他のスクリプトに参照するための関数
    public int GetEquipment()
    {
        return equipment;
    }

  

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        processCharaAnimEvent = transform.root.GetComponent<ProcessCharaAnimEventScript>();

        //初期装備設定
        equipment = 0;
        StartWepon();
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            chestData.Initialize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //特定のキーやボタンを押したら武器切り替える処理
        if(Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown("joystick button 5"))
        {
            InstantiateWepon();
            
        }
        
    }

    void InstantiateWepon()
    {
        equipment++;
        //斧を持っていなかったら数字を加算して斧を持たないようにする処理
        if(!gameExplanationScript.GetIsAxeExplocion() && equipment==1)
        {
            equipment++;
        }
        //炎の剣を持っていなかったら数字を加算して炎の剣を持たないようにする処理
        if (equipment == 2 && chestData.chestCounter == 0) 
        {
            equipment++;
        }
        //水の剣を持っていなかったら数字を加算して水の剣を持たないようにする処理
        if (equipment == 3 && chestData.chestCounter <= 1) 
        {
            equipment++;
        }
        //2つ目の斧を持っていなかったら数字を加算して2つ目の斧を持たないようにする処理
        if (equipment == 4 && chestData.chestCounter <= 2)
        {
            equipment++;
        }
        //3つ目の斧を持っていなかったら数字を加算して3つ目の斧を持たないようにする処理
        if (equipment == 5 && chestData.chestCounter <= 3)
        {
            equipment++;
        }
        //持っている武器以上の数値になったら0にする処理
        if (equipment>=weapons.Length)
        {
            equipment = 0;
        }
        //今装備している武器を削除
        if (equipTransform.childCount != 0)
        {
            Destroy(equipTransform.GetChild(0).gameObject);
        }
        //素手ではない時だけ武器をインスタンス化
        if (equipment != -1)
        {
            //新しく装備する武器をインスタンス化
            var weapon = Instantiate<GameObject>(weapons[equipment]);
            processCharaAnimEvent.SetCollider(weapon.GetComponent<Collider>());
            attackSwordScript = weapon.GetComponentInChildren<AttackSwordScript>();


            //武器の位置や角度を設定
            if (equipment ==0) //剣
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.1f, 0.03f, 0.1f);
                weapon.transform.localEulerAngles = new Vector3(291.87f, 90.4f, 80f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if(equipment==1) //斧
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.1f, 0.05f, 0.05f);
                weapon.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                weapon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else if (equipment == 2) //炎の剣
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.1f, 0.03f, 0.1f);
                weapon.transform.localEulerAngles = new Vector3(291.87f, 90.4f, 80f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (equipment == 3) //水の剣
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.1f, 0.03f, 0.1f);
                weapon.transform.localEulerAngles = new Vector3(291.87f, 90.4f, 80f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if(equipment ==4) //回転する斧
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.105f, 0.046f, 0.215f);
                weapon.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (equipment == 5) //でかい斧
            {
                weapon.transform.SetParent(equipTransform);
               
                weapon.transform.localPosition = new Vector3(-0.183f, 0.058f, 0.485f);
                weapon.transform.localEulerAngles = new Vector3(-80.0f, 90.0f, 90.0f);
                weapon.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }

            myStatus.SetEquip(weapon);
        }

        
    }
    //ゲーム開始時剣を持ってもらうための関数
    void StartWepon()
    {
        var weapon = Instantiate<GameObject>(weapons[equipment]);
        processCharaAnimEvent.SetCollider(weapon.GetComponent<Collider>());

        weapon.transform.SetParent(equipTransform);
        weapon.transform.localPosition = new Vector3(-0.1f, 0.03f, 0.05f);
        weapon.transform.localEulerAngles = new Vector3(291.87f, 90.4f, 80f);
        weapon.transform.localScale = new Vector3(1f, 1f, 1f);
        attackSwordScript = weapon.GetComponentInChildren<AttackSwordScript>();
        myStatus.SetEquip(weapon);
    }
    //宝箱を開けた回数を記録するための関数
    public void SetChestCounter(int counter)
    {
        chestData.chestCounter = counter;
    }

    public void StartAttackParticle()
    {
        attackSwordScript.PlaySwordTrail();
    }

   
}
