using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameExplanationScript : MonoBehaviour
{
    //ゲーム説明の際すぐにゲームに戻ってしまうのを防ぐための変数
    [SerializeField]
    private bool isExplanation;
    //ゲーム説明に必要なUIや画像たち
    [SerializeField]
    private GameObject ExplanationUI; //操作説明の文字
    [SerializeField]
    private GameObject BackGround;  //背景
    [SerializeField]
    private GameObject Keys;    //キーボードの画像
    [SerializeField]
    private GameObject LevelUpUI; //レベルアップした際の文字
    //-----------------------//
    //レベルアップの際すぐにゲームに戻ってしまうのを防ぐための変数
    [SerializeField]
    private bool islevelup;
    [SerializeField]
    private PlayerScript playerScript;  //プレイヤー
    [SerializeField]
    private GrayScaleSprict grayscript;  //ポストエフェクト
    [SerializeField]
    private GameObject textmeshpro;  //文字
    [SerializeField]
    private MyItemScript myItem;
    [SerializeField]
    private List<GameObject> chestsUI;
    private bool isAxeExplocion;
    //キーボードかコントローラーを入力しているか分ける処理
    [SerializeField]
    private List<GameObject> explanationsUI;
    private float[] iscontroller;
    [SerializeField]
    private bool isInput;
    [SerializeField]
    private bool isKeyInput;
    // Start is called before the first frame update
    void Start()
    {
        isExplanation = false;
        islevelup = false;
        isAxeExplocion = false;

    }

    // Update is called once per frame
    void Update()
    {
        //キーやボタンを押した際にUIが出るか消えるかの処理
        if(Input.GetKeyDown("joystick button 2")&&isExplanation==false|| Input.GetKeyDown(KeyCode.Y)&&isExplanation==false)
        {
            isExplanation = true;
            BackGround.SetActive(true);
            ExplanationUI.SetActive(true);
            textmeshpro.SetActive(false);
        }
        else if(Input.GetKeyDown("joystick button 2") && isExplanation == true || Input.GetKeyDown(KeyCode.Y) && isExplanation == true)
        {
            isExplanation = false;
            ExplanationUI.SetActive(false);
            BackGround.SetActive(false);
            Keys.SetActive(false);
            textmeshpro.SetActive(true);
        }
        //--------------//
        //UIが出現する時次のページに行ったり戻ったり出来る処理
        if(Input.GetKeyDown(KeyCode.L)|| Input.GetKeyDown("joystick button 5") && isExplanation == true)
        {
            ExplanationUI.SetActive(true);
            BackGround.SetActive(false);
            Keys.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 4") && isExplanation == true)
        {
            ExplanationUI.SetActive(true);
            BackGround.SetActive(true);
            Keys.SetActive(false);
        }
       
       
        //操作説明やゲームオーバーの際敵やボスの時間を止める処理
        if (isExplanation==true|| grayscript.enabled==true&&playerScript.IsGameOver())
        {
            Time.timeScale = 0;
        }
        //敵からドロップした斧を入手する時に表示されるUIの処理
        else if (myItem.GetItemCounter()==1 && islevelup == false)
        {
            LevelUpUI.SetActive(true);
            Time.timeScale = 0;
            isAxeExplocion = true;
            Debug.Log("レベルが2になった");
        }
        else
        {
            Time.timeScale = 1;
        }
        //敵が一体倒されると表示されるUIをキーやボタンを押したら消す処理
        if (Input.GetKeyDown(KeyCode.J) && playerScript.SetDeadCaunter() == 1
            || Input.GetKeyDown("joystick button 4") && playerScript.SetDeadCaunter() == 1)
        {
            LevelUpUI.SetActive(false);
            islevelup = true;
            Time.timeScale = 0;
            Debug.Log("レベル説明終了");
        }
        //宝箱の説明が表示か非表示か確認する処理
        for (int i = 0; i < chestsUI.Count; i++)
        {
            if (chestsUI[i].activeSelf)
            {
                Time.timeScale = 0;
            }
        }
      
        if (isInput&&!isKeyInput)
        {
            explanationsUI[0].SetActive(false);
            explanationsUI[1].SetActive(false);
            explanationsUI[2].SetActive(true);
            explanationsUI[3].SetActive(true);
            isKeyInput = false;
        }
        else
        {
            isKeyInput = true;
        }
        if (Input.anyKeyDown)
        {
            isInput = false;
            //isKeyInput = true;
            foreach(KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code) && isKeyInput)
                {
                    //isInput = false;
                    explanationsUI[0].SetActive(true);
                    explanationsUI[1].SetActive(true);
                    explanationsUI[2].SetActive(false);
                    explanationsUI[3].SetActive(false);
                    isInput = false;
                }
            }
        }
    }
    public bool GetIsAxeExplocion()
    {
        return isAxeExplocion;
    }

    public void OnController()
    {
        isInput = true;
        isKeyInput = false;
    }
}
