using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject icon;
    [SerializeField]
    private RectTransform imageTransform;

    public enum GameState
    {
        Pause,         //ポーズ中
        Playing,       //ゲーム再開
        Instructions,  //操作説明画面
        Title          //タイトルに戻る
    }
    public static PauseScript Instance { get; private set; } //シングルトン
    public GameState currentState { get; private set; } = GameState.Pause; //初期状態
    [SerializeField]
    private float lsv;
    //アイコンの移動にクールタイム
    [SerializeField]
    private int iconcooltime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    //ポーズ画面時剣のアイコンを移動させる処理
    public void MoveIcon()
    {
        lsv = Input.GetAxis("Vertical");
        iconcooltime++;
        Vector2 currentPosition = imageTransform.anchoredPosition;
        if (currentPosition.y == 0 && Input.GetKeyDown(KeyCode.DownArrow) || currentPosition.y == 0 && lsv <= 0.1f && lsv != 0 && iconcooltime >= 50) 
        {
            imageTransform.anchoredPosition -= new Vector2(0, 300);
            iconcooltime = 0;
            Debug.Log("Title");
        }
        else if (currentPosition.y == 0 && Input.GetKeyDown(KeyCode.UpArrow) || currentPosition.y == 0 && lsv >= 0.1f && lsv != 0 && iconcooltime >= 50)
        {
            imageTransform.anchoredPosition += new Vector2(0, 300);
            iconcooltime = 0;
        }
        else if (currentPosition.y == 300 && Input.GetKeyDown(KeyCode.DownArrow) || currentPosition.y == 300 && lsv <= 0.1f && lsv != 0 && iconcooltime >= 50)
        {
            imageTransform.anchoredPosition -= new Vector2(0, 300);
            iconcooltime = 0;
        }
        else if (currentPosition.y == -300 && Input.GetKeyDown(KeyCode.UpArrow) || currentPosition.y == -300 && lsv >= 0.1f && lsv != 0 && iconcooltime >= 50)
        {
            imageTransform.anchoredPosition += new Vector2(0, 300);
            iconcooltime = 0;
        }
        else if (currentPosition.y == -300 && Input.GetKeyDown(KeyCode.DownArrow) || currentPosition.y == -300 && lsv <= 0.1f && lsv != 0 && iconcooltime >= 50)
        {
            imageTransform.anchoredPosition += new Vector2(0, 600);
            iconcooltime = 0;
        }
        else if (currentPosition.y == 300 && Input.GetKeyDown(KeyCode.UpArrow) || currentPosition.y == 300 && lsv >= 0.1f && lsv != 0 && iconcooltime >= 50)
        {
            imageTransform.anchoredPosition -= new Vector2(0, 600);
            iconcooltime = 0;
        }
      
    }

    public void PushBotton()
    {
        Vector2 currentPosition = imageTransform.anchoredPosition;
        if (currentPosition.y == 300 && Input.GetKeyDown(KeyCode.Space)|| currentPosition.y == 300 && Input.GetKeyDown("joystick button 0"))
        {
            ChangeState(GameState.Playing);
            Debug.Log("再開");
        }
        else if(currentPosition.y == 0 && Input.GetKeyDown(KeyCode.Space)|| currentPosition.y == 0 && Input.GetKeyDown("joystick button 0"))
        {
            ChangeState(GameState.Instructions);
            Debug.Log("操作説明");
        }
        else if(currentPosition.y == -300 && Input.GetKeyDown(KeyCode.Space)|| currentPosition.y == -300 && Input.GetKeyDown("joystick button 0"))
        {
            ChangeState(GameState.Title);
            Debug.Log("タイトル");
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //シーンを跨いでも破棄されないようにする
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log($"Game State Changed: {currentState}");
    }

    
}
