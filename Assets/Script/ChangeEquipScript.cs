using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEquipScript : MonoBehaviour
{
    //武器を複数格納するための変数
    [SerializeField]
    private GameObject[] weapons;
    [SerializeField]
    private int equipment;
    //-----------------------//
    //キャラクターのステータススプリクト
    [SerializeField]
    private MyStatus myStatus;
    //武器の親のTransform
    [SerializeField]
    private Transform equipTransform;
    //武器のコライダーをゲーム開始で付けるための変数
    private ProcessCharaAnimEventScript processCharaAnimEvent;
    private PlayerScript playerScript;
    //--------------------------//
    [SerializeField]
    private ChestScript chestScript;  //宝箱を開けた数に応じて使える武器を設定するため

    [SerializeField]
    private int chestcounter;
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
        if(playerScript.SetDeadCaunter()<1&&equipment==1)
        {
            equipment++;
        }
        if (equipment == 2 && chestcounter == 0) 
        {
            equipment++;
        }
        if (equipment == 3 && chestcounter <= 1) 
        {
            equipment++;
        }
        if (equipment == 4 && chestcounter <= 2)
        {
            equipment++;
        }
        if (equipment == 5 && chestcounter <= 3)
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

            //武器の位置や角度を設定
            if(equipment ==0)
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.24f, 0.03f, 0f);
                weapon.transform.localEulerAngles = new Vector3(291.87f, 6.4f, 80f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if(equipment==1)
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.1f, 0.05f, 0.5f);
                weapon.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                weapon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else if (equipment == 2)
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.24f, 0.03f, 0f);
                weapon.transform.localEulerAngles = new Vector3(291.87f, 6.4f, 80f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (equipment == 3)
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.24f, 0.03f, 0f);
                weapon.transform.localEulerAngles = new Vector3(291.87f, 6.4f, 80f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if(equipment ==4)
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.105f, 0.046f, 0.215f);
                weapon.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (equipment == 5)
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
        weapon.transform.localPosition = new Vector3(-0.24f, 0.03f, 0f);
        weapon.transform.localEulerAngles = new Vector3(291.87f, 6.4f, 80f);
        weapon.transform.localScale = new Vector3(1f, 1f, 1f);
        myStatus.SetEquip(weapon);
    }
    //宝箱を開けた回数を記録するための関数
    public void SetChestCounter(int counter)
    {
        chestcounter = counter;
    }

}
