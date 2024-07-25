using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateScript : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //unitychanをplayerに格納
        player = GameObject.Find("unitychan");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //ユニティちゃんを中心に-5f度回転
            transform.RotateAround(player.transform.position, Vector3.up, -2f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(player.transform.position, Vector3.up, 2f);
        }
    }
}
