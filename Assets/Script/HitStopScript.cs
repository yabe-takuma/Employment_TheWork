using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopScript : MonoBehaviour
{
    //どこからでも呼び出せるようにする
    public static HitStopScript instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
      
    }





    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void StartHitStop(float duration)
    {
        if (this.gameObject != null)
        {
            instance.StartCoroutine(instance.HitStopCoroutine(duration));
            Debug.Log("ヒットストップ発生中");
        }

    }

    private IEnumerator HitStopCoroutine(float duration)
    {
        Debug.Log("ヒットストップ発動");
        //ヒットストップの開始
        Time.timeScale = 0f;

        //指定した時間だけ停止
        yield return new WaitForSecondsRealtime(duration);

        //ヒットストップの終了
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
