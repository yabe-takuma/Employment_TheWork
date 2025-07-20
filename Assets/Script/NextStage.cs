using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    private string currentScene;
    [SerializeField]
    private bool isHit = false;
    private PlayerScript playerScript;
    private ParticleSystem ps; //略しています

    //次のステージに行くためにトロルのデータが入っていなかったら次に進むための変数
    [SerializeField]
    private GameObject troll;
    //次に進むためのボタンを表示するUI
    [SerializeField]
    private GameObject nextStageBottonUI;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        isHit = false;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        NextStageUpdate();
    }

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();


        int count = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        int exitCount = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);


        if (count > 0)
        {
            isHit = true;
            Debug.Log("パーティクルがヒット！");
        }
        if (exitCount > 0)
        {
            isHit = false;
            Debug.Log("パーティクルが離れた！");
        }



    }



    void NextStageUpdate()
    {

        currentScene = SceneManager.GetActiveScene().name;
        if (isHit && currentScene == "SampleScene"&&playerScript.SetDeadCaunter()>=5 && Input.GetKeyDown(KeyCode.Space) || 
            isHit && currentScene == "SampleScene" && playerScript.SetDeadCaunter() >= 5&&Input.GetKeyDown("joystick button 1"))
        {
            SceneManager.LoadScene("MiddleBossScene");
        }
        
        if (troll == null)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            Time.timeScale = 1;
        }
        if(isHit)
        {
            nextStageBottonUI.SetActive(true);
        }
        else
        {
            nextStageBottonUI.SetActive(false);
        }
      
    }
}
