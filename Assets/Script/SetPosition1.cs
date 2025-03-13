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
    private int nowPatrolPosition = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //次の目的地の作成
    public void SetNextPosition()
    {
        SetDestination(destination);
        nowPatrolPosition++;
        if(nowPatrolPosition >= patrolPositions.Length)
        {
            nowPatrolPosition = 0;
        }
    }
    //目的地を設定
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }
    //目的地を取得
    public Vector3 GetDestination()
    {
        return destination;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
