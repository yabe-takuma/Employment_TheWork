using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    [SerializeField]
    private NormalAxeData normalAxeData;
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
    [SerializeField]
    private List<GameObject> weaponsUI;
    //次のステージに行くためのオブジェクトを出現させる
    [SerializeField]
    private GameObject nextStageObject;

 
    public enum TimeStopRespon
    {
        None,
        Explanation,
        ItemGet,
        GameOver,
        UIInteraction
    }
    [SerializeField]
    private TimeStopRespon timeStopRespon = TimeStopRespon.None;
    // Start is called before the first frame update
    void Start()
    {
        isExplanation = false;
        islevelup = false;
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            normalAxeData.Initialize();
        }
        isKeyInput = true;
        isChest = false;
    }

    // Update is called once per frame
    void Update()
    {
        Keyshanding();
    }
    public bool GetIsAxeExplocion()
    {
        return normalAxeData.isAxeExplocion;
    }

    public void OnController()
    {
        isInput = true;
        isKeyInput = false;
    }

    //void Keyshanding()
    //{
    //    //キーやボタンを押した際にUIが出るか消えるかの処理
    //    if (Input.GetKeyDown("joystick button 7")&& grayscript.enabled == false && !playerScript.IsGameOver()&&!playerScript.IsGameClear() && !isChest &&!isExplanation || Input.GetKeyDown(KeyCode.Y)&& grayscript.enabled == false && !playerScript.IsGameOver() && !playerScript.IsGameClear()&&!isChest&&!isExplanation)
    //    {
    //        isExplanation = true;
    //        pauseUI.SetActive(true);
    //        textmeshpro.SetActive(false);
    //        isMoveUI = false;
    //        PauseScript.Instance.ChangeState(PauseScript.GameState.Pause);
    //    }

    //    if (Input.GetKeyDown("joystick button 2")  || Input.GetKeyDown(KeyCode.Y))
    //    {
    //        ExplanationUI.SetActive(false);
    //        BackGround.SetActive(false);
    //        PauseScript.Instance.ChangeState(PauseScript.GameState.Pause);
    //        Keys.SetActive(false);
    //        textmeshpro.SetActive(true);
    //    }
    //    //操作説明になった時のコントローラーの操作画面の状態
    //    if (!isMoveUI && isExplanation && PauseScript.Instance.currentState == PauseScript.GameState.Instructions)
    //    {
    //        BackGround.SetActive(true);
    //        ExplanationUI.SetActive(true);
    //        pauseUI.SetActive(false);
    //    }
    //    //--------------//
    //    //UIが出現する時次のページに行ったり戻ったり出来る処理
    //    //操作説明になった時のコントローラーの操作画面の状態に戻る処理
    //    if (Input.GetKeyDown(KeyCode.L) && PauseScript.Instance.currentState == PauseScript.GameState.Instructions || Input.GetKeyDown("joystick button 5") && PauseScript.Instance.currentState == PauseScript.GameState.Instructions)
    //    {
    //        ExplanationUI.SetActive(true);
    //        BackGround.SetActive(false);
    //        Keys.SetActive(true);
    //        isMoveUI = true;
    //    }
    //    //操作説明になった時のキーボードの操作画面の状態
    //    else if (Input.GetKeyDown(KeyCode.R) && PauseScript.Instance.currentState == PauseScript.GameState.Instructions || Input.GetKeyDown("joystick button 4") && PauseScript.Instance.currentState == PauseScript.GameState.Instructions)
    //    {
    //        ExplanationUI.SetActive(true);
    //        BackGround.SetActive(true);
    //        Keys.SetActive(false);
    //        isMoveUI = false;
    //    }
    //    //武器の効果確認画面の処理
    //    if (isExplanation && pauseScript.currentState==PauseScript.GameState.WeaponInstuctions)
    //    {
    //        BackGround.SetActive(false);
    //        ExplanationUI.SetActive(false);
    //        ExplanationUI.SetActive(false);
    //        weaponsUI[0].SetActive(true);
    //        LevelUpUI.SetActive(true);
    //        pauseUI.SetActive(false);
    //        Debug.Log("武器の説明");
    //    }
    //    if(pauseScript.currentState == PauseScript.GameState.WeaponInstuctions)
    //    {
    //        BackGround.SetActive(false);
    //        ExplanationUI.SetActive(false);
    //        pauseUI.SetActive(false);
    //    }
    //    if(Input.GetKey(KeyCode.B))
    //    {
    //        LevelUpUI.SetActive(true);
    //        pauseUI.SetActive(false);
    //    }
    //    //ポーズ画面で再開やタイトルに戻る場合の処理
    //    if ((PauseScript.Instance.currentState == PauseScript.GameState.Playing || PauseScript.Instance.currentState == PauseScript.GameState.Title)
    //        && timeStopRespon == TimeStopRespon.Explanation)
    //    {
    //        isExplanation = false;
    //        pauseUI.SetActive(false);
    //        islevelup = true;
    //        Time.timeScale = 1;
    //        timeStopRespon = TimeStopRespon.None;
    //    }
    //    if (PauseScript.Instance!=null&&PauseScript.Instance.currentState == PauseScript.GameState.Title)
    //    {
    //        sceneScript.Title();
    //    }

    //    //操作説明やゲームオーバーの際敵やボスの時間を止める処理
    //    if (grayscript.enabled == true && playerScript.IsGameOver())
    //    {
    //        if (timeStopRespon == TimeStopRespon.None)
    //        {
    //            Time.timeScale = 0;
    //        }
    //    }
    //    else if (isExplanation)
    //    {
    //        StartCoroutine("ExplanationCoroutine");
    //        pauseScript.MoveIcon();
    //        pauseScript.PushBotton();
    //    }
    //    //敵からドロップした斧を入手する時に表示されるUIの処理
    //    else if (myItem.GetItemCounter() == 1 && islevelup == false)
    //    {
    //        StartCoroutine("ItemCoroutine");
    //    }

    //    //宝箱の説明が表示か非表示か確認する処理
    //    for (int i = 0; i < chestsUI.Count; i++)
    //    {
    //        if(chestsUI[i].activeSelf)
    //        {
    //            StartCoroutine("ChestCoroutine");
    //        }
    //    }

    //    //コントローラーのボタンを押したらコントローラーの操作説明が表示される処理
    //    if (isInput && !isKeyInput)
    //    {
    //        explanationsUI[0].SetActive(false);
    //        explanationsUI[1].SetActive(false);
    //        explanationsUI[2].SetActive(true);
    //        explanationsUI[3].SetActive(true);
    //        isKeyInput = false;
    //    }
    //    else
    //    {
    //        isKeyInput = true;
    //    }
    //    //キーボードを押したらキーボードの操作説明が表示される処理
    //    if (IsKeyboardInput())
    //    {
    //        isInput = false;
    //        isKeyInput = true;
    //        explanationsUI[0].SetActive(true);
    //        explanationsUI[1].SetActive(true);
    //        explanationsUI[2].SetActive(false);
    //        explanationsUI[3].SetActive(false);
    //    }


    //    if (isExplanation || playerScript.IsGameOver()||playerScript.IsGameClear())
    //    {
    //        explanationsUI[0].SetActive(false);
    //        explanationsUI[1].SetActive(false);
    //        explanationsUI[2].SetActive(false);
    //        explanationsUI[3].SetActive(false);
    //    }

    //   if(playerScript.SetDeadCaunter()>=5)
    //   {
    //        nextStageObject.SetActive(true);
    //   }



    //}

    void Keyshanding()
    { 
        //キーやボタンを押した際にUIが出るか消えるかの処理
        if (Input.GetKeyDown("joystick button 7") && grayscript.enabled == false && !playerScript.IsGameOver() && !playerScript.IsGameClear() && !isChest || Input.GetKeyDown(KeyCode.Y) && grayscript.enabled == false && !playerScript.IsGameOver() && !playerScript.IsGameClear() && !isChest)
        {
            isExplanation = true; 
            pauseUI.SetActive(true);
            textmeshpro.SetActive(false);
            isMoveUI = false;
            PauseScript.Instance.ChangeState(PauseScript.GameState.Pause);
        }

        if (Input.GetKeyDown("joystick button 2")&&isExplanation || Input.GetKeyDown(KeyCode.Y)&&isExplanation)
        {
            ExplanationUI.SetActive(false);
            BackGround.SetActive(false);
            PauseScript.Instance.ChangeState(PauseScript.GameState.Pause);
            Keys.SetActive(false);
            textmeshpro.SetActive(true);
            pauseUI.SetActive(true);
        }
        //操作説明になった時のコントローラーの操作画面の状態
        if (!isMoveUI && isExplanation && PauseScript.Instance.currentState == PauseScript.GameState.Instructions)
        {
            BackGround.SetActive(true);
            ExplanationUI.SetActive(true);
            pauseUI.SetActive(false);
        }
        //--------------//
        //UIが出現する時次のページに行ったり戻ったり出来る処理
        //操作説明になった時のコントローラーの操作画面の状態に戻る処理
        if (Input.GetKeyDown(KeyCode.L) && PauseScript.Instance.currentState == PauseScript.GameState.Instructions || Input.GetKeyDown("joystick button 5") && PauseScript.Instance.currentState == PauseScript.GameState.Instructions)
        {
            ExplanationUI.SetActive(true);
            BackGround.SetActive(false);
            Keys.SetActive(true);
            isMoveUI = true;
        }
        //操作説明になった時のキーボードの操作画面の状態
        else if (Input.GetKeyDown(KeyCode.R) && PauseScript.Instance.currentState == PauseScript.GameState.Instructions || Input.GetKeyDown("joystick button 4") && PauseScript.Instance.currentState == PauseScript.GameState.Instructions)
        {
            ExplanationUI.SetActive(true);
            BackGround.SetActive(true);
            Keys.SetActive(false);
            isMoveUI = false;
        }
        //武器の効果確認画面の処理
        if (isExplanation && pauseScript.currentState == PauseScript.GameState.WeaponInstuctions)
        {
            BackGround.SetActive(false);
            ExplanationUI.SetActive(false);
            ExplanationUI.SetActive(false);
            weaponsUI[0].SetActive(true);
            LevelUpUI.SetActive(true);
            pauseUI.SetActive(false);
            Debug.Log("武器の説明");
        }
        if (pauseScript.currentState == PauseScript.GameState.WeaponInstuctions)
        {
            BackGround.SetActive(false);
            ExplanationUI.SetActive(false);
            pauseUI.SetActive(false);
        }
        if (Input.GetKey(KeyCode.B))
        {
            LevelUpUI.SetActive(true);
            pauseUI.SetActive(false);
        }
        //ポーズ画面で再開やタイトルに戻る場合の処理
        if (isExplanation && PauseScript.Instance.currentState == PauseScript.GameState.Playing || isExplanation && PauseScript.Instance.currentState == PauseScript.GameState.Title)
        {
            isExplanation = false;
            pauseUI.SetActive(false);
            islevelup = true;
            Time.timeScale = 1;
        }
        if (PauseScript.Instance != null && PauseScript.Instance.currentState == PauseScript.GameState.Title)
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
            if (chestsUI[i].activeSelf)
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
        if (IsKeyboardInput())
        {
            isInput = false;
            isKeyInput = true;
            explanationsUI[0].SetActive(true);
            explanationsUI[1].SetActive(true);
            explanationsUI[2].SetActive(false);
            explanationsUI[3].SetActive(false);
        }


        if (isExplanation || playerScript.IsGameOver() || playerScript.IsGameClear())
        {
            explanationsUI[0].SetActive(false);
            explanationsUI[1].SetActive(false);
            explanationsUI[2].SetActive(false);
            explanationsUI[3].SetActive(false);
        }

        if (playerScript.SetDeadCaunter() >= 5)
        {
            nextStageObject.SetActive(true);
        }
    }

    //敵が一体倒されて斧を拾うと表示されるUIをキーやボタンを押したら消す処理
    private IEnumerator ItemCoroutine()
    {
        Time.timeScale = 0;
        timeStopRespon = TimeStopRespon.ItemGet;
        LevelUpUI.SetActive(true);
        normalAxeData.isAxeExplocion = true;
        Debug.Log("レベルが2になった");

       
        yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.J) && playerScript.SetDeadCaunter() == 1
            || Input.GetKeyDown("joystick button 4") && playerScript.SetDeadCaunter() == 1);
        

        LevelUpUI.SetActive(false);
        islevelup = true;
        Time.timeScale = 1;
        if (timeStopRespon == TimeStopRespon.ItemGet)
        {
            Time.timeScale = 1;
            timeStopRespon = TimeStopRespon.None;
        }
    }

    private IEnumerator ChestCoroutine()
    {
        Time.timeScale = 0;
        timeStopRespon = TimeStopRespon.UIInteraction;
        isChest = true;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown("joystick button 4"));
        isChest = false;
        Time.timeScale = 1;
        if (timeStopRespon == TimeStopRespon.UIInteraction)
        {
            Time.timeScale = 1;
            timeStopRespon = TimeStopRespon.None;
        }
    }

    private IEnumerator ExplanationCoroutine()
    {
        Time.timeScale = 0;
        timeStopRespon = TimeStopRespon.Explanation;
        playerScript.StopPlayerMotion();
        foreach (GameObject ui in explanationsUI)
        {
            ui.SetActive(false);
        }
        //explanationsUI[0].SetActive(false);
        //explanationsUI[1].SetActive(false);
        //explanationsUI[2].SetActive(false);
        //explanationsUI[3].SetActive(false);
        yield return new WaitUntil(() => PauseScript.Instance.currentState == PauseScript.GameState.Playing || PauseScript.Instance.currentState == PauseScript.GameState.Title);
        playerScript.StartPlayerMotion();
        Time.timeScale = 1;
        if (timeStopRespon == TimeStopRespon.Explanation)
        {
            Time.timeScale = 1;
            timeStopRespon = TimeStopRespon.None;
        }
    }

    public bool IsKeyInput()
    {
        return isKeyInput;
    }

    public bool IsInput()
    {
        return isInput;
    }

    public void GameOverText()
    {
        if(isKeyInput)
        {
            gameOverTextsUI[1].SetActive(true);
        }
        else 
        {
            gameOverTextsUI[0].SetActive(true);
        }
    }

    public void GameClearText()
    {
        if (isKeyInput)
        {
            gameClearTextsUI[1].SetActive(true);
        }
        else
        {
            gameClearTextsUI[0].SetActive(true);
        }
    }

    // キーボードだけを対象にした入力チェック関数を用意
    bool IsKeyboardInput()
    {
        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
        {
            // キーボードのみに絞ったKeyCodeの範囲を指定する
            if ((code >= KeyCode.A && code <= KeyCode.Z) ||
                (code >= KeyCode.Alpha0 && code <= KeyCode.Alpha9) ||
                (code >= KeyCode.Keypad0 && code <= KeyCode.Keypad9) ||
                (code == KeyCode.Space || code == KeyCode.Return || code == KeyCode.Backspace))
            {
                if (Input.GetKeyDown(code))
                    return true;
            }
        }
        return false;
    }

   


}
