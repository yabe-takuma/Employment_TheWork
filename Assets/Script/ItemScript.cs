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
    }

    public Item GetItem()
    {
        return item;
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
