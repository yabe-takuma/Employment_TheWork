using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSwordScript : MonoBehaviour
{

    private MyStatus myStatus;
    [SerializeField]
    private GameObject sworddamageUI;

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
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y-1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                Debug.Log("敵に当たった");
            }
        }
        if(other.tag=="Boss")
        {
            var trollScript = other.GetComponent<TrollScript>();
            if(trollScript.GetState()!=TrollScript.TrollState.Dead)
            {
                other.GetComponent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y-4.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                Debug.Log("ボスに当たった");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
