using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    
    public PlayerScript playerScript;
    
    public TrollScript trollScript;
    [SerializeField]
    private GameObject troll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.GetState() == PlayerScript.MyState.Dead && Input.GetKeyDown("joystick button 3") || 
            Input.GetKeyDown(KeyCode.K)&& playerScript.GetState() == PlayerScript.MyState.Dead)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else if (trollScript.GetState() == TrollScript.TrollState.Dead && Input.GetKeyDown("joystick button 3") && troll == null || 
            Input.GetKeyDown(KeyCode.K)&& trollScript.GetState() == TrollScript.TrollState.Dead&&troll == null)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
