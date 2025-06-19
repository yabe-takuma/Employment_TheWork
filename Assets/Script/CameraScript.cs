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



    // Start is called before the first frame update
    void Start()
    {
        nowPos = TargetObject.transform.position;
        playerScript = GameObject.Find("Character_Female_Hotel Owner").GetComponent<PlayerScript>();
        originalPosition = transform.position;
        StartCamera();
        isStartAnimation = false;
        offset = new Vector3(0, 2, -5);
        isCameraAngle = false;
        nowPos = TargetObject.transform.position + new Vector3(0, 2, -5); // ← 背後にオフセット


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
            speed = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
        }
    }
    public void OnRockon(InputAction.CallbackContext context)
    {
        if (!isStartAnimation) return;

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
                    //target = RockonTarget.transform.position;
                    //distance = Vector3.Distance(TargetObject.transform.position, RockonTarget.transform.position);
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
        if (!rock && isStartAnimation&&!justUnlocked)
        {
            //transform.position = nowPos + new Vector3(cx, cy, cz);
            //var rot = Quaternion.LookRotation((nowPos - transform.position).normalized);
            //transform.rotation = rot;
            float followSpeed = 5.0f;

            transform.position = Vector3.Lerp(transform.position, nowPos + new Vector3(cx, cy, cz), Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nowPos - transform.position), Time.deltaTime * followSpeed);


        }





        ////ロックオンじゃない時のカメラの回転
        //var rot = Quaternion.LookRotation((nowPos - transform.position).normalized);
        //if (!rock&&isStartAnimation) 

        if (rock && RockonTarget.tag == "Enemy")
        {
            initialRotation = rockonEnemyposition.transform.rotation;
            transform.rotation = initialRotation;
            transform.position = rockonEnemyposition.transform.position;

        }
        if (rock && RockonTarget.tag!="Enemy"&&!isCameraAngle)
        {
            transform.rotation = new Quaternion(transform.rotation.x,rockonEnemyposition.transform.rotation.y, rockonEnemyposition.transform.rotation.z, rockonEnemyposition.transform.rotation.w);
            AdjustCamera();
        }
        else if(rock && RockonTarget.tag != "Enemy"&&isCameraAngle)
        {
            transform.rotation = rockonEnemyposition.transform.rotation;
            transform.position = rockonEnemyposition.transform.position;
        }
        //ロックオンの時アイコンを表示する関数
        TargetIcon();
        //ボスが倒された時特定の座標に行く処理
        if (RockonTarget != null && trollscript.GetState() == TrollScript.TrollState.Dead)
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
        if(playerScript.GetState() == PlayerScript.MyState.Dead)
        {
            shakeMagnitude = 0.0f;
        }

        if(rock && RockonTarget.tag != "Enemy"&&Input.GetKeyDown(KeyCode.RightArrow)||
            rock && RockonTarget.tag != "Enemy" && speed.x<=-0.1f)
        {
            isCameraAngle = true;
        }
        else if(rock && RockonTarget.tag != "Enemy"&&Input.GetKeyDown(KeyCode.LeftArrow)||
                 rock && RockonTarget.tag != "Enemy" && speed.x>=0.1f)
        {
            isCameraAngle = false;
        }

        if (justLockedOn)
        {
            if (rockonCameraPosition != Vector3.zero)  // **(0, 0, 0) の場合は保存しない**
            {
                rockonCameraRotation = transform.rotation;
                rockonCameraPosition = transform.position;
            }

            Debug.Log("CameraUpdate()で正しく保存された回転: " + rockonCameraRotation);
            Debug.Log("CameraUpdate()で正しく保存された位置: " + rockonCameraPosition);
            justLockedOn = false;
        }




        if (justUnlocked) justUnlocked = false;






    }

    public void StartCamera()
    {
        transform.position = startCamera.transform.position;
        transform.rotation = startCamera.transform.rotation;
        Debug.Log("カメラ移動中");
    }

    public void StartCameraEnd()
    {
        if (!hasInitializedCamera)
        {
            StartCoroutine(StartCameraCoroutine());
            hasInitializedCamera = true;
        }
        Debug.Log("カメラを切り替える");


    }

    IEnumerator StartCameraCoroutine()
    {
        Debug.Log("StartCameraCoroutine 実行開始");
        transform.position = new Vector3(975, 0.4f, 45);
        yield return new WaitForSecondsRealtime(1.0f);
        isStartAnimation = true;
        transform.position = TargetObject.transform.position;
        transform.rotation = TargetObject.transform.rotation;
        Debug.Log("StartCameraCoroutine 実行終了: " + transform.position);


    }



    private void AdjustCamera()
    {
        //オブジェクトの境界ボックスを取得
        Bounds bounds = new Bounds(RockonTarget.transform.position, Vector3.zero);
        foreach(Renderer renderer in RockonTarget.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        //オブジェクトのサイズから最適な距離を計算
        float objectSize = bounds.extents.magnitude;
        float zoomFactor = 0.7f;  //調整用の倍率 (小さいほどカメラに近づく)
        float distance = (objectSize / Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2)) * zoomFactor;

        //カメラの位置を調整
        camera.transform.position = bounds.center - camera.transform.forward * (distance + 0.5f);
        camera.transform.LookAt(bounds.center);
    }

    IEnumerator ForceApplyRotation()
    {
        yield return new WaitForEndOfFrame();

        transform.position = rockonCameraPosition; // **ロックオン中に更新された位置を適用**
        transform.rotation = Quaternion.LookRotation((rockonCameraPosition - TargetObject.transform.position).normalized);

        Debug.Log("強制適用後の回転: " + transform.rotation);
        Debug.Log("強制適用後の位置: " + transform.position);







    }

    IEnumerator SaveRockonPositionNextFrame()
    {
        yield return new WaitForEndOfFrame();

        float deg = Mathf.Deg2Rad;
        float cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        float cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        float cy = Mathf.Sin(nowHeightAngle * deg) * Distance;

        Vector3 targetPos = nowPos + new Vector3(cx, cy, cz) + offset; // ← ここでオフセット適用

        rockonCameraRotation = transform.rotation;
        rockonCameraPosition = targetPos; // 位置を適切に修正

        Debug.Log("遅延保存: rockonCameraPosition = " + rockonCameraPosition);




    }



    IEnumerator SmoothReturnToPlayer()
    {
        float duration = 0.5f; // カメラが戻る時間（調整可能）
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        // `cx, cy, cz` をここで計算
        var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * Distance;


        Vector3 targetPos = nowPos + new Vector3(cx, cy, cz);

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // 最終的にプレイヤーの追従カメラへ戻す
    }

    IEnumerator SmoothTransitionToPlayer()
    {
        float transitionDuration = 1.0f;

        Vector3 initialPosition = transform.position; // **解除直後のカメラ位置**
        Vector3 offsetBehindPlayer = new Vector3(0, 2, -5);
        Vector3 desiredPosition = nowPos + offsetBehindPlayer;
        Quaternion desiredRotation = Quaternion.LookRotation(TargetObject.transform.position - desiredPosition);

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, desiredPosition, elapsedTime / transitionDuration);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("ロックオン解除: カメラ移行完了、通常追従モードへ");
    }





    IEnumerator SmoothTransitionToPreviousView()
    {
        float transitionDuration1 = 1.0f; // **背後へ移動する時間（秒）**
        float transitionDuration2 = 1.0f; // **元の視点へ戻る時間（秒）**

        var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * Distance;

        Vector3 offsetBehindPlayer = new Vector3(cx, cy, cz);
        Vector3 desiredPosition = nowPos + offsetBehindPlayer;
        Quaternion desiredRotation = Quaternion.LookRotation(TargetObject.transform.position - desiredPosition);

        // **① 徐々にプレイヤー背後へ移動**
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration1)
        {
            transform.position = Vector3.Lerp(preRockonPosition, desiredPosition, elapsedTime / transitionDuration1);
            transform.rotation = Quaternion.Slerp(preRockonRotation, desiredRotation, elapsedTime / transitionDuration1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // **② 一定時間後、元のロックオン前視点へ戻る**
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration2)
        {
            transform.position = Vector3.Lerp(desiredPosition, preRockonPosition, elapsedTime / transitionDuration2);
            transform.rotation = Quaternion.Slerp(desiredRotation, preRockonRotation, elapsedTime / transitionDuration2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // **③ その後、通常のカメラ追従モードへ**
        isTransitioning = false;
        Debug.Log("カメラ移行完了、通常追従モードへ");
    }

    IEnumerator SmoothTransitionToFollowMode()
    {
        float transitionDuration = 1.0f; // **背後へ移動する時間（秒）**

        var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * Distance;

        Vector3 offsetBehindPlayer = new Vector3(cx, cy, cz);
        Vector3 desiredPosition = nowPos + offsetBehindPlayer;
        Quaternion desiredRotation = Quaternion.LookRotation(TargetObject.transform.position - desiredPosition);

        // **① 徐々にプレイヤー背後へ移動**
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            transform.position = Vector3.Lerp(preRockonPosition, desiredPosition, elapsedTime / transitionDuration);
            transform.rotation = Quaternion.Slerp(preRockonRotation, desiredRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // **② 一定時間後、通常のプレイヤー追従モードへ**
        isFollowingPlayer = true;
        Debug.Log("カメラ移行完了、通常追従モードへ");
    }

    IEnumerator SmoothLockon()
    {
        float transitionDuration = 0.5f;

        Vector3 initialPosition = transform.position; // **ロックオン開始時のカメラ位置**
        Vector3 offsetBehindPlayer = new Vector3(0, 2, -5); // **プレイヤーの背後に固定**
        Vector3 targetPosition = TargetObject.transform.position + offsetBehindPlayer;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(RockonTarget.transform.position - targetPosition);

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            // **カメラの位置は維持しつつ、向きのみ変更**
            transform.position = targetPosition;
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("ロックオン: カメラ移行完了、ターゲット追従モード開始");
    }





    IEnumerator HideCameraDuringTransition()
    {
        float fadeDuration = 0.5f; // **カメラを一時的に消す時間**

        // **カメラをフェードアウト（または一時的に遠くへ移動）**
        camera.enabled = false; // **カメラを非表示**
        yield return new WaitForSeconds(fadeDuration);

        // **背後へ移動**
        Vector3 offsetBehindPlayer = new Vector3(0, 2, -5);
        transform.position = TargetObject.transform.position + offsetBehindPlayer;
        transform.rotation = Quaternion.LookRotation(TargetObject.transform.position - transform.position);

        yield return new WaitForSeconds(fadeDuration);

        // **カメラをフェードイン（または視界内へ復帰）**
        camera.enabled = true; // **カメラを表示**
        isTransitioning = false;

        Debug.Log("カメラ移行完了、通常追従モードへ");
    }

    IEnumerator SmoothLockonRoutine()
    {
        // カメラ移動前に今の位置と回転を保存
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // プレイヤー背後を基準に敵を見る角度へ移動
        Vector3 offset = new Vector3(0, 2, -5);
        Vector3 desiredPos = TargetObject.transform.position + offset;
        Quaternion lookAtEnemy = Quaternion.LookRotation(RockonTarget.transform.position - desiredPos);

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(RockonTarget.transform.position - transform.position);

        float t = 0;
        while (t < 0.1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t / 0.3f);
            t += Time.deltaTime;
            yield return null;
        }



        transform.position = desiredPos;
        transform.rotation = lookAtEnemy;

        // 最後にロックオンを有効にする
        rock = true;
        isLockonTransitioning = false;



        Debug.Log("ロックオン処理完了");
    }

    IEnumerator WaitThenStartLockon()
    {
        yield return null; // 1フレーム待つ（視点ジャンプを見せない）
        yield return new WaitForEndOfFrame(); // 念押しで固定してから回転開始

        StartCoroutine(SmoothLockonRoutine());
    }






}
