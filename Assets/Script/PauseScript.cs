using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject icon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void MoveIcon()
    {
        if (transform.position.y==0&&Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position = new Vector3(transform.position.x,300,transform.position.z);
            Debug.Log("Title");
        }
        else if (transform.position.y == 0 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position = new Vector3(transform.position.x, -300, transform.position.z);
        }
        else if (transform.position.y == 300 && Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else if (transform.position.y == -300 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        if(transform.position.y==0)
        {
            Debug.Log("pas");
        }
        Debug.Log("Pause");
    }
}
