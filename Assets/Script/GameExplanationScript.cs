using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExplanationScript : MonoBehaviour
{
    [SerializeField]
    private bool isExplanation;
    [SerializeField]
    private GameObject ExplanationUI;
    [SerializeField]
    private GameObject BackGround;
    [SerializeField]
    private GameObject GameUI;
    // Start is called before the first frame update
    void Start()
    {
        isExplanation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("joystick button 2")&&isExplanation==false|| Input.GetKeyDown(KeyCode.Y)&&isExplanation==false)
        {
            isExplanation = true;
            BackGround.SetActive(true);
            ExplanationUI.SetActive(true);
            GameUI.SetActive(true);
        }
        else if(Input.GetKeyDown("joystick button 2") && isExplanation == true || Input.GetKeyDown(KeyCode.Y) && isExplanation == true)
        {
            isExplanation = false;
            ExplanationUI.SetActive(false);
            BackGround.SetActive(false);
            GameUI.SetActive(false);
        }
        if(isExplanation==true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
