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

    [SerializeField]
    private GameObject rockonplayer;

    private Vector3 startposition;
    [SerializeField]
    private TrollScript trollscript;
    [SerializeField]
    private LayerMask obstacleLayer;

    private PlayerScript playerScript;
   
    [SerializeField]
    private GameObject rockonposition;

    // Start is called before the first frame update
    void Start()
    {
        nowPos = TargetObject.transform.position;
        playerScript = GameObject.Find("Character_Female_Hotel Owner").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RotAngle -= speed.x * Time.deltaTime * 50.0f;
        HeightAngle += speed.z * Time.deltaTime * 50.0f;
     
        HeightAngle = Mathf.Clamp(HeightAngle, 3.7f, 60.0f);
        Distance = Mathf.Clamp(Distance, 5.0f, 15.0f);

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
        //プレイヤーとカメラの距離を調整する処理
        if (rock)
        {
            var dis = Vector3.Distance(TargetObject.transform.position, RockonTarget.transform.position);
            if (HeightAngle > 30)
            {
                Distance = Mathf.Lerp(Distance, dis_mdl * dis / 10 * HeightAngle / 30.0f, Time.deltaTime);
            }
            else if (HeightAngle <= 30 && HeightAngle >= 3)
            {
                Distance = Mathf.Lerp(Distance, dis_mdl * dis / 10, Time.deltaTime);
            }
            else if (HeightAngle < -3)
            {
                rock = false;
            }
        }
        else
        {
            if (HeightAngle > 30)
            {
                Distance = Mathf.Lerp(Distance, 5.0f * HeightAngle / 30.0f, Time.deltaTime);
            }
            else if (HeightAngle <= 30 && HeightAngle >= -3)
            {
                Distance = Mathf.Lerp(Distance, 5.0f, Time.deltaTime);
            }
            else if (HeightAngle < -3)
            {
                Distance = Mathf.Lerp(Distance, dis_min, Time.deltaTime);
            }
        }
        var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * Distance;
        //ロックオンじゃない時のカメラの座標
        if(!rock)
        {
            transform.position = nowPos + new Vector3(cx, cy, cz);
        }
        //ロックオンじゃない時のカメラの回転
        var rot = Quaternion.LookRotation((nowPos - transform.position).normalized);
        if (!rock) transform.rotation = rot;
        //ロックオン時のカメラの座標と回転
        if (rock)
        {
            transform.rotation = rockonposition.transform.rotation;
            transform.position = rockonposition.transform.position;
        }
        //ロックオンの時アイコンを表示する関数
        TargetIcon();
        //ボスが倒された時特定の座標に行く処理
        if (trollscript.GetState() == TrollScript.TrollState.Dead)
        {
            transform.position = new Vector3(transform.position.x, 3.0f, transform.position.z - 5.0f);
        }
       
    }

    void Update()
    {
        
    }
    public void OnCamera(InputAction.CallbackContext context)
    {
        speed = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
    }
    public void OnRockon(InputAction.CallbackContext context)
    {
        if(context.started&& RockonTarget != null)
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
    //ロックオンされた敵に応じてアイコンの位置を変える処理
    private void TargetIcon()
    {
        if (rock && RockonTarget != null && RockonTarget.transform.GetChild(1) != null && RockonTarget.tag == "Enemy")
        {
            targetIcon.SetActive(true);
            targetIcon.transform.position = new Vector3(RockonTarget.transform.position.x,RockonTarget.transform.position.y+2.0f,RockonTarget.transform.position.z);
        }
        else if (rock && RockonTarget != null && RockonTarget.transform.GetChild(1) != null && RockonTarget.tag == "Boss")
        {
            targetIcon.SetActive(true);
            targetIcon.transform.position = new Vector3(RockonTarget.transform.position.x, RockonTarget.transform.position.y + 2.0f, RockonTarget.transform.position.z);
        }
        else
        {
            targetIcon.SetActive(false);
        }
    }
}
