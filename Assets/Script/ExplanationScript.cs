using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplanationScript : MonoBehaviour
{
    [SerializeField]
    private Material material;
    private float colorcounter;
    private bool isInput;
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1"))
        {
            isInput = true;
        }
        if (!isInput)
        {
            colorcounter += 0.001f;
            if (material.GetFloat("_Threshold") <= 1.0f)
            {
                material.SetFloat("_Threshold", colorcounter);
            }
        }
        else
        {
            colorcounter -= 0.001f;
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
