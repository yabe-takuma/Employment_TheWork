using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTextScript : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    //フェードさせる時間
    [SerializeField]
    private float fadeTime = 1f;
    //経過時間を取得
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //経過時間をfadeTimeで割った値をalphaに入れる
        canvasGroup.alpha = timer / fadeTime;
    }
}
