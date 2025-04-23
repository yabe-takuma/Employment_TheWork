using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TitleCameraScript : MonoBehaviour
{
    //
    [SerializeField]
    private new GameObject camera;  //プレイヤーの子関係のオブジェクト
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private bool isEvent;
    [SerializeField]
    private PlayableDirector timeline;
    [SerializeField]
    private GameObject textUI;
    [SerializeField]
    private GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        isEvent = false;
        timeline.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        TitleCamerahauding();
    }

    void TitleCamerahauding()
    {
        //プレイヤーが指定した座標に来るまで別のオブジェクトに追従
        if (player.transform.position.z < 192)
        {
            transform.position = camera.transform.position;
        }
        //プレイヤーが指定した座標に来たらフラグが立つまでカメラが回り込む演出をする処理
        if (player.transform.position.z > 192 && !isEvent)
        {
            timeline.Play();
        }
        //カメラが指定した座標に来たらフラグと立たせる
        if (transform.position.z <= 188f)
        {
            isEvent = true;
        }
        //アニメーションが終わったらUIを表示する処理
        if (timeline.time >= timeline.duration)
        {
            textUI.SetActive(true);
            panel.SetActive(true);
        }
    }
}
