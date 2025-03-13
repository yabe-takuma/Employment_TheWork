using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeParticle : MonoBehaviour
{
    //ボスが設置物を置いた時の消滅に必要な変数
    [SerializeField]
    private GameObject particle;
    [SerializeField]
    private int caunter;
    //--------------//
    // Start is called before the first frame update
    void Start()
    {
        //消滅までの時間
        Destroy(this.gameObject, 5f);
        //ボスが特定の行動の時設置物を生成
        Instantiate(particle, transform.position, particle.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
