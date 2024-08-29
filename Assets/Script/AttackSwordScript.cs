using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSwordScript : MonoBehaviour
{

    private MyStatus myStatus;

    // Start is called before the first frame update
    private void Start()
    {
        myStatus = transform.root.GetComponent<MyStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
           
            var enemyScript = other.GetComponent<MoveEnemyScript>();
            if (/*enemyScript.GetState() != MoveEnemyScript.EnemyState.Damage &&*/ enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(),other.ClosestPointOnBounds(transform.position));
                Debug.Log("敵に当たった");
            }
        }
        if(other.tag=="Boss")
        {
            var trollScript = other.GetComponent<TrollScript>();
            if(trollScript.GetState()!=TrollScript.TrollState.Dead)
            {
                other.GetComponent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                Debug.Log("ボスに当たった");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
