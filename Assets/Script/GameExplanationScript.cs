using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField]
    private List<GameObject> gameOverTextsUI;
    [SerializeField]
    private List<GameObject> gameClearTextsUI;

    private bool isExplocionflag;
    private bool isTitle;
    private bool isGame;
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private PauseScript pauseScript;
    private bool isMoveUI;
    //タイトルに戻るためにSceneScriptを参照
    [SerializeField]
    private SceneScript sceneScript;
    private bool isChest;

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
        Keyshanding();
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

    void Keyshanding()
    {
        //キーやボタンを押した際にUIが出るか消えるかの処理
        if (Input.GetKeyDown("joystick button 7")&& grayscript.enabled == false && !playerScript.IsGameOver()&&!playerScript.IsGameClear() && !isChest || Input.GetKeyDown(KeyCode.Y)&& grayscript.enabled == false && !playerScript.IsGameOver() && !playerScript.IsGameClear()&&!isChest)
        {
            isExplanation = true;
            pauseUI.SetActive(true);
            textmeshpro.SetActive(false);
            isMoveUI = false;
            PauseScript.Instance.ChangeState(PauseScript.GameState.Pause);
        }
       
        if(!isMoveUI&&isExplanation&&PauseScript.Instance.currentState == PauseScript.GameState.Instructions)
        {
            BackGround.SetActive(true);
            ExplanationUI.SetActive(true);
            pauseUI.SetActive(false);
        }
        if (Input.GetKeyDown("joystick button 2")  || Input.GetKeyDown(KeyCode.Y))
        {
            ExplanationUI.SetActive(false);
            BackGround.SetActive(false);
            PauseScript.Instance.ChangeState(PauseScript.GameState.Pause);
            Keys.SetActive(false);
            textmeshpro.SetActive(true);
        }
        //--------------//
        //UIが出現する時次のページに行ったり戻ったり出来る処理
        if (Input.GetKeyDown(KeyCode.L) && PauseScript.Instance.currentState == PauseScript.GameState.Instructions || Input.GetKeyDown("joystick button 5") && PauseScript.Instance.currentState == PauseScript.GameState.Instructions)
        {
            ExplanationUI.SetActive(true);
            BackGround.SetActive(false);
            Keys.SetActive(true);
            isMoveUI = true;
        }
        else if (Input.GetKeyDown(KeyCode.R) && PauseScript.Instance.currentState == PauseScript.GameState.Instructions || Input.GetKeyDown("joystick button 4") && PauseScript.Instance.currentState == PauseScript.GameState.Instructions)
        {
            ExplanationUI.SetActive(true);
            BackGround.SetActive(true);
            Keys.SetActive(false);
            isMoveUI = false;
        }
        //ポーズ画面で再開やタイトルに戻る場合の処理
        if(isExplanation&&PauseScript.Instance.currentState==PauseScript.GameState.Playing|| isExplanation && PauseScript.Instance.currentState == PauseScript.GameState.Title)
        {
            isExplanation = false;
            pauseUI.SetActive(false);
            islevelup = true;
            Time.timeScale = 1;
        }
        if(PauseScript.Instance!=null&&PauseScript.Instance.currentState == PauseScript.GameState.Title)
        {
            sceneScript.Title();
        }

        //操作説明やゲームオーバーの際敵やボスの時間を止める処理
        if (grayscript.enabled == true && playerScript.IsGameOver())
        {
            Time.timeScale = 0;
        }
        else if (isExplanation)
        {
            StartCoroutine("ExplanationCoroutine");
            pauseScript.MoveIcon();
            pauseScript.PushBotton();
        }
        //敵からドロップした斧を入手する時に表示されるUIの処理
        else if (myItem.GetItemCounter() == 1 && islevelup == false)
        {
            StartCoroutine("ItemCoroutine");
        }
       
        //宝箱の説明が表示か非表示か確認する処理
        for (int i = 0; i < chestsUI.Count; i++)
        {
            if(chestsUI[i].activeSelf)
            {
                StartCoroutine("ChestCoroutine");
            }
        }
        
        //コントローラーのボタンを押したらコントローラーの操作説明が表示される処理
        if (isInput && !isKeyInput)
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
        //キーボードを押したらキーボードの操作説明が表示される処理
        if (Input.anyKeyDown)
        {
            isInput = false;
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code) && isKeyInput)
                {
                    explanationsUI[0].SetActive(true);
                    explanationsUI[1].SetActive(true);
                    explanationsUI[2].SetActive(false);
                    explanationsUI[3].SetActive(false);
                    isInput = false;
                }
            }
        }
        if (isExplanation || playerScript.IsGameOver()||playerScript.IsGameClear())
        {
            explanationsUI[0].SetActive(false);
            explanationsUI[1].SetActive(false);
            explanationsUI[2].SetActive(false);
            explanationsUI[3].SetActive(false);
        }

    }
    //敵が一体倒されて斧を拾うと表示されるUIをキーやボタンを押したら消す処理
    private IEnumerator ItemCoroutine()
    {
        Time.timeScale = 0;

        LevelUpUI.SetActive(true);
        isAxeExplocion = true;
        Debug.Log("レベルが2になった");

       
        yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.J) && playerScript.SetDeadCaunter() == 1
            || Input.GetKeyDown("joystick button 4") && playerScript.SetDeadCaunter() == 1);
        

        LevelUpUI.SetActive(false);
        islevelup = true;

        Time.timeScale = 1;
    }

    private IEnumerator ChestCoroutine()
    {
        Time.timeScale = 0;
        isChest = true;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown("joystick button 4"));
        isChest = false;
        Time.timeScale = 1;
    }

    private IEnumerator ExplanationCoroutine()
    {
        Time.timeScale = 0;
        playerScript.StopPlayerMotion();
        explanationsUI[0].SetActive(false);
        explanationsUI[1].SetActive(false);
        explanationsUI[2].SetActive(false);
        explanationsUI[3].SetActive(false);
        yield return new WaitUntil(() => PauseScript.Instance.currentState == PauseScript.GameState.Playing || PauseScript.Instance.currentState == PauseScript.GameState.Title);
        playerScript.StartPlayerMotion();
        Time.timeScale = 1;
    }

    public bool IsKeyInput()
    {
        return isKeyInput;
    }

    public void GameOverText()
    {
        if(isKeyInput)
        {
           // gameOverTextsUI[0].SetActive(false);
            gameOverTextsUI[1].SetActive(true);
        }
        else 
        {
            gameOverTextsUI[0].SetActive(true);
            //gameOverTextsUI[1].SetActive(false);
        }
    }

    public void GameClearText()
    {
        if (isKeyInput)
        {
            // gameOverTextsUI[0].SetActive(false);
            gameClearTextsUI[1].SetActive(true);
        }
        else
        {
            gameClearTextsUI[0].SetActive(true);
            //gameOverTextsUI[1].SetActive(false);
        }
    }
}
