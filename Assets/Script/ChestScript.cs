using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField]
    private Animation chestanimation;  //宝箱のアニメーション
    [SerializeField]
    private Animator chestanimator;
    [SerializeField]
    private bool isOpen;  //宝箱を開けたかを検知する変数
    private bool isEndOpen;  //宝箱の処理に必要な変数
    [SerializeField]
    private bool isClick;  //宝箱を一回だけ処理させる変数
    [SerializeField]
    private List<GameObject> chestsOpenUI;  //宝箱を開けた時のUI
    private int chestindex;  //配列に入れる用の変数
    [SerializeField]
    private int chestcounter;  //宝箱を開けた回数
    [SerializeField]
    private ChangeEquipScript changeEquipScript;  //宝箱を開けた変数を参照するための変数
    [SerializeField]
    private bool isCollision;  //OnTrigger関数内でキーボードやボタンを使った操作を正常にするための変数
    [SerializeField]
    private GameObject collisionUI;
    [SerializeField]
    private int opentimer;

    private bool isKey;
    // Start is called before the first frame update
    void Start()
    {
        chestcounter = 0;
        chestindex = 0;
        isEndOpen = false;
        isClick = false;
        collisionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ChestAnimation();
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag=="Player")
        {
            CollisionChest();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isCollision = false;
        collisionUI.SetActive(false);

    }

    public void ChestOpenUI(List<GameObject> chestsopenUI)
    {
        chestsOpenUI = chestsopenUI;
    }

    public void ChestCollisionUI(GameObject chestscollisionUI)
    {
        collisionUI = chestscollisionUI;
    }

    public int GetChestCounter()
    {
        return chestcounter;
    }

    public void GetChangeEquipScript(ChangeEquipScript changeequipscript)
    {
        changeEquipScript = changeequipscript;
    }

    void CollisionChest()
    {
        isCollision = true;
        if (!isClick)
        {
            collisionUI.SetActive(true);
        }
        Debug.Log("プレイヤーが来た");
        //キーを押すと宝箱を開く
        if (isOpen)
        {
            isEndOpen = true;
            //chestanimation.Play();
            chestanimator.SetTrigger("Open");
            Debug.Log("宝箱を開けた");
        }
    }
    
    void ChestAnimation()
    {
        //複数の宝箱それぞれに違うUIを一括で変える処理
        for (int i = 0; i < chestsOpenUI.Count; i++)
        {
            if (isEndOpen && opentimer >= 187)
            {
                chestsOpenUI[chestindex].SetActive(true);
                Debug.Log("宝箱開け切った");
                isOpen = false;
                isEndOpen = false;
                isKey = true;
            }
            if (chestsOpenUI[chestindex].activeSelf && Input.GetKeyDown(KeyCode.J) ||
                chestsOpenUI[chestindex].activeSelf && Input.GetKeyDown("joystick button 4"))
            {
                chestsOpenUI[chestindex].SetActive(false);
                collisionUI.SetActive(false);
                chestcounter++;
                changeEquipScript.SetChestCounter(chestcounter);
                isClick = true;
                if (chestindex < chestsOpenUI.Count - 1)
                {
                    chestindex++;
                    chestsOpenUI[i] = chestsOpenUI[chestindex];
                    isClick = true;
                }
            }
        }
        if (isCollision && Input.GetKeyDown(KeyCode.Q) && !isClick && !isKey || isCollision && Input.GetKeyDown("joystick button 1") && !isClick && !isKey)
        {
            isOpen = true;

        }

        if (isOpen)
        {
            opentimer++;
            Time.timeScale = 0;
        }
    }

}
