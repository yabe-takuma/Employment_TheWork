using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrackLife : MonoBehaviour
{
    [SerializeField]
    private LifeGauge lifegauge;
    [SerializeField]
    private GameObject hpObj;
    [SerializeField]
    private int Hp;

    //ライフゲージ全削除&HP分作成
    public void SetLifeGauge()
    {
        
        ////体力を一旦全削除
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    Destroy(transform.GetChild(i).gameObject);
        //}
        //現在の体力数分のライフゲージを作成
        for (int i = 0; i < Hp; i++)
        {
            Instantiate<GameObject>(hpObj, transform);

        }
       
    }

    // Start is called before the first frame update
    void Start()
    {
        Hp = 10;
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
