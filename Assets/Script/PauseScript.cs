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
    public float moveSpeed = 50f;
    public float[] yPositions = { 300f, 0f, -300f }; // 上・中央・下のY座標
    private int currentIndex = 1; // 最初は中央（y=0）
    [SerializeField]
    private Vector2 targetPosition;



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

    [SerializeField] private float axisCoolTime = 0.3f;
    private float axisTimer = 0f;

    private bool isUp,isDown;
    [SerializeField]
    private bool isMoving = false;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"[確認] imageTransform の名前: {imageTransform?.name}");

        // 初期位置の設定（←ここが重要！）
        if (imageTransform != null)
        {
            float startX = imageTransform.anchoredPosition.x;
            targetPosition = new Vector2(startX, yPositions[currentIndex]);
            imageTransform.anchoredPosition = targetPosition;
        }


    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    imageTransform.anchoredPosition = new Vector2(0, -300f);
        //    Debug.Log("強制移動したぞ！");
        //}

        //if (currentState == GameState.Pause)
        //{
        //    MoveIcon();
        //    PushBotton();

        //    if (isMoving)
        //    {
        //        Debug.Log($"[Lerp中] 現在: {imageTransform.anchoredPosition}, 目標: {targetPosition}");

        //        imageTransform.anchoredPosition = Vector2.MoveTowards(
        //           imageTransform.anchoredPosition,
        //           targetPosition,
        //           Time.deltaTime * moveSpeed
        //        );


        //        if (Vector2.Distance(imageTransform.anchoredPosition, targetPosition) < 0.1f)
        //        {
        //            imageTransform.anchoredPosition = targetPosition;
        //            isMoving = false;
        //            Debug.Log("🔚 目的地に到達、isMoving = false");
        //        }
        //    }


        //}


        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    imageTransform.anchoredPosition = new Vector2(0, 300f);
        //    Debug.Log("Zキーで強制配置したぞ！");
        //}



        //Debug.DrawLine(Vector3.zero, imageTransform.position, Color.green);


    }

    public void MoveIcon2()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) && currentIndex < yPositions.Length - 1)
            {
                currentIndex++;
                float currentX = imageTransform.anchoredPosition.x;
                targetPosition = new Vector2(currentX, yPositions[currentIndex]);
                isMoving = true;
                Debug.Log($"▼ DOWN：currentIndex={currentIndex} | targetY={targetPosition.y}");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && currentIndex > 0)
            {
                currentIndex--;
                float currentX = imageTransform.anchoredPosition.x;
                targetPosition = new Vector2(currentX, yPositions[currentIndex]);
                isMoving = true;
                Debug.Log($"▲ UP：currentIndex={currentIndex} | targetY={targetPosition.y}");
            }
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

    public void MoveIcon()
    {

        lsv = Input.GetAxis("Vertical");
        iconcooltime++;
        Vector2 currentPosition = imageTransform.anchoredPosition;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isDown = true;
        }
        if (currentPosition.y == 0 && Input.GetKeyDown(KeyCode.DownArrow) || currentPosition.y == 0 && lsv <= 0.1f && lsv != 0 && iconcooltime >= 50)
        {
            imageTransform.anchoredPosition -= new Vector2(0, 300);
            iconcooltime = 0; Debug.Log("Title");
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


        imageTransform.anchoredPosition = Vector2.Lerp(imageTransform.anchoredPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

}
