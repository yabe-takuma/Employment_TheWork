using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTargetScript : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    protected void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            target = other.gameObject;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            target = null;
        }
    }

    public GameObject getTarget()
    {
        return this.target;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
