using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEquipScript : MonoBehaviour
{

    [SerializeField]
    private GameObject[] weapons;
    [SerializeField]
    private int equipment;
    //キャラクターのステータススプリクト
    [SerializeField]
    private MyStatus myStatus;
    //武器の親のTransform
    [SerializeField]
    private Transform equipTransform;

    private ProcessCharaAnimEventScript processCharaAnimEvent;
    private PlayerScript playerScript; 

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
        equipment = -1;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("1")||Input.GetKeyDown(KeyCode.RightShift)
           || Input.GetKeyDown("joystick button 5") && playerScript.GetState() == PlayerScript.MyState.Normal)
        {
            InstantiateWepon();
        }
    }

    void InstantiateWepon()
    {
        equipment++;
        if(equipment>=weapons.Length)
        {
            equipment = -1;
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

            //サンプルの為、直接武器の位置や角度を設定
            if(equipment ==0)
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.156f, 0.052f, -0.003f);
                weapon.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
                weapon.transform.localScale = new Vector3(0.05f, 0.1f, 0.05f);
            }
            else if(equipment ==1)
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(-0.27f, 0.005f, 0.092f);
                weapon.transform.localEulerAngles = new Vector3(291.87f, 6.4f, 80f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                weapon.transform.SetParent(equipTransform);
                weapon.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
                weapon.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                weapon.transform.localScale = new Vector3(1f, 1f, 1f);
            }

                myStatus.SetEquip(weapon);
        }

        
    }

}
