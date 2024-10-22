using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotateScript : MonoBehaviour
{
    [Header("追従するゲームオブジェクト")]
    public GameObject targetObj;

    private Vector3 targetPos;

    [SerializeField]
    private float cameraRotateSpeed = 200f;

    [SerializeField]
    private float maxLimit = 30.0f;

    private float minLimit;


    [SerializeField]
    private LockOnTarget lockOnTarget;

    private GameObject locktarget;
    private float ANGLE_LIMIT_DOWN;
    private float ANGLE_LIMIT_UP;



    // Start is called before the first frame update
    void Start()
    {
        targetPos = targetObj.transform.position;

        minLimit = 360 - maxLimit;
        ANGLE_LIMIT_DOWN = -100;
        ANGLE_LIMIT_UP = 100;

    }

    // Update is called once per frame
    void Update()
    {

        if (targetObj != null)
        {

            transform.position += targetObj.transform.position - targetPos;

            targetPos = targetObj.transform.position;

        }


        if (Input.GetKey(KeyCode.C))
        {
            RotateCamera();
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            RotateCamera();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            RotateCamera();
        }
        else if (Input.GetKey(KeyCode.X))
        {
            RotateCamera();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            GameObject target = lockOnTarget.getTarget();

            if (target!=null)
            {
                locktarget = target;
            }
            else
            {
                
                locktarget = null;
            }
        }

        if(locktarget)
        {
            lockOnTargetObject(locktarget);
        }else
        {
            if(Input.GetKey(KeyCode.U))
            {
                RotateCamera();
            }
        }

        float angle_x = 360f <= transform.eulerAngles.x ? transform.eulerAngles.x - 720 : transform.eulerAngles.x;
        transform.eulerAngles = new Vector3(
            Mathf.Clamp(angle_x, ANGLE_LIMIT_DOWN, ANGLE_LIMIT_UP),
            transform.eulerAngles.y,
            transform.eulerAngles.z);

    }

    private void lockOnTargetObject(GameObject target)
    {
        transform.LookAt(target.transform, Vector3.up);
    }

    private void RotateCamera()
    {

        float x = Input.GetAxis("Fire1");
        float z = Input.GetAxis("Fire2");
        

        transform.RotateAround(targetObj.transform.position, Vector3.up, x * Time.deltaTime * cameraRotateSpeed);

        var localAngle = transform.localEulerAngles;

        localAngle.x += z;

        if(localAngle.x>maxLimit&&localAngle.x<180)
        {
            localAngle.x = maxLimit;
        }

        if(localAngle.x<minLimit&&localAngle.x>180)
        {
            localAngle.x = minLimit;
        }

        transform.localEulerAngles = localAngle;

    }

    //void LateUpdate()
    //{
    //    transform.position = Vector3.Lerp(this.transform.position, target.transform.position - diff, followSpeed); //線形補間関数によるカメラの移動
    //}

}
