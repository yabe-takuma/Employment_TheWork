using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAxe : MonoBehaviour
{
    // Start is called before the first frame update
    private MyStatus myStatus;
    private PlayerScript playerscript;
    [SerializeField]
    private GameObject weakUI;
    [SerializeField]
    private GameObject axedamageUI;
    [SerializeField]
    private GameObject axenormaldamageUI;
    [SerializeField]
    private GameObject damageEffect;
    private bool isAttack;

    // Start is called before the first frame update
    private void Start()
    {
        myStatus = transform.root.GetComponent<MyStatus>();
        playerscript = transform.root.GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //敵に当たった時ダメージを表示するUIやエフェクトなどを表示しています。
        if (other.tag == "Enemy")
        {

            var enemyScript = other.GetComponent<MoveEnemyScript>();
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAxeAttackPower(), other.ClosestPointOnBounds(transform.position));
                var axedamageobj = Instantiate(axenormaldamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y, other.bounds.center.z), Quaternion.identity);
                axedamageobj.transform.SetParent(other.transform);
                Debug.Log("敵に当たった");
            }
        }
        //ボスに当たった時ダメージを表示するUIやエフェクトなどを表示しています。
        if (other.tag == "Boss")
        {
            var trollScript = other.GetComponentInParent<TrollScript>();
            if (trollScript.GetState() != TrollScript.TrollState.Dead && isAttack == false)
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAxeAttackPower()*2, other.ClosestPointOnBounds(transform.position));
                var weakobj = Instantiate(weakUI, new Vector3(other.bounds.center.x,other.bounds.center.y-2.0f,other.bounds.center.z), Quaternion.identity);
                weakobj.transform.SetParent(other.transform);
                var axedamageobj = Instantiate(axedamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 4.0f, other.bounds.center.z), Quaternion.identity);
                axedamageobj.transform.SetParent(other.transform);
                var damageobj = Instantiate(damageEffect, new Vector3(other.bounds.center.x, other.bounds.center.y, other.bounds.center.z), Quaternion.identity);
                damageobj.transform.SetParent(other.transform);
                isAttack = true;
                Debug.Log("ボスに当たった");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerscript.GetState() != PlayerScript.MyState.Attack)
        {
            isAttack = false;
        }
    }
}
