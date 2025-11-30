using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;


//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    //カメラの操作スピード
    private Vector3 speed;
    //プレイヤー追従
    public GameObject TargetObject;
    public float Height = 1.5f;
    public float Distance = 5.0f;
    public float RotAngle = 0.0f;
    public float HeightAngle = 10.0f;
    public float dis_min = 2.0f;
    public float dis_mdl = 10.0f;
    [SerializeField]
    private Vector3 nowPos;
    private float nowRotAngle;
    private float nowHeightAngle;

    //減衰挙動
    public bool EnableAtten = true;
    public float AttenRate = 1.0f;
    public float ForwardDistance = 2.0f;
    private Vector3 addForward;
    [SerializeField]
    private Vector3 prevTargetPos;
    public float RotAngleAttenRate = 5.0f;
    public float AngleAttenRate = 1.0f;
    //private GameObject player;  //プレイヤー情報格納用
    private Vector3 offset;  //相対距離取得用

    //ロックオン機能
    public bool rock = false;
    private bool isRockonTransitioning;
    public GameObject RockonTarget;
    public GameObject SertchCircle;
    private const float fixedDistance = 5f;
    [SerializeField]
    private float distance;
    [SerializeField]
    private GameObject targetIcon;

    private Vector3 startposition;
    [SerializeField]
    private TrollScript trollscript;
    [SerializeField]
    private LayerMask obstacleLayer;

    private PlayerScript playerScript;
   //ロックオンの時ボスの時に追従するもの
    [SerializeField]
    private GameObject rockonposition;
    //ロックオンの時敵の時に追従するもの
    [SerializeField]
    private GameObject rockonEnemyposition;
    [SerializeField]
    private GameObject deadposition;
    [SerializeField]
    private GameObject rockonPlayer;

    private float cameraRotateSpeed = 45f;

    private float shakeDuration = 0.5f;  //揺れの時間
    private float shakeMagnitude = 0.1f; //揺れの強さ

    private Vector3 originalPosition;
    private float elapsedTime = 0f;

    //ゲーム開始時トロルに子関係にあるオブジェクトを参照
    [SerializeField]
    private GameObject startCamera;

    private bool isStartAnimation;

    //カメラ
    public Transform target;

    [SerializeField]
    private new Camera camera;

    private float angleSpeed=0.5f;

    private bool isCameraAngle;
    [SerializeField]
    private Quaternion rockonCameraRotation; //ロックオン時のカメラの回転を保存
    private Vector3 rockonCameraPosition; // ロックオン時のカメラ位置を保存

    private bool justLockedOn = false;
    private bool hasInitializedCamera = false;  // フラグを追加

    private bool isTransitioning = false;
    private float transitionTimer;
    private bool isFollowingPlayer;

    private Vector3 preRockonPosition;
    private Quaternion preRockonRotation;

    [SerializeField]
    private Quaternion initialRotation;  //初期ロックオン時の回転
    private bool isLockonTransitioning;
    private bool justUnlocked = false;

    private Vector3 unlockedPosition;
    private Quaternion unlockedRotation;
    private bool isUnlockTransitioning;
    private float unlockLerpTime;
    private bool isUnlockJustNow = false;
    private bool skipLerpOnce;
    private bool skipCameraUpdateOnce;
    private Vector3 velocity;
    [SerializeField]
    private MyStatus myStatus;

    private bool isWarpExplanation;

    private float followSpeed = 5.0f;
    //ワープが出現したか検知するための変数
    [SerializeField]
    private GameObject nextWarpStage;
    //ワープが出現したらカメラを別の位置に移動するための変数
    [SerializeField]
    private GameObject warpCameraPosition;
    //ボスのオブジェクトがnullかどうかを確認するための変数
    [SerializeField]
    private GameObject troll;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Character_Female_Hotel Owner").GetComponent<PlayerScript>();
        isStartAnimation = false;
        offset = new Vector3(0, 2, -5);
        isCameraAngle = false;
        skipLerpOnce = false;
        skipCameraUpdateOnce = false;
        

    }
    //カメラの揺れをelapsedTimeに代入する関数
    public void StartShake()
    {
        elapsedTime = shakeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        CameraUpdate();
    }
    public void OnCamera(InputAction.CallbackContext context)
    {
        if (trollscript.GetState() != TrollScript.TrollState.Dead)
        {
            speed = new Vector3(context.ReadValue<Vector2>().x * 3, 0f, context.ReadValue<Vector2>().y * 3);
        }
    }
    //特定のボタンを押されたらロックオンの処理が出来るようにフラグを立てる関数
    public void OnRockon(InputAction.CallbackContext context)
    {

        if (context.started && RockonTarget != null && trollscript.GetState() != TrollScript.TrollState.Dead)
        {
            if (rock)
            {
                rock = false;

              
            }
            else
            {
                rock = true;
            }


        }


    }
    public void IsRockon()
    {
        rock = false;
       
    }

    public bool IsStartAnimation()
    {
        return isStartAnimation;
    }

    public void GetRockonTarget(GameObject target)
    {
        RockonTarget = target;
      
    }
    public GameObject SetRockonTarget()
    {
        return RockonTarget;
    }
    //ロックオンされた敵に応じてアイコンの位置を変える処理
    private void TargetIcon()
    {
        if (rock && RockonTarget != null && RockonTarget.transform.GetChild(1) != null && RockonTarget.tag == "Enemy")
        {
            targetIcon.SetActive(true);
            targetIcon.transform.position = new Vector3(RockonTarget.transform.position.x,RockonTarget.transform.position.y+2.0f,RockonTarget.transform.position.z);
        }
        else if (rock && RockonTarget != null && RockonTarget.transform.GetChild(1) != null && RockonTarget.tag == "Troll")
        {
            targetIcon.SetActive(true);
            targetIcon.transform.position = new Vector3(RockonTarget.transform.position.x, RockonTarget.transform.position.y + 2.0f, RockonTarget.transform.position.z);
        }
        else
        {
            targetIcon.SetActive(false);
        }
    }

    void CameraUpdate()
    {

        if (skipCameraUpdateOnce)
        {
            skipCameraUpdateOnce = false;
            return; // 1フレームだけ完全にスキップ
        }



        //カメラの回転
        RotAngle -= speed.x * Time.deltaTime * 100.0f;
        HeightAngle += speed.z * Time.deltaTime * 50.0f;
        //カメラの距離や回転などの制限
        HeightAngle = Mathf.Clamp(HeightAngle, 3.7f, 60.0f);
        Distance = Mathf.Clamp(Distance, 5.0f, 15.0f);
        //あらかじめプレイヤーにコライダーを用意して当たったらこの変数に当たった敵を格納
        RockonTarget = SertchCircle.GetComponent<SensorScript>().nowTarget;
        //減衰
        if (EnableAtten)
        {
            var target = TargetObject.transform.position;
            //敵が一定範囲にいてrockフラグが経つと敵の座標などを渡す処理
            if (rock)
            {
                if (RockonTarget != null)
                {
                   
                    float lockonSpeed = 5.0f;


                    var deg2 = Mathf.Deg2Rad;
                    var cx2 = Mathf.Sin(nowRotAngle * deg2) * Mathf.Cos(nowHeightAngle * deg2) * Distance;
                    var cz2 = -Mathf.Cos(nowRotAngle * deg2) * Mathf.Cos(nowHeightAngle * deg2) * Distance;
                    var cy2 = Mathf.Sin(nowHeightAngle * deg2) * Distance;
                    // プレイヤー背後の位置に配置（カメラの位置はプレイヤー視点）
                    Vector3 offset = new Vector3(cx2, cy2, cz2);
                    Vector3 behindPlayer = TargetObject.transform.position + offset;

                    // 敵方向を見るように回転
                    Quaternion lookAtEnemy = Quaternion.LookRotation(RockonTarget.transform.position - behindPlayer);

                    // スムーズに移動・回転
                    transform.position = Vector3.Lerp(transform.position, behindPlayer, Time.deltaTime * lockonSpeed);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookAtEnemy, Time.deltaTime * lockonSpeed);



                }
                else
                {
                    rock = false;
                }

            }
           


            //減衰処理
            var halfPoint = (TargetObject.transform.position + target) / 2;
            var deltaPos = halfPoint - prevTargetPos;
            prevTargetPos = halfPoint;
            deltaPos *= ForwardDistance;

            addForward += deltaPos * Time.deltaTime * 20.0f;
            addForward = Vector3.Lerp(addForward, Vector3.zero, Time.deltaTime * AttenRate);

            nowPos = Vector3.Lerp(nowPos, halfPoint + Vector3.up * Height + addForward, Mathf.Clamp01(Time.deltaTime * AttenRate));
        }
        else nowPos = TargetObject.transform.position + Vector3.up * Height;
        if (EnableAtten) nowRotAngle = Mathf.Lerp(nowRotAngle, RotAngle, Time.deltaTime * RotAngleAttenRate);
        else nowRotAngle = RotAngle;
        if (EnableAtten) nowHeightAngle = Mathf.Lerp(nowHeightAngle, HeightAngle, Time.deltaTime * RotAngleAttenRate);
        else nowHeightAngle = HeightAngle;

        var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * Distance;

        //ロックオンじゃない時のカメラの座標
        if (!rock )
        {
           
           

            if (skipLerpOnce)
            {
                // 初期化後は1フレームだけ瞬間移動（スムーズ補間せずに直接追従）
                transform.position = nowPos + new Vector3(cx, cy, cz);
                transform.rotation = Quaternion.LookRotation(nowPos - transform.position);
                skipLerpOnce = false; // これで次フレームから通常処理に戻る
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, nowPos + new Vector3(cx, cy, cz), Time.deltaTime * followSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nowPos - transform.position), Time.deltaTime * followSpeed);
            }




        }


        if (rock && RockonTarget.tag == "Enemy")
        {
            transform.rotation = new Quaternion(transform.rotation.x, rockonEnemyposition.transform.rotation.y, rockonEnemyposition.transform.rotation.z, rockonEnemyposition.transform.rotation.w);
            AdjustEnemyCamera();
        }
        if (rock && RockonTarget.tag!="Enemy"&&!isCameraAngle)
        {
           
            transform.rotation = new Quaternion(transform.rotation.x, rockonEnemyposition.transform.rotation.y, rockonEnemyposition.transform.rotation.z, rockonEnemyposition.transform.rotation.w);
            AdjustCamera();
        }

        if(nextWarpStage.activeSelf&&!isWarpExplanation)
        {
            StartCoroutine(WarpExplanationCoroutine());
        }
      
        //ロックオンの時アイコンを表示する関数
        TargetIcon();
        //ボスが倒された時特定の座標に行く処理
        if (RockonTarget != null && trollscript.GetState() == TrollScript.TrollState.Dead&&isCameraAngle&&troll!=null)
        {
           
            transform.rotation = new Quaternion(transform.rotation.x, rockonEnemyposition.transform.rotation.y, rockonEnemyposition.transform.rotation.z, rockonEnemyposition.transform.rotation.w);
            AdjustCamera();
        }
        //変数が代入されることでカメラを揺らす処理
        if(elapsedTime >0&&playerScript.GetState()!=PlayerScript.MyState.Dead)
        {
            transform.position = transform.position + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsedTime -= Time.deltaTime;
        }
        //プレイヤーが死んでいる状態だったらカメラを揺らさない処理
        if(playerScript.GetState() == PlayerScript.MyState.Dead)
        {
            shakeMagnitude = 0.0f;
        }
        //右にスティックや右矢印キーを押すとボス全体を見れるモードに変更
        if(rock && RockonTarget.tag != "Enemy"&&Input.GetKeyDown(KeyCode.RightArrow)||
            rock && RockonTarget.tag != "Enemy" && speed.x<=-0.1f)
        {
            isCameraAngle = true;
        }
        //左にスティックや左矢印キーを押すとプレイヤーを見れるモードに変更
        else if (rock && RockonTarget.tag != "Enemy"&&Input.GetKeyDown(KeyCode.LeftArrow)||
                 rock && RockonTarget.tag != "Enemy" && speed.x>=0.1f)
        {
            isCameraAngle = false;
        }

        if (justLockedOn)
        {
            if (rockonCameraPosition != Vector3.zero)  //(0, 0, 0) の場合は保存しない
            {
                rockonCameraRotation = transform.rotation;
                rockonCameraPosition = transform.position;
            }

            Debug.Log("CameraUpdate()で正しく保存された回転: " + rockonCameraRotation);
            Debug.Log("CameraUpdate()で正しく保存された位置: " + rockonCameraPosition);
            justLockedOn = false;
        }

        if (justUnlocked) justUnlocked = false;
        if (!hasInitializedCamera)
        {
            StartCoroutine(StartCameraCoroutine());
            hasInitializedCamera = true;
        }
    }
   

    IEnumerator StartCameraCoroutine()
    {
        Debug.Log("StartCameraCoroutine 実行開始");
        yield return new WaitForSecondsRealtime(1.0f);
        isStartAnimation = true;

        // 角度を正しい値に初期化する
        RotAngle = 0f;
        HeightAngle = 15f;
        nowRotAngle = RotAngle;
        nowHeightAngle = HeightAngle;

        // nowPos を計算
        var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * Distance;
        nowPos = TargetObject.transform.position + new Vector3(cx, cy, cz);

        // カメラ位置と回転を即設定
        transform.position = nowPos;
        transform.rotation = Quaternion.LookRotation(TargetObject.transform.position - nowPos);

        // Lerp 無効化のためのフラグ
        skipCameraUpdateOnce = true;
        skipLerpOnce = true;
        Debug.Log("StartCameraCoroutine 実行終了: " + transform.position);
        



    }

    IEnumerator WarpExplanationCoroutine()
    {
        transform.position = warpCameraPosition.transform.position;
        transform.rotation = warpCameraPosition.transform.rotation;
        yield return new WaitForSecondsRealtime(3.0f);
        isWarpExplanation = true;

        var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * Distance;
        transform.position = Vector3.Lerp(transform.position, nowPos + new Vector3(cx, cy, cz), Time.deltaTime * followSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nowPos - transform.position), Time.deltaTime * followSpeed);
    }


    //ボス姿の全体を見れる処理
    private void AdjustCamera()
    {


        if (RockonTarget == null || TargetObject == null) return;

        // プレイヤーと敵の位置を取得
        Vector3 playerPos = TargetObject.transform.position + Vector3.up * 1.5f;
        Vector3 enemyPos = RockonTarget.transform.position + Vector3.up * 1.5f;

        // 中間点
        Vector3 midPoint = (playerPos + enemyPos) * 0.5f;

        // ロックオン対象のサイズを取得
        Bounds bounds = new Bounds(RockonTarget.transform.position, Vector3.zero);
        foreach (Renderer renderer in RockonTarget.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }
        float targetSize = bounds.extents.magnitude;

        // カメラ方向と距離の調整
        Vector3 direction = (midPoint - enemyPos).normalized;
        float baseDistance = Vector3.Distance(playerPos, enemyPos);
        float dynamicDistance = Mathf.Max(baseDistance + 3.0f, targetSize * 2.0f);

        // カメラ位置を計算（高さ補正込み）
        Vector3 cameraPos = midPoint + direction * dynamicDistance + Vector3.up * 2.5f;
        cameraPos.y = Mathf.Max(cameraPos.y, 1.0f); // 地面以下にならないよう補正

        // カメラ移動・注視
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, cameraPos, ref velocity, 0.15f);
        camera.transform.LookAt(midPoint);



    }

    


    //敵の姿の全体を見れる処理
    private void AdjustEnemyCamera()
    {
        if (RockonTarget == null || TargetObject == null) return;

        Vector3 playerPos = TargetObject.transform.position + Vector3.up * 1.5f;
        Vector3 enemyPos = RockonTarget.transform.position + Vector3.up * 1.5f;

        Vector3 directionToEnemy = (enemyPos - playerPos).normalized;
        float distance = Vector3.Distance(playerPos, enemyPos);

        // プレイヤーの背後から敵を見るようにカメラ位置を調整
        Vector3 cameraOffset = -directionToEnemy * Mathf.Clamp(distance * 0.8f, 5f, 12f) + Vector3.up * 3.5f;
        Vector3 desiredCameraPos = playerPos + cameraOffset;

        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, desiredCameraPos, ref velocity, 0.1f);

        // プレイヤーと敵の間を常に注視
        Vector3 lookTarget = (playerPos + enemyPos) * 0.5f;
        camera.transform.LookAt(lookTarget);



    }
    //ボスが攻撃しているかの関数
    private bool IsTrollAttack()
    {
        return trollscript.GetShockwave() || trollscript.GetIsWave()
            || trollscript.GetExplocion() || trollscript.GetInstallation();
    }

   
}
