using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
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
    public float dis_min = 5.0f;
    public float dis_mdl = 10.0f;
    [SerializeField]
    private Vector3 nowPos;
    private float nowRotAngle;
    private float nowHeightAngle;

    //減衰挙動
    public bool EnableAtten = true;
    public float AttenRate = 3.0f;
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

    
    // Start is called before the first frame update
    void Start()
    {
        nowPos = TargetObject.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        nowPos = TargetObject.transform.position;
        RotAngle -= speed.x * Time.deltaTime * 50.0f;
        HeightAngle += speed.z * Time.deltaTime * 20.0f;
        HeightAngle = Mathf.Clamp(HeightAngle, -40.0f, 60.0f);
        Distance = Mathf.Clamp(Distance, 5.0f, 40.0f);

        //減衰
        if (EnableAtten)
        {
            var target = TargetObject.transform.position;

            if(rock)
            {
                if(RockonTarget!=null)
                {
                    target = RockonTarget.transform.position;
                }
                else
                {
                    rock = false;
                }
            }
            var halfPoint = (TargetObject.transform.position + target) / 2;
            var deltaPos = halfPoint - prevTargetPos;
            prevTargetPos = halfPoint;
            deltaPos *= ForwardDistance;

            addForward += deltaPos * Time.deltaTime * 20.0f;
            addForward = Vector3.Lerp(addForward, Vector3.zero, Time.deltaTime * AttenRate);

        }
        else nowPos = TargetObject.transform.position + Vector3.up * Height;
        if (EnableAtten) nowRotAngle = Mathf.Lerp(nowRotAngle, RotAngle, Time.deltaTime * RotAngleAttenRate);
        else nowRotAngle = RotAngle;
        if (EnableAtten) nowHeightAngle = Mathf.Lerp(nowHeightAngle, HeightAngle, Time.deltaTime * RotAngleAttenRate);
        else nowHeightAngle = HeightAngle;

        if (rock)
        {
            var dis = Vector3.Distance(TargetObject.transform.position, RockonTarget.transform.position);
            if (HeightAngle > 30)
            {
                Distance = Mathf.Lerp(Distance, dis_mdl*dis/10 * HeightAngle / 30.0f, Time.deltaTime);
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

        var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * Distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * Distance;
        if (rock)
        {
            if (RockonTarget != null)
            {
                nowPos = RockonTarget.transform.position + new Vector3(cx, cy, cz);
            }
            
        }
        else
        {
             cx = Mathf.Sin(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * fixedDistance;
             cz = -Mathf.Cos(nowRotAngle * deg) * Mathf.Cos(nowHeightAngle * deg) * fixedDistance;
             cy = Mathf.Sin(nowHeightAngle * deg) * fixedDistance;
            transform.position = nowPos + new Vector3(cx, cy, cz);
        }
        transform.position = nowPos + new Vector3(cx, cy, cz);

        var rot = Quaternion.LookRotation((nowPos - transform.position).normalized);
        if (EnableAtten) transform.rotation = rot;
        else transform.rotation = rot;

    }
    public void OnCamera(InputAction.CallbackContext context)
    {
        speed = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
    }
    public void OnRockon(InputAction.CallbackContext context)
    {
        if(context.started)
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
}
