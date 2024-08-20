using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerchAreaScript : MonoBehaviour
{
    private MoveEnemyScript moveEnemy;
    [SerializeField]
    private int timer;
    // Start is called before the first frame update
    void Start()
    {
        moveEnemy = GetComponentInParent<MoveEnemyScript>();
    }

    void OnTriggerStay(Collider other)
    {
        //プレイヤーキャラクタを発見
        if (other.tag == "Player")
        {
            //敵キャラクターの状態を発見
            MoveEnemyScript.EnemyState state = moveEnemy.GetState();
            //敵キャラクターが追いかける状態でなければ追いかける設定に変更
            if(state == MoveEnemyScript.EnemyState.Wait||state == MoveEnemyScript.EnemyState.Walk)
            {
                Debug.Log("プレイヤー発見");
                moveEnemy.SetState(MoveEnemyScript.EnemyState.Chase, other.transform);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("見失う");
            moveEnemy.SetState(MoveEnemyScript.EnemyState.Wait);
            
        }
        if (other.tag == "Tree")
        {
            
            moveEnemy.SetState(MoveEnemyScript.EnemyState.Wait);

        }
    }
}
