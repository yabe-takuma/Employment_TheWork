using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGauge : MonoBehaviour
{
    //ライフゲージプレハブ
    [SerializeField]
    private GameObject hpObj;
    [SerializeField]
    private GameObject brackhpObj;
    [SerializeField]
    private int Hp;

    

    //ライフゲージ全削除&HP分作成
    public void SetLifeGauge(int hp)
    {
        //体力を一旦全削除
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        //現在の体力数分のライフゲージを作成
        for (int i=0;i<hp;i++)
        {
            Instantiate<GameObject>(hpObj, transform);
            //Instantiate<GameObject>(brackhpObj, transform);
        }
        //for (int i = 0; i < 10; i++)
        //{
        //    Instantiate<GameObject>(brackhpObj, transform);
        //}
        Hp = hp;
    }
    //ダメージ分だけ削除
    public void SetDamageLifeGauge(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            //最後のライフゲージを削除
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetHP()
    {
        return Hp;
    }
}
