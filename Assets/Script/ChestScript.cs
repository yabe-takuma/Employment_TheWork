using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField]
    private Animation chestanimation;  //宝箱のアニメーション
    private bool isOpen;  //宝箱を開けたかを検知する変数
    private bool isEndOpen;  //宝箱の処理に必要な変数
    [SerializeField]
    private List<GameObject> chestsOpenUI;  //宝箱を開けた時のUI
    private int chestindex;  //配列に入れる用の変数
    [SerializeField]
    private int chestcounter;  //宝箱を開けた回数
    [SerializeField]
    private ChangeEquipScript changeEquipScript;  //宝箱を開けた変数を参照するための変数
    // Start is called before the first frame update
    void Start()
    {
        chestcounter = 0;
        chestindex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //キーを押すと宝箱を開く
        if (Input.GetKeyDown(KeyCode.Q)||Input.GetKeyDown("joystick button 1"))
        {
            isOpen = true;
        }
        //複数の宝箱それぞれに違うUIを一括で変える処理
        for (int i = 0; i < chestsOpenUI.Count; i++)
        {
            if (isEndOpen && !chestanimation.isPlaying)
            {
                chestsOpenUI[chestindex].SetActive(true);
                Debug.Log("宝箱開け切った");
                isEndOpen = false;
               
            }
            if (chestsOpenUI[chestindex].activeSelf && Input.GetKeyDown(KeyCode.J)||
                chestsOpenUI[chestindex].activeSelf && Input.GetKeyDown("joystick button 4"))
            {
                chestsOpenUI[chestindex].SetActive(false);
                chestcounter++;
                changeEquipScript.SetChestCounter(chestcounter);
                if (chestindex < chestsOpenUI.Count-1)
                {
                    chestindex++;
                    chestsOpenUI[i] = chestsOpenUI[chestindex];
                }
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        
        if(col.tag=="Player")
        {
            Debug.Log("プレイヤーが来た");
            if(isOpen)
            {
                chestanimation.Play();
                isOpen = false;
                isEndOpen = true;
                Debug.Log("宝箱を開けた");
            }
            
        }
       
        
    }

    public void ChestOpenUI(List<GameObject> chestsopenUI)
    {
        chestsOpenUI = chestsopenUI;
    }

    public int GetChestCounter()
    {
        return chestcounter;
    }

    public void GetChangeEquipScript(ChangeEquipScript changeequipscript)
    {
        changeEquipScript = changeequipscript;
    }
    
}
