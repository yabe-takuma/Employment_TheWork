using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplocionOmenScript : MonoBehaviour
{
    //オブジェクトを徐々に大きくするための変数
    private Vector3 omentimer;
    // Start is called before the first frame update
    void Start()
    {
        omentimer = new Vector3(0.1f, 0, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        //特定のサイズになるまで大きくする処理
        if (transform.localScale.z <= 50)
        {
            transform.localScale += omentimer;
          
        }
        //特定のサイズ以上になったら削除するための処理
        if(transform.localScale.z>=50)
        {
            Destroy(this.gameObject,1f);
        }
    }
}
