using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeParticle : MonoBehaviour
{

    [SerializeField]
    private GameObject particle;

    private int caunter;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        caunter++;
        if(caunter>500)
        {
            Instantiate(particle, transform.position, particle.transform.rotation);
            caunter = 0;
        }
    }
}
