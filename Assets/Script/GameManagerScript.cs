using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    [SerializeField]
    private PlayerScript playerScript;
    [SerializeField]
    private GrayScaleSprict grayscript;
    //ゲーム開始時にフェードインの演出を入れる変数
    [SerializeField]
    private Material material;
    private float colorcounter;
    [SerializeField]
    private GameObject fadeOutUI;

    

    // Start is called before the first frame update
    void Start()
    {
        grayscript.enabled = false;
        material.SetFloat("_Threshold", 0.0f);
        colorcounter = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
       
        //ゲーム開始時徐々に画面が見えるようにする演出を入れる処理
        if (material.GetFloat("_Threshold") <= 0.8f)
        {
            colorcounter += 0.004f;
            material.SetFloat("_Threshold", colorcounter);
        }
        //透明になったらUIを非表示にする処理
        else if (material.GetFloat("_Threshold") >= 0.8f)
        {
            fadeOutUI.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        //ゲームオーバーになる時スクリプトを表示させてグレイスケールを発生させる処理
        if (playerScript.GetState() == PlayerScript.MyState.Dead)
        {
            grayscript.enabled = true;
            
        }
       
    }

   
}
