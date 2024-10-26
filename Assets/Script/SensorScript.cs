using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SensorScript : MonoBehaviour
{
    public GameObject nowTarget;
    [SerializeField]
    private List<GameObject> enemyList;
    private Camera cam;
    [SerializeField]
    private CameraScript camerascript;
    // Start is called before the first frame update
    void Start()
    {
        nowTarget = null;
        enemyList = new List<GameObject>();
        cam = Camera.main;
        camerascript = cam.GetComponent<CameraScript>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && !enemyList.Contains(other.gameObject))
        {
            enemyList.Add(other.gameObject);
            if (other.tag == null)
            {
                nowTarget = other.gameObject;
                
            }
        }
        camerascript.GetRockonTarget(nowTarget);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" && !enemyList.Contains(other.gameObject))
        {
            if (other.tag == null)
            {
                nowTarget = null;
            }
            enemyList.Remove(other.gameObject);
        }
       
            camerascript.IsRockon();
        
    }


    // Update is called once per frame
    void Update()
    {
        if(enemyList.Count==0)
        {
            nowTarget = null;
            return;
        }
        else if(enemyList.Count!=0&&nowTarget==null)
        {
            SetNowTarget();
        }
    }

    public GameObject GetNowTarget()
    {
        return nowTarget;
    }

    public void SetNowTarget()
    {
        foreach (var enemy in enemyList)
        {
            if(nowTarget == null)
            {
                nowTarget = enemy;
            }
        }
    }

    public void OnRockonSwitch(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(enemyList.IndexOf(nowTarget)!=enemyList.Count-1)
            {
                nowTarget = enemyList[enemyList.IndexOf(nowTarget) + 1];
            }
            else
            {
                nowTarget = enemyList[0];
            }
        }
    }
}
