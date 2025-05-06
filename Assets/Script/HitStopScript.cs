using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopScript : MonoBehaviour
{
    //どこからでも呼び出せるようにする
    public static HitStopScript instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void StartHitStop(float duration)
    {
        instance.StartCoroutine(instance.HitStopCoroutine(duration));
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
