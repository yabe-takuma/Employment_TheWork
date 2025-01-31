using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    [SerializeField]
    private PlayerScript playerScript;
    [SerializeField]
    private GrayScaleSprict grayscript;
    [SerializeField]
    private bool isGrayScale;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        grayscript.enabled = false;
        isGrayScale = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScript.GetState()==PlayerScript.MyState.Dead)
        {
            isGrayScale = true;
        }
        
    }

    private void FixedUpdate()
    {
        if (isGrayScale)
        {
            grayscript.enabled = true;
            Time.timeScale = 0.1f;
        }
    }
}
