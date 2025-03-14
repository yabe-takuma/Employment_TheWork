using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScript : MonoBehaviour
{
    [SerializeField]
    private Material material;
    private float colorcounter;
    private bool isInput;
    // Start is called before the first frame update
    void Start()
    {
        material.SetFloat("_Threshold", 1.0f);
        colorcounter = 1.0f;
        isInput = false;
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1"))
        {
            isInput = true;
        }
        if(isInput)
        {
            colorcounter -= 0.004f;

            if (material.GetFloat("_Threshold") >= 0.0f)
            {
                material.SetFloat("_Threshold", colorcounter);
            }
            else
            {
                SceneManager.LoadScene("ExplanationScene");
            }
        }
        
    }

    public void OnScene(InputAction context)
    {

    }

}
