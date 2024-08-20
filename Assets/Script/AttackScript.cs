using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Z_Arm") && other.tag == "Player")
        {
            Debug.Log("当たり");
            other.GetComponent <PlayerScript>().TakeDamage(transform.root,other.ClosestPoint(transform.position));
        }
       
    }

}
