using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TitleCameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private bool isEvent;
    [SerializeField]
    private PlayableDirector timeline;
    [SerializeField]
    private GameObject textUI;
    [SerializeField]
    private GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        isEvent = false;
        timeline.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z < 192)
        {
            transform.position = camera.transform.position;
        }
        if(player.transform.position.z>192&&!isEvent)
        {
           timeline.Play();
        }
        if(transform.position.z <= 188f)
        {
            isEvent = true;
        }
        if(timeline.time>=timeline.duration)
        {
            textUI.SetActive(true);
            panel.SetActive(true);
        }
        
    }
}
