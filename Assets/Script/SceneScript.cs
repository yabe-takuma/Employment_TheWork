using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    
    public PlayerScript playerScript;  //プレイヤーの行動があるスクリプト
    
    public TrollScript trollScript;  //ボスの行動があるスクリプト
    [SerializeField]
    private GameObject troll; //ボスのオブジェクト
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバーになった時やゲームクリアになった時キーやボタンを押したらタイトルシーンに行くための処理
        if (playerScript.GetState() == PlayerScript.MyState.Dead && Input.GetKeyDown("joystick button 0") || 
            Input.GetKeyDown(KeyCode.K)&& playerScript.GetState() == PlayerScript.MyState.Dead)
        {
            SceneManager.LoadScene("TitleScene");
        }
        if (trollScript.GetState() == TrollScript.TrollState.Dead && Input.GetKeyDown("joystick button 3") && troll == null || 
            Input.GetKeyDown(KeyCode.K)&& trollScript.GetState() == TrollScript.TrollState.Dead&&troll == null)
        {
            SceneManager.LoadScene("TitleScene");
        }
       
    }

    public void Title()
    {
        SceneManager.LoadScene("TitleScene");
        Debug.Log("タイトルに戻る");
    }
}
