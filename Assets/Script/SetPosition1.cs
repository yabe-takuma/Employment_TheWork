using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition1 : MonoBehaviour
{

    //初期位置
    private Vector3 startPosition;

    //目的地
    private Vector3 destination;

    //巡回する位置
    [SerializeField]
    private Transform[] patrolPositions;

    //次に巡回する位置
    private int nowPatrolPosition = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetNextPosition()
    {
        SetDestination(destination);
        nowPatrolPosition++;
        if(nowPatrolPosition >= patrolPositions.Length)
        {
            nowPatrolPosition = 0;
        }
    }

    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    public Vector3 GetDestination()
    {
        return destination;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
