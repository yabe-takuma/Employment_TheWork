using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField]
    private GameObject line;
    private LineRenderer[] renderer=new LineRenderer[5];


    private Vector3 poss;
   
    // Start is called before the first frame update
    
    void Start()
    {
        line = new GameObject();
        
        poss = new Vector3();
        //LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        for (int i = 0; i < 5; i++)
        {
            renderer[i] = gameObject.GetComponent<LineRenderer>();
            renderer[i].positionCount = 2;
        }
        //renderer[0] = gameObject.GetComponent<LineRenderer>();
        //renderer[0].positionCount = 2;

        //頂点を設定
        renderer[0].SetPosition(0, new Vector3(170f, 100f, 0f));
        renderer[0].SetPosition(1, new Vector3(200f, 100f, -300f));
        //renderer[1].SetPosition(0, new Vector3(-150f, 70f, 0f));
        //renderer[1].SetPosition(1, new Vector3(-200f, 100f, -300f));
        //renderer.SetPosition(1, poss[1]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
