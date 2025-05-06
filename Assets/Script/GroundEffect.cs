using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            var playerScript = other.GetComponent<PlayerScript>();
            if(playerScript.GetState()!=PlayerScript.MyState.Dead)
            {
                other.GetComponent<PlayerScript>().Damage(1);
                
            }
            Debug.Log("地面がゆがむ");
        }
    }
}
