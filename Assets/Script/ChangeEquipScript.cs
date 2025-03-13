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
        if(Input.GetKeyDown("1")||Input.GetKeyDown(KeyCode.RightShift) && playerScript.SetDeadCaunter() >= 1
           || Input.GetKeyDown("joystick button 5") && playerScript.GetState() == PlayerScript.MyState.Normal
           && playerScript.SetDeadCaunter() >= 1)
        {
            InstantiateWepon();
            
        }
    }

    void InstantiateWepon()
    {
        equipment++;
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
            else if(equipment==1&&playerScript.SetDeadCaunter()>=1)
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.1f, 0.05f, 0.5f);
                weapon.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
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

}
