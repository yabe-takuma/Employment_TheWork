using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class MyItemScript : MonoBehaviour
{
    public int[] items = new int[2];  //持っているアイテム数分の配列を用意
    public GameObject canvas;  //アイテムを取得したことを表示するテキストキャンバス
    private bool iscanvas;  //キャンバスが開いているかどうか確認するための物

    // Start is called before the first frame update
    void Start()
    {
        //アイテム数初期化処理
        for(int i=0;i<items.Length;i++)
        {
            items[i] = 0;
        }
        
    }

    //アイテムを取得する処理
    public void SetItem(Item item)
    {
        //アイテム数を1つ足す
        items[(int)item] += 1;
        //アイテム名を入れる変数を宣言
        string itemName = "";

        //アイテムの種類でアイテム名を指定
        if(item == Item.Mizu)
        {
            itemName = "水";
        }
        else if(item==Item.Yakusou)
        {
            itemName = "薬草";
        }
        //テキストUIを取得
        Text preText = canvas.GetComponentInChildren<Text>();
        //表示する文字列を宣言
        string putText = "";

        //空文字でなければ改行文字を入れておく
        if(preText.text != "")
        {
            putText = "/n";
        }

        //表示する文字列に取得したアイテム情報を追加
        putText += itemName + "を手に入れた";
        preText.text += putText;

        //アイテム取得表示用UIが表示されていなければUIを表示
        if(!canvas.activeSelf)
        {
            StartCoroutine(ShowText());
        }
        
        //確認の為、それぞれの数をコンソールに表示
        Debug.Log("持っている水:" + GetItem(Item.Mizu) + " 持っている薬草" + GetItem(Item.Yakusou));
       
        
    }

    //アイテム数を返す
    public int GetItem(Item item)
    {
        return items[(int)item];
    }

    //アイテム取得表示用テキストのオン、オフ、UIを表示してから10秒たったら非表示にする
    IEnumerator ShowText()
    {
        canvas.SetActive(true);
        yield return new WaitForSeconds(10.0f);
        canvas.SetActive(false);
        canvas.transform.GetChild(0).GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas.activeSelf&&Input.GetKeyDown(KeyCode.J) || canvas.activeSelf&&Input.GetKeyDown("joystick button 4"))
        {
            canvas.SetActive(false);
            iscanvas = true;
            Debug.Log("説明終了");
        }
        else
        {
            iscanvas = false;
        }
    }

    public int GetItemCounter()
    {
        return items[0];
    }

    public bool  GetCanvas()
    {
        return iscanvas;
    }
}
