using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShockwaveScript : MonoBehaviour
{

    //ScaleUp用の経過時間
    private float elapsedScaleUpTime = 0f;
    //Scaleを大きくする間隔時間
    [SerializeField]
    private float scaleUpTime = 0.03f;
    //ScaleUpする割合
    [SerializeField]
    private float scaleUpParam = 0.1f;
    //パーティクル削除用の経過時間
    private float elapsedDeleteTime = 0f;
    //パーティクルを削除するまでの時間
    [SerializeField]
    private float deleteTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedScaleUpTime += Time.deltaTime;
        elapsedDeleteTime += Time.deltaTime;

        //一定時間が経ったら衝撃波を削除する
        if(elapsedDeleteTime >= deleteTime)
        {
            Destroy(gameObject);
        }
        //サークルを段々大きくする
        if(elapsedScaleUpTime > scaleUpTime)
        {
            transform.localScale += Vector3.one * scaleUpParam;
            elapsedScaleUpTime = 0f;
        }
    }
}
