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
        Screen.SetResolution(1920, 1080, false);
        grayscript.enabled = false;
        material.SetFloat("_Threshold", 0.0f);
        colorcounter = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
       

        if (material.GetFloat("_Threshold") <= 0.8f)
        {
            colorcounter += 0.001f;
            material.SetFloat("_Threshold", colorcounter);
        }
        else if (material.GetFloat("_Threshold") >= 0.8f)
        {
            fadeOutUI.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        if (playerScript.GetState() == PlayerScript.MyState.Dead)
        {
            grayscript.enabled = true;
        }
    }

   
}
