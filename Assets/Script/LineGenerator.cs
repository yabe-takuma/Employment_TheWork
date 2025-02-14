using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    private GameObject line;

    private Vector3[] linetransform = new Vector3[5];

    // Start is called before the first frame update
    void Start()
    {
        linetransform[0] = new Vector3(-150f, 70f, 0f);
        linetransform[1] = new Vector3(-150f, 70f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject lines = Instantiate(line, line.transform.position, Quaternion.identity);
        }
    }
}
