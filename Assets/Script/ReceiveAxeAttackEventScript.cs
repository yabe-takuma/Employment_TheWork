using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveAxeAttackEventScript : MonoBehaviour
{

    [SerializeField]
    private GameObject particle;
    [SerializeField]
    private Transform createShockwavePoint;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateAxeShockwave()
    {
        Instantiate(particle, new Vector3(transform.position.x-0.34f,transform.position.y+0.589f,transform.position.z), particle.transform.rotation);
    }
}
