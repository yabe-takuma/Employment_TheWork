using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SearchItemScript : MonoBehaviour
{
    //アイテムを取る時のモード
    public enum GetMode
    {
        Auto,
        Manual
    }
    //自動でアイテムを取得するか手動で取得するか
    public GetMode getItemMode;
    //アイテム管理スクリプト
    [SerializeField]
    private MyItemScript myItem;
    //近くにあるアイテムのリスト
    [SerializeField]
    private List<GameObject> itemList;
    //メッセージ表示用キャンバス
    public GameObject itemMessageCanvas;

    
    // Start is called before the first frame update
    private void Start()
    {
        myItem = GetComponentInParent<MyItemScript>();
    }

    //アイテムがサーチエリア内に入ったら
    void OnTriggerEnter(Collider col)
    {
        if(col.tag=="Item")
        {
            //自動取得モード
            if(getItemMode == GetMode.Auto)
            {
                myItem.SetItem(col.transform.GetComponent<ItemScript>().GetItem());
                Destroy(col.gameObject);
            }
            //手動取得モード
            else
            {
                SetItem(col.gameObject);
                itemMessageCanvas.GetComponentInChildren<Text>().text = "アイテムを取る";
                itemMessageCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.tag=="Item")
        {
            //アイテムリストに存在していたらアイテムリストから削除
            if(itemList.Contains(col.gameObject))
            {
                itemList.Remove(col.gameObject);
                //アイテムが範囲内になかったら主人公の状態変更
                if(itemList.Count<=0)
                {
                    itemMessageCanvas.SetActive(false);
                }
            }
        }
    }

    //アイテムリストにアイテムを追加する処理
    void SetItem(GameObject addItem)
    {
        //既に同じアイテムがあるかどうか
        if(!itemList.Contains(addItem))
        {
            itemList.Add(addItem);
        }
    }

    //アイテムリストからアイテムを削除する処理
    public void DeleteItem(GameObject deleteItem)
    {
        if(itemList.Contains(deleteItem))
        {
            itemList.Remove(deleteItem);
        }
    }

    //手動モードの場合一番近いアイテムを探し取得する
    public void SelectItem()
    {
        if(getItemMode==GetMode.Manual)
        {
            //一つでも検知エリア内にアイテムがあれば
            if(itemList.Count>=1)
            {
                //アイテムリストの先頭にあるアイテムを初期値にする
                Transform near = itemList[0].transform;
                float distance = Vector3.Distance(transform.root.position, near.position);

                //一番近いアイテムを探す
                foreach (var item in itemList)
                {
                    if(Vector3.Distance(transform.root.position,item.transform.position)<distance)
                    {
                        near = item.transform;
                        distance = Vector3.Distance(transform.root.position, item.transform.position);
                    }
                }

                //一番近いアイテムを取得、アイテムリストから削除、アイテムゲームオブジェクトの削除
                itemMessageCanvas.GetComponentInChildren<Text>().text = "";
                itemMessageCanvas.SetActive(false);
                myItem.SetItem(near.GetComponent<ItemScript>().GetItem());
                DeleteItem(near.gameObject);
                Destroy(near.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
