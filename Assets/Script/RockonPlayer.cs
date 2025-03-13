using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockonPlayer : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーと同じ座標にする処理
        transform.position = player.transform.position;
        //プレイヤーと同じ回転座標にして常にプレイヤーの背中を追ってもらう処理
        transform.rotation = new Quaternion(transform.rotation.x,player.transform.rotation.y,transform.rotation.z, player.transform.rotation.w);
    }
}
