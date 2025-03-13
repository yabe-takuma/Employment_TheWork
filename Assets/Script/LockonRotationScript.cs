using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockonRotationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        //カメラと同じ向きに設定
        transform.rotation = Camera.main.transform.rotation;   
    }
}
