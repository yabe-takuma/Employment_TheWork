using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveAxeAttackEventScript : MonoBehaviour
{
    //爆発のパーティクルを格納する変数
    [SerializeField]
    private GameObject particle;
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //アニメーション中に爆発を生成する処理
    public void CreateAxeShockwave()
    {
        Instantiate(particle, new Vector3(transform.position.x-0.34f,transform.position.y+0.589f,transform.position.z), particle.transform.rotation);
    }
}
