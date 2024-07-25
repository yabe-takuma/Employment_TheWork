using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerchAreaScript : MonoBehaviour
{
    private MoveEnemyScript MoveEnemy;
    // Start is called before the first frame update
    void Start()
    {
        MoveEnemy = GetComponentInParent<MoveEnemyScript>();
    }

    private void OnTriggerStay(Collider other)
    {
        //プレイヤーキャラクタを発見
        if (other.tag == "Player")
        {
            //敵
        }
    }
}
