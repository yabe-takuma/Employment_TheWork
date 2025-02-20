using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    [SerializeField]
    private PlayerScript playerScript;
    [SerializeField]
    private GrayScaleSprict grayscript;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        grayscript.enabled = false;
       
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    private void FixedUpdate()
    {
        if (playerScript.GetState() == PlayerScript.MyState.Dead)
        {
            grayscript.enabled = true;
        }
    }

   
}
