using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplanationScript : MonoBehaviour
{
    
    [SerializeField]
    private Material material;　　　//シェーダーを参照するための変数
    private float colorcounter;     //ディゾルブするために必要な変数
    private bool isInput;           //ボタンを押したかを検知する変数
    // Start is called before the first frame update
    void Start()
    {
        material.SetFloat("_Threshold", 1.0f);
        colorcounter = 0.0f;
        isInput = false;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム説明画面の時ゲーム画面に行くための処理
        if (Input.GetKeyDown(KeyCode.Space)&& material.GetFloat("_Threshold") >= 1.0f || Input.GetKeyDown("joystick button 1")&&material.GetFloat("_Threshold") >= 1.0f)
        {
            isInput = true;
        }
        //押していないときは徐々に消えていく処理
        if (!isInput)
        {
            colorcounter += 0.004f;
            if (material.GetFloat("_Threshold") <= 1.0f)
            {
                material.SetFloat("_Threshold", colorcounter);
            }
        }
        //押したときは徐々に画面を見えなくして次のシーンに行くための処理
        else
        {
            colorcounter -= 0.008f;
            if (material.GetFloat("_Threshold") >= 0.0f)
            {
                material.SetFloat("_Threshold", colorcounter);
            }
            else
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
}
