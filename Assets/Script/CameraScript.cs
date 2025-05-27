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
        if(context.started&& RockonTarget != null&&trollscript.GetState()!=TrollScript.TrollState.Dead)
        {
            if(rock)
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
                    target = RockonTarget.transform.position;
                    distance = Vector3.Distance(TargetObject.transform.position, RockonTarget.transform.position);
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
        if (!rock&&isStartAnimation)
        {
            transform.position = nowPos + new Vector3(cx, cy, cz);
        }
        //ロックオンじゃない時のカメラの回転
        var rot = Quaternion.LookRotation((nowPos - transform.position).normalized);
        if (!rock&&isStartAnimation) transform.rotation = rot;
      
        if(rock && RockonTarget.tag == "Enemy")
        {
            transform.rotation = rockonEnemyposition.transform.rotation;
            transform.position = rockonEnemyposition.transform.position;
        }
        if(rock && RockonTarget.tag!="Enemy"&&!isCameraAngle)
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
        
    }

    public void StartCamera()
    {
        transform.position = startCamera.transform.position;
        transform.rotation = startCamera.transform.rotation;
        Debug.Log("カメラ移動中");
    }

    public void StartCameraEnd()
    {
        if (!isStartAnimation)
        {
            StartCoroutine("StartCameraCoroutine");
        }
       
        Debug.Log("カメラを切り替える");
    }

    IEnumerator StartCameraCoroutine()
    {
        transform.position = new Vector3(975, 0.4f, 45);
        yield return new WaitForSecondsRealtime(1.0f);
        isStartAnimation = true;
        transform.position = TargetObject.transform.position;
        transform.rotation = TargetObject.transform.rotation;
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
}
