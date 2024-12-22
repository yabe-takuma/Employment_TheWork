using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplocionOmenScript : MonoBehaviour
{
    private Vector3 omentimer;
    // Start is called before the first frame update
    void Start()
    {
        omentimer = new Vector3(0.1f, 0, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.z <= 50)
        {
            transform.localScale += omentimer;
          
        }
        if(transform.localScale.z>=50)
        {
            Destroy(this.gameObject,1f);
        }
    }
}
