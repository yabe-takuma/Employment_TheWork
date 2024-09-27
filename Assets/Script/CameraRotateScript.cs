using System.Collections;
using System.Collections.Generic;
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



   
    
    // Start is called before the first frame update
    void Start()
    {
        targetPos = targetObj.transform.position;

        minLimit = 360 - maxLimit;
       
    }

    // Update is called once per frame
    void Update()
    {
       
        if(targetObj!= null)
        {

            transform.position += targetObj.transform.position - targetPos;

            targetPos = targetObj.transform.position;
            
        }
      

        if(Input.GetKey(KeyCode.C))
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
