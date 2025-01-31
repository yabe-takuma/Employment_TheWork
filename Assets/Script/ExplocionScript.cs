using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplocionScript : MonoBehaviour
{

    private Vector3 explocionLimiter;
  
    private PlayerScript playerScript;

    private bool IsOnCollision;

   

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 3f);
        explocionLimiter = new Vector3(1, 1, 1);
        playerScript = GameObject.Find("Character_Female_Hotel Owner").GetComponent<PlayerScript>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.z<=30f)
        {
            transform.localScale += explocionLimiter;
        }
       
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"&&IsOnCollision==false&&playerScript.GetAvoid()==false)
        {
            playerScript.Damage(1);
            IsOnCollision = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsOnCollision = false;
    }

  

}
