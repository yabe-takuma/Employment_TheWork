using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameExplanationScript : MonoBehaviour
{
    [SerializeField]
    private bool isExplanation;
    [SerializeField]
    private GameObject ExplanationUI;
    [SerializeField]
    private GameObject BackGround;
    [SerializeField]
    private GameObject Keys;
    [SerializeField]
    private GameObject GameUI;
    [SerializeField]
    private GameObject LevelUpUI;
    private int leveluptimer;
    private bool islevelup;
    [SerializeField]
    private PlayerScript playerScript;
    [SerializeField]
    private GrayScaleSprict grayscript;
    [SerializeField]
    private GameObject textmeshpro;
    // Start is called before the first frame update
    void Start()
    {
        isExplanation = false;
        leveluptimer = 0;
        islevelup = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("joystick button 2")&&isExplanation==false|| Input.GetKeyDown(KeyCode.Y)&&isExplanation==false)
        {
            isExplanation = true;
            BackGround.SetActive(true);
            ExplanationUI.SetActive(true);
            GameUI.SetActive(true);
            textmeshpro.SetActive(false);
        }
        else if(Input.GetKeyDown("joystick button 2") && isExplanation == true || Input.GetKeyDown(KeyCode.Y) && isExplanation == true)
        {
            isExplanation = false;
            ExplanationUI.SetActive(false);
            BackGround.SetActive(false);
            GameUI.SetActive(false);
            Keys.SetActive(false);
            textmeshpro.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.L)|| Input.GetKeyDown("joystick button 5") && isExplanation == true)
        {
            GameUI.SetActive(true);
            ExplanationUI.SetActive(true);
            BackGround.SetActive(false);
            Keys.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 4") && isExplanation == true)
        {
            GameUI.SetActive(true);
            ExplanationUI.SetActive(true);
            BackGround.SetActive(true);
            Keys.SetActive(false);
        }
       
       

        if (isExplanation==true|| grayscript.enabled==true&&playerScript.IsGameOver())
        {
            Time.timeScale = 0;
        }
        else if (playerScript.SetDeadCaunter() == 1 && islevelup==false)
        {
            LevelUpUI.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("レベルが2になった");
        }
        else
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.J) && playerScript.SetDeadCaunter() == 1)
        {
            LevelUpUI.SetActive(false);
            Time.timeScale = 0;
            Debug.Log("レベル説明終了");
            islevelup = true;
        }

    }
}
