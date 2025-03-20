using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    Mizu,
    Yakusou
}

public class ItemScript : MonoBehaviour
{

    public Item item;  //どのアイテムか
    public float deleteTime;  //アイテムが消えるまでの時間
    private SearchItemScript searchItem;
    // Start is called before the first frame update
    void Start()
    {
        searchItem = GameObject.FindWithTag("SearchItemArea").GetComponent<SearchItemScript>();
        //アイテムが登場したら消す処理スタート
        //StartCoroutine(DeleteItem());
    }

    public Item GetItem()
    {
        return item;
    }

    //指定時間が経過したらアイテムを削除
    IEnumerator DeleteItem()
    {
        yield return new WaitForSeconds(deleteTime);
        searchItem.DeleteItem(this.gameObject);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
