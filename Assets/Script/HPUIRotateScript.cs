using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUIRotateScript : MonoBehaviour
{

    private void LateUpdate()
    {
        //カメラと同じ向きに設定
        transform.rotation = Camera.main.transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
