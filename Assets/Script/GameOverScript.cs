using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{

    [SerializeField]
    private GameObject gameovertext;
    [SerializeField]
    private GameObject gameoverexplanationtext;
    [SerializeField]
    private PlayerScript playerscript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerscript.GetState()==PlayerScript.MyState.Dead)
        {
            gameovertext.SetActive(true);
            gameoverexplanationtext.SetActive(true);
        }
    }
}
