using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAxe : MonoBehaviour
{
    // Start is called before the first frame update
    private MyStatus myStatus;
    [SerializeField]
    private GameObject weakUI;
    [SerializeField]
    private GameObject axedamageUI;
    [SerializeField]
    private GameObject axenormaldamageUI;

    // Start is called before the first frame update
    private void Start()
    {
        myStatus = transform.root.GetComponent<MyStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {

            var enemyScript = other.GetComponent<MoveEnemyScript>();
            if (/*enemyScript.GetState() != MoveEnemyScript.EnemyState.Damage &&*/ enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAxeAttackPower(), other.ClosestPointOnBounds(transform.position));
                var axedamageobj = Instantiate(axenormaldamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y, other.bounds.center.z), Quaternion.identity);
                axedamageobj.transform.SetParent(other.transform);
                Debug.Log("敵に当たった");
            }
        }
        if (other.tag == "Boss")
        {
            var trollScript = other.GetComponent<TrollScript>();
            if (trollScript.GetState() != TrollScript.TrollState.Dead)
            {
                other.GetComponent<TrollScript>().TakeDamage(myStatus.GetAxeAttackPower()*2, other.ClosestPointOnBounds(transform.position));
                var weakobj = Instantiate(weakUI, new Vector3(other.bounds.center.x,other.bounds.center.y-2.0f,other.bounds.center.z), Quaternion.identity);
                weakobj.transform.SetParent(other.transform);
                var axedamageobj = Instantiate(axedamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 4.0f, other.bounds.center.z), Quaternion.identity);
                axedamageobj.transform.SetParent(other.transform);
                Debug.Log("ボスに当たった");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
