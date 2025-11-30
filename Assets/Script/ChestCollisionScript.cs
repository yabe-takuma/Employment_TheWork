using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChestCollisionScript : MonoBehaviour
{
    [SerializeField]
    private GameObject collisionUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChestFlag()
    {
        // 全ての FlagHolder を取得
        ChestsFlagScript[] allFlags = FindObjectsOfType<ChestsFlagScript>();

        // trueになっているオブジェクトをカウント
        int activeCount = 0;
        foreach (var flag in allFlags)
        {
            if (flag.isActive) activeCount++;
        }

        // 2つだけtrueにして、それ以外はfalseにする
        if (activeCount > 2)
        {
            int count = 0;
            foreach (var flag in allFlags)
            {
                if (flag.isActive)
                {
                    count++;
                    if (count > 2)
                    {
                        flag.isActive = false;
                    }
                }
            }
        }

    }
    //プレイヤーに宝箱が触れたらボタン表示するための処理
    public void ChestCollisionUI(GameObject chestscollisionUI)
    {
        collisionUI = chestscollisionUI;
    }
    
    public void IsChestActive(GameObject chestflag)
    {
        collisionUI = chestflag;
    }
    public void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            collisionUI.SetActive(true);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        collisionUI.SetActive(false);
    }
}
